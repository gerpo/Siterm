namespace Siterm.Settings.Models
{
    public class AppSettings
    {
        public string DatabaseConnectionString { get; set; }
        public string MainPath { get; set; }
        public string FirstResponderExcelFile { get; set; }
        public string FirstResponderInfoFile { get; set; }
        public string SubstancePath { get; set; }
        public string FacilityPath { get; set; }
        public string InstructionFolderName { get; set; }
        public string ServiceReportFolderName { get; set; }
    }
}
