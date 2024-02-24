using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.WinForms.Objects
{
    /*public*/ class CTable : CObject, ICObject
    {
        private Dictionary<string, ushort> Columns;
        private List<List<object>> data;
        public void OrderBy(string column, bool asc)
        {
            /*Columns[column].Sort();
            if (!asc) Columns[column].Reverse();*/
        }
        public void Redraw()
        {

        }
    }
}
