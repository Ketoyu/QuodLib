using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace QuodLib.Database.ADO;

public static class Read {
    public class Resultset<T> {
        public List<T> Contents { get; init; }
        public Dictionary<string, object>? Outputs { get; internal set; }
    }

    public static async Task<List<T>> ReadContentsAsync<T>(string procName, Func<SqlDataReader, T> readLine, IEnumerable<SqlParameter>? parameters = null, CommandType commandType = CommandType.StoredProcedure) {
        Resultset<T> resultset = await ReadAsync(procName, readLine, parameters, commandType);
        return resultset.Contents;
    }
    public static async Task<Resultset<T>> ReadAsync<T>(string procName, Func<SqlDataReader, T> readLine, IEnumerable<SqlParameter>? parameters = null, CommandType commandType = CommandType.StoredProcedure) {
        Resultset<T> resultset = new() {
            Contents = new()
        };
        
        resultset.Outputs = await Execute.ExecuteAsync(procName, async (cmd) => {
            using (var rd = await cmd.ExecuteReaderAsync()) {
                while (await rd.ReadAsync())
                    resultset.Contents.Add(readLine(rd));
            }
        }, parameters, commandType);

        return resultset;
    }

    public static Task<SqlDataReader> OpenAsync(string procName, out SqlConnection cnn, out SqlCommand cmd, IEnumerable<SqlParameter>? parameters = null, CommandType commandType = CommandType.StoredProcedure) {
        cnn = new(Static.ConnectionString);
        cmd = new(procName, cnn);
        cmd.CommandType = commandType;

        if (parameters != null)
            foreach (var prm in parameters)
                cmd.Parameters.Add(prm);

        return cmd.ExecuteReaderAsync();
    }
}
