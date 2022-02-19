using Newtonsoft.Json.Linq;
using Serilog;
using smart_backup.DTO;
using smart_backup.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace smart_backup.Service
{
    public static class GitRepository
    {
        public static List<GitProjectDTO> RequestApiAllProjectByOrganization(string nomeOrg,GitLogin gitLogin)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", gitLogin.gitToken))));

                    using (HttpResponseMessage response = client.GetAsync("https://dev.azure.com/" + nomeOrg + "/_apis/projects").GetAwaiter().GetResult())
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        var jobject = JObject.Parse(responseBody);
                        List<GitProjectDTO> listProjects = jobject["value"].ToObject<List<GitProjectDTO>>();
                        return listProjects;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("errore durante la richiesta Api di {0}, verificare nome organizzazione o validità token",nomeOrg);
                Log.Error("{0}", ex);
                return null;
            }
        }

        public static List<GitRepoDTO> RequestApiAllRepoByProject(string nomeOrg,string project, GitLogin gitLogin)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", gitLogin.gitToken))));

                    using (HttpResponseMessage response = client.GetAsync($"https://dev.azure.com/{nomeOrg}/{project}/_apis/git/repositories").GetAwaiter().GetResult())
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        var jobject = JObject.Parse(responseBody);
                        List<GitRepoDTO> listProjects = jobject["value"].ToObject<List<GitRepoDTO>>();
                        return listProjects;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("errore durante la richiesta Api di {0}, verificare nome organizzazione, nome progetto o validità token", nomeOrg);
                Log.Error("{0}", ex);
                return null;
            }
        }


    }
}
