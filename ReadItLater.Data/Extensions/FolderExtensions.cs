using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ReadItLater.Data
{
    public static class FolderExtensions
    {
        public static DataTable ToUdt(this Folder model)
        {
            var dt = new DataTable("FolderUdt");
            var propTypes = typeof(Folder).GetProperties();

            dt.Columns.Add(GetProperty(propTypes, nameof(Folder.Id)));
            dt.Columns.Add(GetProperty(propTypes, nameof(Folder.ParentId)));
            dt.Columns.Add(GetProperty(propTypes, nameof(Folder.Name)));
            dt.Columns.Add(GetProperty(propTypes, nameof(Folder.Order)));

            dt.Rows.Add(model.Id, model.ParentId, model.Name, model.Order);

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
