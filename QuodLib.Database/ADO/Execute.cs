using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Database.ADO;

internal static class Execute {
    internal static async Task<Dictionary<string, object>?> ExecuteAsync(string command, Func<SqlCommand, Task> doExecuteAsync, IEnumerable<SqlParameter>? sqlParams = null, CommandType commandType = CommandType.StoredProcedure) {
        using (SqlConnection cnn = new(Static.ConnectionString))
        using (SqlCommand cmd = new(command, cnn)) {
            cmd.CommandType = commandType;

            if (sqlParams != null)
                foreach(var param in sqlParams)
                    cmd.Parameters.Add(param);

            await doExecuteAsync(cmd);

            var outPrms = cmd.Parameters.AsOutputs();

            if (outPrms.Any())
                return outPrms;

            return null;
        }
    }
}
