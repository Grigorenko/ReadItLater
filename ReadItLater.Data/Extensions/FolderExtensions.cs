using System.Data;

namespace ReadItLater.Data
{
    public static class FolderExtensions
    {
        public static DataTable ToUdt(this Folder model)
        {
            var dt = new DataTable("FolderUdt");
            var propTypes = typeof(Folder).GetProperties();

            dt.Columns.Add(propTypes.GetProperty(nameof(Folder.Id)));
            dt.Columns.Add(propTypes.GetProperty(nameof(Folder.ParentId)));
            dt.Columns.Add(propTypes.GetProperty(nameof(Folder.Name)));
            dt.Columns.Add(propTypes.GetProperty(nameof(Folder.Order)));

            dt.Rows.Add(model.Id, model.ParentId, model.Name, model.Order);

            return dt;
        }
    }
}
