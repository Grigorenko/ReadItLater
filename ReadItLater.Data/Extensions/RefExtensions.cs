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
            dt.Columns.Add(GetProperty(propTypes, nameof(Ref.Note)));

            dt.Rows.Add(model.Id, model.FolderId, model.Title, model.Url, model.Image, (int)model.Priority, model.Date, model.Note);

            return dt;
        }

        private static DataColumn GetProperty(PropertyInfo[] properties, string name)
        {
            var type = properties.Single(p => p.Name.Equals(name)).PropertyType;

            if (type.IsEnum)
                type = typeof(int);

            return new DataColumn(name, Nullable.GetUnderlyingType(type) ?? type);
        }

        public static DataTable ToSortUdt(this string sort)
        {
            var dt = new DataTable("SortUdt");
            var propTypes = typeof(Ref).GetProperties();

            dt.Columns.Add("OrderBy", typeof(string));
            dt.Columns.Add("Direction", typeof(string));
            dt.Columns.Add("Position", typeof(int));

            ParseSortParam(sort)
                ?.ToList()
                ?.ForEach(x => dt.Rows.Add(x.orderBy, x.direction, x.position));

            return dt;
        }

        private static (string orderBy, string direction, int position)[] ParseSortParam(string sort)
        {
            int seqNum = 0;
            var sets = sort
                ?.Split(',')
                ?.Where(i => !string.IsNullOrEmpty(i));

            return sets
                ?.Select(s => s.StartsWith("-") 
                    ? (s.TrimStart('-'), "DESCENDING", seqNum++) 
                    : (s.TrimStart('+'), "ASCENDING", seqNum++))
                ?.ToArray();
        }
    }
}
