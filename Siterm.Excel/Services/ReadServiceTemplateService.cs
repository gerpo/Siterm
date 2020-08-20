using System.Collections.ObjectModel;
using System.IO;
using OfficeOpenXml;
using Siterm.Domain.Models;

namespace Siterm.Excel.Services
{
    public static class ReadServiceTemplateService
    {
        static ReadServiceTemplateService()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public static ObservableCollection<ServiceTask> ReadFile(FileInfo fileInfo)
        {
            var collection = new ObservableCollection<ServiceTask>();

            if (!fileInfo.Exists) throw new ExcelFileDoesNotExist();
            using var excel = new ExcelPackage(fileInfo);

            var worksheet = excel.Workbook.Worksheets[0];
            var curRow = 8;
            while (worksheet.Cells[curRow, 2].GetValue<string>() != null)
            {
                var task = new ServiceTask
                {
                    Area = worksheet.Cells[curRow, 1].GetValue<string>(),
                    Description = worksheet.Cells[curRow, 2].GetValue<string>()
                };
                collection.Add(task);

                curRow++;
            }

            return collection;
        }
    }
}