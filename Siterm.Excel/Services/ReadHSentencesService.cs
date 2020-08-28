using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Siterm.Settings.Models;
using Siterm.Settings.Services;

namespace Siterm.Excel.Services
{
    public class ReadHSentencesService
    {
        private readonly string _hSentenceFile;

        public ReadHSentencesService(SettingsProvider settingsProvider)
        {
            _hSentenceFile = settingsProvider.GetSetting(SettingName.HFile).Value;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public Dictionary<string, string> GetHSentences()
        {
            var collection = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(_hSentenceFile)) return collection;

            var fileInfo = new FileInfo(_hSentenceFile);

            if (!fileInfo.Exists) throw new ExcelFileDoesNotExist();
            using var excel = new ExcelPackage(fileInfo);

            var worksheet = excel.Workbook.Worksheets[0];

            return worksheet.Cells["a:a"]
                .ToDictionary(c => worksheet.Cells[c.Start.Row, 1].Value.ToString(),
                    c => worksheet.Cells[c.Start.Row, 2].Value.ToString());
        }

        public Dictionary<string, string> GetHIcon()
        {
            var collection = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(_hSentenceFile)) return collection;

            var fileInfo = new FileInfo(_hSentenceFile);

            if (!fileInfo.Exists) throw new ExcelFileDoesNotExist();
            using var excel = new ExcelPackage(fileInfo);

            var worksheet = excel.Workbook.Worksheets[0];

            return worksheet.Cells["a:a"]
                .ToDictionary(c => worksheet.Cells[c.Start.Row, 1].Value.ToString(),
                    c => worksheet.Cells[c.Start.Row, 3].Value?.ToString());
        }
    }
}