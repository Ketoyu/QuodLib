using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Database.ADO {
    public static class Parameters {
        private static SqlParameter Make(string name, SqlDbType type, object value, ParameterDirection direction)
            => new(name, type) {
                Direction = direction,
                Value = value
            };

        public static SqlParameter Make(string name, bool value, ParameterDirection direction = ParameterDirection.Input)
            => Make(name, SqlDbType.Bit, value, direction);

        public static SqlParameter Make(string name, int value, ParameterDirection direction = ParameterDirection.Input)
            => Make(name, SqlDbType.Int, value, direction);

        public static SqlParameter Make(string name, short value, ParameterDirection direction = ParameterDirection.Input)
            => Make(name, SqlDbType.SmallInt, value, direction);
        public static SqlParameter Make(string name, sbyte value, ParameterDirection direction = ParameterDirection.Input)
            => Make(name, SqlDbType.TinyInt, value, direction);

        public static SqlParameter Make(string name, long value, ParameterDirection direction = ParameterDirection.Input)
            => Make(name, SqlDbType.BigInt, value, direction);

        public static SqlParameter Make(string name, string value, short size, ParameterDirection direction = ParameterDirection.Input)
            => new(name, SqlDbType.VarChar, size) {
                Direction = direction,
                Value = value
            };

        public static SqlParameter Make(string name, decimal value, byte scale, byte precision, ParameterDirection direction = ParameterDirection.Input)
            => new(name, SqlDbType.VarChar) {
                Scale = scale,
                Precision = precision,
                Direction = direction,
                Value = value
            };

        public static Dictionary<string, object> AsOutputs(this SqlParameterCollection parameters)
            => parameters.Cast<SqlParameter>()
                .Where(p => p.Direction is ParameterDirection.Output or ParameterDirection.InputOutput or ParameterDirection.ReturnValue)
                .ToDictionary(p => p.ParameterName, p => p.Value);
            
    }
}
