using System;
using System.Collections.Generic;

namespace smart_backup.DTO
{
    public class LogMailModel
    {
        public DateTime dataBackup = DateTime.Now;
        public List<LineLogModel> lines = new List<LineLogModel>();

        Dictionary<string, string> messages = new Dictionary<string, string>
        {
            {"OK","Completato"},
            {"ERR01","File Listorganization.txt non trovato!! Verificare file che il file esista e che il riferimento in /Config/appsettings.json sia corretto"},
            {"ERR02","ListOrganization.txt vuota"},
            {"ERR04","Attenzione cartella repo già esistente"},
            {"ERR05","Errore richiesta Api: organizzazione non trovata o vuota, verificare organizzazione o token"},
            {"ERR06","Errore durante creazione zip"},
            {"ERR07","Cartella temporanea non trovata oppure vuota"},
            {"ERR08","Errore richiesta Api: lista repo vuota"},
            {"FAIL","Comando GIT Fallito"},
        };

        public void AddLine(string org,string proj,string repo,string esito)
        {
            lines.Add(new LineLogModel(org,proj,repo, esito, messages[esito]));
        }

    }
}
