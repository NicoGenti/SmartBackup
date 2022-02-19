using System;

namespace smart_backup.DTO
{
    public class FolderSetting
    {
        public string gitPath { get; set; }
        public string pathFileOrg { get; set; }
        public string rootBackup { get; set; }
        public string nameBackup { get; set; }

        public void SetProgramDirectory(string programDirectory)
        {
            pathFileOrg = programDirectory + pathFileOrg;
            rootBackup = programDirectory + rootBackup;
            nameBackup = DateTime.Now.ToString("yyyyMMddhhmmss") + nameBackup;
        }
    }
}
