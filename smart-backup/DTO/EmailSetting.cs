namespace smart_backup.DTO
{
    public class EmailSetting
    {
        public string EmailSubject { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string FromName { get; set; }
        public string ApiKeySendgrid { get; set; }
    }
}
