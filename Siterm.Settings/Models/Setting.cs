﻿using Siterm.Support.Misc;

namespace Siterm.Settings.Models
{
    public class Setting : CanNotifyPropertyChanged
    {
        private SettingName _name;
        private SettingType _type;
        private string _value;

        public Setting(SettingName settingName, string settingsValue, SettingType type)
        {
            Name = settingName;
            Value = settingsValue;
            Type = type;
        }

        public SettingName Name
        {
            get => _name;
            private set => SetField(ref _name, value);
        }

        public string Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }

        public SettingType Type
        {
            get => _type;
            set => SetField(ref _type, value);
        }
    }

    public enum SettingType
    {
        File,
        Folder,
        MultiFolder,
        FolderFilter,
        String,
        StringOptional,
        Int,
        Password,
    }

    public enum SettingName
    {
        DatabaseConnectionString,
        SmtpServer,
        SmtpServerPort,
        MailUserName,
        MailPassword,
        MailSenderAddress,
        CCEmailAddresses,
        MainPath,
        FirstResponderExcelFile,
        FirstResponderInfoFile,
        SubstancePath,
        FacilityPath,
        PFile,
        HFile,
        InstructionFolderName,
        InstructionArchiveFolderName,
        ServiceReportFolderName,
        InstructionTemplateFile,
        HomeInfoFile
    }
}