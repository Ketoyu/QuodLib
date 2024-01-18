using QuodLib.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Database
{
    public interface IDataAccessAsync
    {
        Task<IEnumerable<T>> LoadAll<T>();
        Task<T> LoadByID<T>(int id) where T : IRecord;
        Task Save<T>(T item) where T : IRecord;
    }
}
