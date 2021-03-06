using System;
using System.Data;
using System.Linq;

namespace ReadItLater.Data
{
    public static class RefExtensions
    {
        public static DataTable ToUdt(this Ref model)
        {
            var dt = new DataTable("RefUdt");
            var propTypes = typeof(Ref).GetProperties();

            dt.Columns.Add(propTypes.GetProperty(nameof(Ref.Id)));
            dt.Columns.Add(propTypes.GetProperty(nameof(Ref.FolderId)));
            dt.Columns.Add(propTypes.GetProperty(nameof(Ref.Title)));
            dt.Columns.Add(propTypes.GetProperty(nameof(Ref.Url)));
            dt.Columns.Add(propTypes.GetProperty(nameof(Ref.Image)));
            dt.Columns.Add(propTypes.GetProperty(nameof(Ref.Priority)));
            dt.Columns.Add(propTypes.GetProperty(nameof(Ref.Date)));
            dt.Columns.Add(propTypes.GetProperty(nameof(Ref.Note)));

            dt.Rows.Add(model.Id, model.FolderId, model.Title, model.Url, model.Image, (int)model.Priority, model.Date, model.Note);

            return dt;
        }

        public static DataTable ToSortUdt(this string? sort)
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

            (string orderBy, string direction, int position)[]? ParseSortParam(string? sort)
            {
                int seqNum = 0;
                var sets = sort?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                return sets
                    ?.Select(s => s.StartsWith("-")
                        ? (s.TrimStart('-'), "DESCENDING", seqNum++)
                        : (s.TrimStart('+'), "ASCENDING", seqNum++))
                    ?.ToArray();
            }
        }
    }
}
