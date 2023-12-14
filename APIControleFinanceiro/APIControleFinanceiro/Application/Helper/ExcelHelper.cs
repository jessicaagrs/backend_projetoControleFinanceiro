using ExcelDataReader;
using System.Data;
using System.Reflection;
using System.Text;

namespace APIControleFinanceiro.Application.Helper
{
    public class ExcelHelper<T> where T : class, new()
    {
        private IExcelDataReader _reader;

        public ExcelHelper(IFormFile file)
        {
            var stream = file.OpenReadStream();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (file.FileName.EndsWith(".csv"))
            {
                _reader = ExcelReaderFactory.CreateCsvReader(stream);
            }
            else
                throw new Exception("O arquivo não está no formato correto.");
        }

        public List<T> GetValues()
        {
            var list = new List<T>();
            var table = ConvertToDataTable();

            var properties = typeof(T).GetProperties().ToList();
            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                list.Add(item);
            }

            return list;
        }


        private DataTable ConvertToDataTable()
        {
            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            };

            var dataset = _reader.AsDataSet(conf);
            return dataset.Tables[0];
        }

        private T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            var item = new T();

            foreach (var property in properties)
            {
                if (row.Table.Columns.Contains(property.Name))
                {
                    object value = row[property.Name];

                    if (value != DBNull.Value)
                    {
                        if (property.PropertyType == typeof(decimal))
                        {
                            property.SetValue(item, Convert.ToDecimal(value));
                        }
                        else if (property.PropertyType == typeof(DateTime))
                        {
                            property.SetValue(item, Convert.ToDateTime(value));
                        }
                        else
                        {
                            property.SetValue(item, value);
                        }
                    }
                    else
                    {
                        if (!property.PropertyType.IsValueType || Nullable.GetUnderlyingType(property.PropertyType) != null)
                        {
                            property.SetValue(item, null);
                        }
                        else if (property.PropertyType == typeof(DateTime))
                        {
                            property.SetValue(item, DateTime.MinValue);
                        }
                        else
                        {
                            property.SetValue(item, Convert.ChangeType(0.0, property.PropertyType));
                        }
                    }
                }
            }

            return item;
        }
    }
}
