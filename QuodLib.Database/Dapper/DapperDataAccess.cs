using Dapper;
using QuodLib.DataModel;
using QuodLib.DataModel.CustomAttributes;
using QuodLib.Linq;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace QuodLib.Database.Dapper {
    public abstract class DapperDataAccess<TConnection> : IDataAccessAsync, IDisposable
         where TConnection : DbConnection {

        private bool disposedValue;
        protected TConnection? connection;

        protected DapperDataAccess() {
            connection = CreateConnection();
        }


        public Task<IEnumerable<T>> LoadAll<T>()
            => connection!.QueryAsync<T>($"SELECT * FROM {typeof(T).Name}");

        public Task<T> LoadByID<T>(int id) where T : IRecord
            => connection!.QuerySingleAsync<T>($"SELECT * FROM {typeof(T).Name} WHERE {nameof(IRecord.ID)} = @{nameof(IRecord.ID)}", new { ID = id });

        public async Task Save<T>(T item) where T : IRecord {
            FieldInfo[] fields = typeof(T).GetFields();

            IEnumerable<FieldInfo> writeFields = fields
                .Except<IRecord>()
                .ExceptAttribute<DbIgnoreAttribute>();

            DynamicParameters p = new(
                writeFields
                    .Select(f => new KeyValuePair<string, object?>(f.Name, f.GetValue(item)))
            );
            p.Add($"@{nameof(IRecord.ID)}", item.ID, direction: ParameterDirection.InputOutput);

            if (item.ID == null) {
                await connection!.ExecuteAsync($@"
INSERT
{typeof(T).Name} ({string.Join(", ", writeFields.Select(f => f.Name))})
VALUES
({string.Join(", ", writeFields.Select(f => writeFields.Select(f => $"@{f.Name}")))});

SET @{nameof(IRecord.ID)} = @SCOPE_IDENTITY;"
                    , p);

                item.ID = p.Get<int?>($"@{nameof(IRecord.ID)}");
            } else {
                await connection!.ExecuteAsync($@"
UPDATE {typeof(T).Name} SET
{string.Join(",\r\n    ", writeFields.Select(f => $"{f.Name} = @{f.Name}"))}
WHERE
{nameof(IRecord.ID)} = @{nameof(IRecord.ID)};"
                    , p);
            }

        }

        protected abstract string LoadConnectionString(string id);
        protected abstract TConnection CreateConnection();
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    connection?.Close();
                    connection?.Dispose();
                    connection = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
