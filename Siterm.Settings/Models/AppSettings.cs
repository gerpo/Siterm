namespace Siterm.Settings.Models
{
    public class AppSettings
    {
        public string DatabaseConnectionString { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpServerPort { get; set; }
        public string MailUserName { get; set; }
        public string MailPassword { get; set; }
        public string MailSenderAddress { get; set; }
        public string CCEmailAddresses { get; set; }
        public string MainPath { get; set; }
        public string HomeInfoFile { get; set; }
        public string FirstResponderExcelFile { get; set; }
        public string FirstResponderInfoFile { get; set; }
        public string SubstancePath { get; set; }
        public string FacilityPath { get; set; }
        public string InstructionFolderName { get; set; }
        public string InstructionArchiveFolderName { get; set; }
        public string ServiceReportFolderName { get; set; }
        public string InstructionTemplateFile { get; set; }
    }
}
