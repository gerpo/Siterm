using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Siterm.Settings.Models;
using Siterm.Settings.Services;

namespace Siterm.Excel.Services
{
    public class ReadPSentencesService
    {
        private readonly string _pSentenceFile;

        public ReadPSentencesService(SettingsProvider settingsProvider)
        {
            _pSentenceFile = settingsProvider.GetSetting(SettingName.PFile).Value;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public Dictionary<string, string> GetPSentences()
        {
            var collection = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(_pSentenceFile)) return collection;

            var fileInfo = new FileInfo(_pSentenceFile);

            if (!fileInfo.Exists) return collection;
            using var excel = new ExcelPackage(fileInfo);

            var worksheet = excel.Workbook.Worksheets[0];

            return worksheet.Cells["a:a"]
                .ToDictionary(c => worksheet.Cells[c.Start.Row, 1].Value.ToString(),
                    c => worksheet.Cells[c.Start.Row, 2].Value.ToString());
        }
    }
}