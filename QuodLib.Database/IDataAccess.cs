using QuodLib.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Database
{
    public interface IDataAccess
    {
        List<T> LoadAll<T>();
        T LoadByID<T>(int id) where T : IRecord;
        void Save<T>(T item) where T : IRecord;
    }
}
