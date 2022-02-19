using Microsoft.Extensions.Configuration;
using Serilog;
using smart_backup.DTO;
using smart_backup.Elaborate;
using smart_backup.Model;
using smart_backup.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SmartBackup.service
{
    public class Business
    {
        private static string _programDirectory = Directory.GetCurrentDirectory();
        private static string _pathAppSettings = _programDirectory + "/Config/appsettings.json";
        private static string _tempClone;

        private static GitLogin _gitLogin;
        private static FolderSetting _folderSettings;
        private static EmailSetting _emailSettings;

        static private List<String> ListOrganization;
        static private LogMailModel logMail = new LogMailModel();
        private static string _pathLogMail = _programDirectory + "/Logs/logmail.txt";


        internal void Run()
        {
            LoadConfig();

            _tempClone = _folderSettings.rootBackup + "/" + _folderSettings.nameBackup;

            Log.Information("Application Started");

            //File.Delete("ListCMD.txt");
            Elaborate.CleanFolderTemp(_tempClone);

            CheckOrganizationList();

            logMail.AddLine(_folderSettings.nameBackup, "file .zip","", Elaborate.ZipTheFolder(_tempClone));

            Elaborate.CleanFolderTemp(_tempClone);

            File.WriteAllText(_pathLogMail, logMail.dataBackup.ToString() + ";inizio backup" + Environment.NewLine);

            using (StreamWriter stream = File.AppendText(_pathLogMail))
            {
                foreach (var line in logMail.lines)
                {
                    stream.WriteLine($"{line.data};{line.nomeOrg};{line.nomeProject};{line.nomeRepo};{line.esito};{line.messaggio}");
                }
            }

            Elaborate.SendMail(_emailSettings, _pathLogMail).Wait();

            //Business logic END
            Log.Information("Application Ended");
            Log.Information("----------------------------------------------");
        }

        private void CheckOrganizationList()
        {
            //Business Logic START
            if (ListOrganization != null)
            {
                if (ListOrganization.Count != 0)
                {
                    foreach (var nomeOrg in ListOrganization)
                    {
                        generateExecuteScript(nomeOrg, _gitLogin, _tempClone);
                    }
                }
                else
                {
                    Log.Error("ListOrganization.txt vuota");
                    logMail.AddLine("ORG_NULL", "PROJ_NULL", "REPO_NULL", "ERR02");
                }
            }
        }

        private static void LoadConfig()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(_pathAppSettings)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
            .Build();

            Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(configuration)
              //.WriteTo.TextWriter(logInString, restrictedToMinimumLevel: LogEventLevel.Information)
              .CreateLogger();

            _gitLogin = configuration.GetSection("GitSettings").Get<GitLogin>();
            _folderSettings = configuration.GetSection("FolderSettings").Get<FolderSetting>();
            _folderSettings.SetProgramDirectory(_programDirectory);
            _emailSettings = configuration.GetSection("EmailSettings").Get<EmailSetting>();

            if (File.Exists(_folderSettings.pathFileOrg))
            {
                ListOrganization = File.ReadAllLines(_folderSettings.pathFileOrg).ToList();
            }
            else
            {
                Log.Warning("File Listorganization.txt non trovato!!");
                Log.Warning("Verificare che il file esista nella cartella /Config o che sia corretto il riferimento in: /Config/appsettings.json");
                logMail.AddLine("ORG_NULL", "PROJ_NULL", "REPO_NULL", "ERR01");
            }
        }

        private void generateExecuteScript(string nomeOrg, GitLogin gitLogin, string tempClone)
        {
            List<GitProjectDTO> listProject = GitRepository.RequestApiAllProjectByOrganization(nomeOrg, gitLogin);

            if (listProject != null)
            {
                foreach (var proj in listProject)
                {
                    List<GitRepoDTO> listRepo = GitRepository.RequestApiAllRepoByProject(nomeOrg,proj.name,gitLogin);

                    if (listRepo!=null)
                    {
                        foreach (var repo in listRepo)
                        {
                            string descRepo = $"{repo.name}    project:{proj.name}     organization:{nomeOrg}";
                            string pathRepo = $"{nomeOrg}/{proj.id}/_git/{repo.id}";
                            try
                            {
                                string workdirPath = $"{tempClone}/{nomeOrg}/{proj.name}";
                                //clone
                                Log.Debug("inizio clonazione repo {0}", descRepo);
                                int exitCode=GitCloneDos(gitLogin, pathRepo,repo.name, workdirPath);

                                if (exitCode==0)
                                {
                                    Log.Information("clonazione repo {0} esito: positivo", descRepo);
                                    logMail.AddLine(nomeOrg, proj.name,repo.name, "OK");
                                }
                                else
                                {
                                    Log.Error("clonazione repo {0} esito: negativo", descRepo);
                                    logMail.AddLine(nomeOrg, proj.name, repo.name, "FAIL");
                                }
                            
                            }
                            catch (Exception ex)
                            {
                                Log.Debug("Attenzione {0} già esistente", descRepo);
                                Log.Error("clonazione repo {0} esito: negativo", descRepo);
                                Log.Error("{0}", ex);
                                logMail.AddLine(nomeOrg, proj.name,repo.name, "ERR04");
                            }

                        }
                    }
                    else
                    {
                        Log.Warning("{0} Lista repo vuota su progetto: {1}", nomeOrg,proj.name);
                        logMail.AddLine(nomeOrg,proj.name,"REPO_NULL","ERR08");
                    }

                }
            }
            else
            {
                Log.Warning("{0} Lista progetti vuota su account: {1}", nomeOrg, gitLogin.gitUser);
                logMail.AddLine(nomeOrg, "PROJ_NULL", "REPO_NULL", "ERR05");
            }
        }

        private int GitCloneDos(GitLogin gitLogin,string pathRepo,string repoName, string pathBackup)
        {
            int exitCode;
            string gitPath = _folderSettings.gitPath;
            repoName = $"\"{repoName}\"";
            string resultScript = $"clone \"https://{gitLogin.gitUser}:{gitLogin.gitToken}@dev.azure.com/{pathRepo}\" ./{repoName} -c core.longpaths=true";

            //File.AppendAllText("ListCMD.txt", $"repo:{repoName} git {resultScript}\n");
           
            Directory.CreateDirectory(pathBackup);

            using (Process process = new Process())
            {
                process.StartInfo.WorkingDirectory = pathBackup;
                process.StartInfo.FileName = _folderSettings.gitPath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                
                process.StartInfo.Arguments = resultScript;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            return exitCode;
        }
    }
}
