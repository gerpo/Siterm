using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using OfficeOpenXml;
using Siterm.Domain.Models;

namespace Siterm.Excel.Services
{
    public static class ReadFirstAidExcelService
    {
        static ReadFirstAidExcelService()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public static ObservableCollection<FirstResponder> ReadFile(string path)
        {
            var collection = new ObservableCollection<FirstResponder>();
            if (string.IsNullOrEmpty(path)) return collection;

            var fileInfo = new FileInfo(path);

            if (!fileInfo.Exists) throw new ExcelFileDoesNotExist();
            using var excel = new ExcelPackage(fileInfo);

            var worksheet = excel.Workbook.Worksheets[0];
            var row = 2;

            while (true)
            {
                var facility = worksheet.Cells[row, 1].Text;
                var name = worksheet.Cells[row, 2].Text;
                var phone = worksheet.Cells[row, 3].Text;
                var lastTraining = worksheet.Cells[row, 4].Text;
                var nextTraining = worksheet.Cells[row, 5].Text;

                collection.Add(new FirstResponder(name, facility, phone, lastTraining, nextTraining));
                
                row++;
                if (string.IsNullOrEmpty(worksheet.Cells[row, 1].Text)) break;
            }

            return collection;
        }

    }

    public class ExcelFileDoesNotExist : Exception
    {
    }
}
