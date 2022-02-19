using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smart_backup.DTO
{
    public class GitRepoDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string remoteUrl { get; set; }
    }
}
