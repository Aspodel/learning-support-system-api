using ExcelDataReader;
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
                if (table.Columns.Contains(property.Name))
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

}
