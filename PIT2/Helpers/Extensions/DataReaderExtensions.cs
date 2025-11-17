using System.Data.Common;
using System.Reflection;

namespace PIT2.Helpers.Extensions
{
    public static class DataReaderExtensions
    {
        public static List<T> MapToList<T>(this DbDataReader reader) where T : new()
        {
            var list = new List<T>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var columnNames = Enumerable.Range(0, reader.FieldCount)
                .Select(reader.GetName)
                .ToList();

            while (reader.Read())
            {
                var obj = new T();

                foreach (var prop in props)
                {
                    if (columnNames.Contains(prop.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        var val = reader[prop.Name];

                        if (val == DBNull.Value)
                            continue;

                        prop.SetValue(obj, Convert.ChangeType(val, prop.PropertyType));
                    }
                }

                list.Add(obj);
            }

            return list;
        }
    }
}