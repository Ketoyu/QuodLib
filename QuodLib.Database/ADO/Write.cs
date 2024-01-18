using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Database.ADO;
public static class Write {
    public static Task<Dictionary<string, object>?> ExecuteAsync(string procName, IEnumerable<SqlParameter>? sqlParams = null)
        => Execute.ExecuteAsync(procName, 
            cmd => cmd.ExecuteNonQueryAsync(),
            sqlParams);
}
