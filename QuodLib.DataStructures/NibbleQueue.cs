using QuodLib.Bitwise;
using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using Out = System.Diagnostics.Debug;

namespace QuodLib.DataStructures
{
    class NibbleQueue
    {
        private Queue<byte> Data = new Queue<byte>();
        public bool IsEmpty
        {
            get {
                return (Count() == 0);
            }
        }
        /// <summary>
        /// (first last) nibbles.
        /// </summary>
        private byte PartialIn, PartialOut;
        /// <summary>
        /// False: empty (0000 0000) | True: half-full (In[xxxx 0000] || Out[0000 xxxx])
        /// </summary>
        private bool MidNibble_In, MidNibble_Out;

        //Constructor() remians default.

        #region Methods
            #region Quantify
        public uint Count()
        {
            return (uint)((Data.Size * 2) + (PartialIn > 0 ? 1 : 0) + (PartialOut > 0 ? 1 : 0));
        }
            #endregion //Quantify
            #region Peek
        /// <summary>
        /// Peeks one nibble (four bits) of data.
        /// </summary>
        /// <returns></returns>
        public byte Peek()
        {
            if (MidNibble_Out) //(0000 xxxx) partial.
            {
                if (PartialOut > 15) throw new Exception("Unexpected data: PartialOut > 15 while MiddNibble_Out is true.");
                return (byte)(PartialOut % 16);
            } else {
                if (PartialOut > 0) //(0000 xxxx) partial.
                {
                    throw new Exception("Unexpected data: PartialOut > 0 while MiddNibble_Out is false.");
                } else { //(0000 0000): Peek new byte.
                    byte peek = Data.Peek();
                    return (byte)(peek / 16); //From (xxxx ____) partial, outputs as (0000 xxxx).
                }
            }
        }
        /// <summary>
        /// Peeks two crumbs (two pairs of bits) of data.
        /// [0]: Rightmost (lower) two bits. | [1]: Leftmost (higher) two bits.
        /// </summary>
        /// <returns></returns>
        public byte[] PeekTwoCrumbs()
        {
            return Pieces.DivideNibble(Peek());
        }
            #endregion //Peek
            #region Enqueue
        /// <summary>
        /// Enqueues two crumbs (two pairs of bits) of data.
        /// </summary>
        /// <param name="crumbRight">Rightmost (lower) two bits.</param>
        /// <param name="crumbLeft">Leftmost (higher) two bits.</param>
        public void Enqueue(byte crumbRight, byte crumbLeft)
        {
            if (crumbLeft > 3 || crumbRight > 3) throw new Exception("This operation intended for crumbs (numbers 0-3) only.");
            Enqueue((byte)(crumbLeft * 4 + crumbRight));
        }
        /// <summary>
        /// Enqueues a nibble (four bits) of data.
        /// </summary>
        /// <param name="nibble"></param>
        public void Enqueue(byte nibble)
        {
            if (nibble > 15) throw new Exception("This data structure is intended for nibbles (numbers 0-15) only.");
            if (!MidNibble_In) //Save nibble (0000 xxxx) into PartialIn as (xxxx 0000).
            {
                if (PartialIn > 0) throw new Exception("Unexpected data: PartialIn > 0 while MidNibble_In is false.");
                PartialIn = (byte)(nibble * 16); //Bitshift left by four bits. Saves as (xxxx 0000).
                MidNibble_In = true;
            } else { //Save nibble (0000 xxxx) into PartialIn as (____ xxxx), then Enqueue and subsequently reset PartialIn.
                //PartialIn starts as (____ 0000).
                PartialIn += (byte)(nibble); //Saves as (____ xxxx).
                Data.Enqueue(PartialIn); //Enqueues (xxxx xxxx) (first last).
                PartialIn = 0; //Saves as (0000 0000).
                MidNibble_In = false;
            }
        }
            #endregion //Enqueue
            #region Dequeue
        /// <summary>
        /// Dequeues a nibble (four bits) of data.
        /// </summary>
        /// <returns></returns>
        public byte Dequeue()
        {
            if (Count() == 0) throw new Exception("Cannot dequeue because entire object is empty of data.");
            if (MidNibble_Out)
            {
                if (PartialOut > 15) throw new Exception("Unexpected data: PartialOut > 15 while MidNibble_Out is true.");
                byte rtn = PartialOut; //From (0000 xxxx).
                PartialOut = 0; //Save as (0000 0000).
                MidNibble_Out = false;
                return rtn; //Outputs as (0000 xxxxx).
            } else {
                if (PartialOut > 0) throw new Exception("Unexpected data: PartialOut > 0 while MidNibble_Out is false.");
                if (Data.IsEmpty())
                {
                    if (MidNibble_In)
                    {
                        PartialOut = PartialIn;
                        PartialIn = 0;
                        MidNibble_In = false;
                    } else {
                        throw new Exception("Data queue empty while (MidNibble_Out is false, MidNibble_In is true, and Count() == 0).");
                    }
                } else {
                    PartialOut = Data.Dequeue(); //Save (xxxx xxxx).
                }
                MidNibble_Out = true;
                byte rtn = (byte)(PartialOut / 16); //From (____ xxxx) to (0000 xxxx).
                PartialOut %= 16; //Save (xxxx ____) as (0000 xxxx).
                return rtn; //Outputs as (0000 xxxx).
            }
        }
        /// <summary>
        /// Dequeues two crumbs (two pairs of bits) of data.
        /// [0]: Rightmost (lower) two bits. | [1]: Leftmost (hither) two bits.
        /// </summary>
        /// <returns></returns>
        public byte[] DequeueTwoCrumbs()
        {
            byte nibble = Dequeue();
            if (nibble > 15) throw new Exception("Unexpected data: dequeued nibble exceeds 15.");
            return Pieces.DivideNibble(nibble); //[Take rightmost (lower) two bits], [Leftmost (higher) by two bits].
        }
            #endregion //Dequeue
        #endregion //Methods
    }
}
