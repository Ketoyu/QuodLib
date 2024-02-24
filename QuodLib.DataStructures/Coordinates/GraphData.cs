using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Objects
{
    public class GraphData
    {

        /*TYPE CONVENTIONS
            * Data: ushort
            * DataField indexes: byte
            * Averages/Sums: float
            * */

        /*OBJECTIVES
            * ~ Bar graph: primary data
            * ~ ( transluscent bars || opaque lines ): average
            * ~ Average: running average, past-x average.
            * 
            * */

        #region Fields
        /// <summary>
        /// Data within the Object.
        /// </summary>
        public float[] Data;
        /// <summary>
        /// Moving average of Data
        /// </summary>
        public float[] MovingAverage; //(just take an average of all data each time there's a Push)
        /// <summary>
        /// The greatest index which is currently being occupied by the datafield Data.
        /// </summary>
        public ushort Length;
        public object Scale = new System.Drawing.Size(1, 1); //Visual scale; for outside use. {x,y}
		/// <summary>
		/// The maximum number of data points contained.
		/// </summary>
        public byte Size = 1;
        #endregion //Fields

        #region Subroutines
        /// <summary>
        /// Inserts a new data value in the next available index. If Data is full, all values for Data are shifted toward [0].
        /// </summary>
        /// <param name="value"></param>
        public void Push(float value)
        {
            //Shift data if the specified dataField is full.
            //Increment Length if the specified dataField is not full.
				
			//Do the same for MovingAverage.
        }
		private void Push(float value, float[] dataset)
		{
			//if (
			for (int i = 1; i < Length; i++) {

			}
		}
            #region Clear
        /// <summary>
        /// Clear all Data and MovingAverage.
        /// </summary>
        public void Clear()
        {
            for (byte i = 0; i < Data.Length; i++)
                {
                    Data[i] = 0;
                    MovingAverage[i] = 0;
                }
                Length = 0;
        }
            #endregion //Clear
        #endregion //Subroutines

        #region Functions
            #region Average
        /// <summary>
        /// Averages the data between indexes period[0] and period[1] (inclusive).
        /// </summary>
        /// <param name="start">Data</param>
        /// <param name="end">Index span</param>
        /// <returns></returns>
        private float Average(ushort start, ushort end)
        {
            return (float)Sum(start, end) / (float)(end - start + 1);
        }
        /// <summary>
        /// Averages all data.
        /// </summary>
        /// <returns></returns>
        private float Average()
        {
            return (float)Sum() / (float)Data.Length;
        }
            #endregion //Average
            #region Sum
        /// <summary>
        /// Adds up the data between indexes bounds[0] and bounds[1] (inclusive).
        /// </summary>
        /// <param name="start">Data</param>
        /// <param name="end">Index span</param>
        /// <returns></returns>
        private float Sum(ushort start, ushort end)
        {
            float mysum = 0f;
            for (ushort i = start; i < end; i++)
            {
                mysum += Data[i];
            }
            return mysum;
        }
        /// <summary>
        /// Adds up all data.
        /// </summary>
        /// <returns></returns>
        private float Sum()
        {
            return Sum(0, (ushort)(Data.Length - 1));
        }
            #endregion //Sum
        #endregion //Functions
    }
}
