using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Data;

namespace LearningSupportSystemAPI;

public static class ExcelHandler
{
    private static void ValidateExcelFormat(this string filename)
    {
        if (!Path.GetExtension(filename).Equals(".xlsx"))
            throw new Exception("File should be in '.xlsx' format");
    }

    public static async Task<DataTable> GetExcelDataTable(this IFormFile file, CancellationToken cancellationToken = default)
    {
        var filename = file.FileName;
        filename.ValidateExcelFormat();

        var tempPath = Path.GetTempFileName();

        using (FileStream stream = new FileStream(tempPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.RandomAccess | FileOptions.DeleteOnClose))
        {
            await file.CopyToAsync(stream, cancellationToken);

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
            {
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };

                var dataSet = reader.AsDataSet(conf);

                var dataTable = dataSet.Tables[0];

                return dataTable;
            }
        }
    }
    public static List<T> GetEntitiesFromDataTable<T>(this DataTable table) where T : class, new()
    {
        var list = new List<T>();
        var properties = typeof(T).GetProperties();

        foreach (DataRow row in table.Rows)
        {
            var entity = new T();

            foreach (var property in properties)
            {
                if (property.Name == "Grades" && property.PropertyType == typeof(Dictionary<string, int?>))
                {
                    var grades = new Dictionary<string, int?>();
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName != "Id" && column.ColumnName != "Student")
                        {
                            var columnName = column.ColumnName;
                            var value = row[columnName];
                            if (value != DBNull.Value)
                            {
                                if (value is double doubleValue)
                                {
                                    var intValue = (int)doubleValue;
                                    grades.Add(columnName, intValue);
                                }
                                else if (value is int intValue)
                                {
                                    grades.Add(columnName, intValue);
                                }
                            }
                            else
                            {
                                grades.Add(columnName, null);
                            }
                        }
                    }
                    property.SetValue(entity, grades);
                }
                else if (table.Columns.Contains(property.Name))
                {
                    var value = row[property.Name];
                    if (value != DBNull.Value)
                    {

                        var targetType = property.PropertyType;

                        if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            targetType = Nullable.GetUnderlyingType(targetType);
                        }

                        if (targetType == typeof(DayOfWeek) && value is double numericValue)
                        {
                            var dayOfWeekValue = (int)numericValue;
                            var convertedValue = (DayOfWeek)(dayOfWeekValue % 7); // Ensure the value is within the valid range of DayOfWeek enumeration
                            property.SetValue(entity, convertedValue);
                        }
                        else
                        {
                            var convertedValue = Convert.ChangeType(value, targetType);
                            property.SetValue(entity, convertedValue);
                        }
                    }
                }
            }

            list.Add(entity);
        }

        return list;
    }

    private static MemoryStream ExportToMemoryStream<T>(this List<T> dataList)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the LicenseContext

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // Add the header row
            // var properties = typeof(T).GetProperties();
            // for (var columnIndex = 0; columnIndex < properties.Length; columnIndex++)
            // {
            //     var property = properties[columnIndex];
            //     var columnName = property.Name;
            //     worksheet.Cells[1, columnIndex + 1].Value = columnName;
            // }

            // Load the data starting from the second row
            worksheet.Cells.LoadFromCollection(dataList, true);
            worksheet.Cells.AutoFitColumns();

            var stream = new MemoryStream(package.GetAsByteArray());
            return stream;
        }
    }

    private static MemoryStream ExportToMemoryStreamForGrade(List<GradeRowDTO> dataList)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // Add the header row
            var columnNames = dataList.SelectMany(row => row.Grades.Keys).Distinct().ToList();
            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = "Student";
            for (var columnIndex = 0; columnIndex < columnNames.Count; columnIndex++)
            {
                var columnName = columnNames[columnIndex];
                worksheet.Cells[1, columnIndex + 3].Value = columnName;
            }

            // Load the data starting from the second row
            for (var rowIndex = 0; rowIndex < dataList.Count; rowIndex++)
            {
                var rowData = dataList[rowIndex];
                var grades = rowData.Grades;

                worksheet.Cells[rowIndex + 2, 1].Value = rowData.Id;
                worksheet.Cells[rowIndex + 2, 2].Value = rowData.Student;

                for (var columnIndex = 0; columnIndex < columnNames.Count; columnIndex++)
                {
                    var columnName = columnNames[columnIndex];
                    if (grades.TryGetValue(columnName, out var gradeValue))
                    {
                        worksheet.Cells[rowIndex + 2, columnIndex + 3].Value = gradeValue;
                    }
                }
            }

            worksheet.Cells.AutoFitColumns();

            var stream = new MemoryStream(package.GetAsByteArray());
            return stream;
        }
    }

    public static FileContentResult ExportToFile<T>(this List<T> dataList, string fileName = "export")
    {
        var stream = typeof(T) == typeof(GradeRowDTO)
       ? ExportToMemoryStreamForGrade(dataList.Cast<GradeRowDTO>().ToList())
       : ExportToMemoryStream(dataList);

        var content = stream.ToArray();
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var fileDownloadName = $"{fileName}.xlsx";

        return new FileContentResult(content, contentType)
        {
            FileDownloadName = fileDownloadName
        };
    }
}
