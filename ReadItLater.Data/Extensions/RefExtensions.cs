using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ReadItLater.Data
{
    public static class RefExtensions
    {
        public static DataTable ToUdt(this Ref model)
        {
            var dt = new DataTable("RefUdt");
            var propTypes = typeof(Ref).GetProperties();

            dt.Columns.Add(GetProperty(propTypes, nameof(Ref.Id)));
            dt.Columns.Add(GetProperty(propTypes, nameof(Ref.FolderId)));
            dt.Columns.Add(GetProperty(propTypes, nameof(Ref.Title)));
            dt.Columns.Add(GetProperty(propTypes, nameof(Ref.Url)));
            dt.Columns.Add(GetProperty(propTypes, nameof(Ref.Image)));
            dt.Columns.Add(GetProperty(propTypes, nameof(Ref.Priority)));
            dt.Columns.Add(GetProperty(propTypes, nameof(Ref.Date)));

            dt.Rows.Add(model.Id, model.FolderId, model.Title, model.Url, model.Image, (int)model.Priority, model.Date);

            return dt;
        }

        private static DataColumn GetProperty(PropertyInfo[] properties, string name)
        {
            var type = properties.Single(p => p.Name.Equals(name)).PropertyType;

            if (type.IsEnum)
                type = typeof(int);

            return new DataColumn(name, Nullable.GetUnderlyingType(type) ?? type);
        }
    }
}
