using System;

namespace smart_backup.DTO
{
    public class LineLogModel
    {
        public DateTime data { get; set; }
        public string nomeOrg { get; set; }
        public string nomeProject { get; set; }
        public string nomeRepo { get; set; }
        public string esito { get; set; }
        public string messaggio { get; set; }

        public LineLogModel(string nomeOrg,string nomeProject, string nomeRepo, string esito, string messaggio)
        {
            this.data = DateTime.Now;
            this.nomeOrg = nomeOrg;
            this.nomeProject = nomeProject;
            this.nomeRepo = nomeRepo;
            this.esito = esito;
            this.messaggio = messaggio;
        }
    }
}
