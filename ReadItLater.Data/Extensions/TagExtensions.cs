using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ReadItLater.Data
{
    public static class TagExtensions
    {
        public static DataTable ToUdt(this Tag model)
        {
            return CreateDataTable(model);
        }

        public static DataTable ToUdt(this IEnumerable<Tag> models)
        {
            return CreateDataTable(models?.ToArray());
        }

        private static DataTable CreateDataTable(params Tag[]? models)
        {
            var dt = new DataTable("TagUdt");
            var propTypes = typeof(Tag).GetProperties();

            dt.Columns.Add(propTypes.GetProperty(nameof(Tag.Id)));
            dt.Columns.Add(propTypes.GetProperty(nameof(Tag.Name)));

            if (models != null)
                models.ToList().ForEach(m => dt.Rows.Add(m.Id, m.Name));

            return dt;
        }
    }
}
