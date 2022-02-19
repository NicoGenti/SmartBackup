using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;
using smart_backup.DTO;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace smart_backup.Elaborate
{
    public static class Elaborate
    {
        public static void DeleteDirectory(string target_dir)
        {
            if (Directory.Exists(target_dir))
            {
                
                string[] files = Directory.GetFiles(target_dir);
                string[] dirs = Directory.GetDirectories(target_dir);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(target_dir, false);
                
            }
        }

        public static string ZipTheFolder(string FolderToZip)
        {
            string ZipPath = FolderToZip + ".zip";
            if (Directory.Exists(FolderToZip) && IsDirectoryEmpty(FolderToZip) == false)
            {
                try
                {
                    Log.Information("inizio creazione {0}",ZipPath);
                    ZipFile.CreateFromDirectory(FolderToZip, ZipPath);
                    Log.Information($"{ZipPath} creato correttamente");
                }
                catch (Exception ex)
                {
                    Log.Error("errore durante creazione zip:", ex);
                    return "ERR06";
                }
            }
            else
            {
                Log.Warning("Cartella temporanea non trovata oppure vuota");
                return "ERR07";
            }
            return "OK";
        }

        public static void CleanFolderTemp(string folder) 
        {

            if (Directory.Exists(folder))
            {
                Log.Debug("trovata cartella temporanea ed inizio eliminazione");
                DeleteDirectory(folder);
                Log.Debug("eliminata cartella temporanea");
            }

        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }


        public static async Task SendMail(EmailSetting mail,string pathLogMail)
        {
            string log = File.ReadAllText(pathLogMail);
            Log.Information("inizio invio log alla mail {0}", mail.ToEmail);
            
            bool statusMail = false;
            var apiKey = mail.ApiKeySendgrid;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(mail.FromEmail, "SmartPeg Backup");
            var subject = "Log SmartBackup";
            var to = new EmailAddress(mail.ToEmail, "Guido");
            var plainTextContent =log;
            var htmlContent = "";
           
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            using (var fileStream = File.OpenRead(pathLogMail))
            {
                await msg.AddAttachmentAsync("log.txt", fileStream);
                var response = await client.SendEmailAsync(msg);
                statusMail = response.IsSuccessStatusCode;
            }
            

            if (statusMail)
            {
                Log.Information("mail di partenza {0} corretta", mail.FromEmail);
                Log.Information("invio mail riuscito");
                //File.Delete(pathLogMail);
            }
            else
            {
                Log.Error("invio non riuscito, mail di partenza {0} non corretta, controllare /config/appsetting.json", mail.FromEmail);
            }
            
            
        }

    }

}
