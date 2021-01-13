using System.Collections.Generic;
using System.Text;

namespace ReadItLater.Data.EF.Options
{
    public interface IDbConnection
    {
        string ConnectionString { get; }
    }
}
