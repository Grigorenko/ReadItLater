using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ReadItLater.Data
{
    public static class TagExtensions
    {
        public static DataTable ToUdt(this Tag model)
        {
            //var dt = new DataTable("TagUdt");
            //var propTypes = model.GetType().GetProperties();

            //dt.Columns.Add(GetProperty(propTypes, nameof(Tag.Id)));
            //dt.Columns.Add(GetProperty(propTypes, nameof(Tag.Name)));

            //dt.Rows.Add(model.Id, model.Name);

            return CreateDataTable(model);
        }

        public static DataTable ToUdt(this IEnumerable<Tag> models)
        {
            //var dt = new DataTable("TagUdt");

            //dt.Columns.Add(nameof(Tag.Id), model.First().Id.GetType());
            //dt.Columns.Add(nameof(Tag.Name), model.First().Name.GetType());

            //model.ToList().ForEach(m => dt.Rows.Add(m.Id, m.Name));

            return CreateDataTable(models?.ToArray());
        }

        private static DataTable CreateDataTable(params Tag[] models)
        {
            var dt = new DataTable("TagUdt");
            var propTypes = typeof(Tag).GetProperties();

            dt.Columns.Add(GetProperty(propTypes, nameof(Tag.Id)));
            dt.Columns.Add(GetProperty(propTypes, nameof(Tag.Name)));

            if (models != null)
                models.ToList().ForEach(m => dt.Rows.Add(m.Id, m.Name));

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
