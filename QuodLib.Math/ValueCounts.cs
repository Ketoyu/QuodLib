using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Math {
    public static class ValueCounts {
        //Primarily used by Binomial Theorem (namely, "CHS_simp") function(s) in Math.General.
        #region Dictionary<T, int>
        /// <summary>
        /// Increments the entry's #ofinstances value.
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Dictionary<T, int> IncrementOrAdd<T>(Dictionary<T, int> dic, T key) where T : notnull {
            int ct = 1;
            if (dic.ContainsKey(key)) ct = dic[key] + 1;
            return SetOrAdd(dic, key, ct);
        }
        /// <summary>
        /// Decrements the entry's #ofinstances value.
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Dictionary<T, int> DecrementOrRemove<T>(Dictionary<T, int> dic, T key) where T : notnull {
            Dictionary<T, int> rtn = dic;
            if (dic.ContainsKey(key)) {
                DecrementOrAdd(rtn, key, 1);
                if (rtn[key] == 0) rtn.Remove(key);
            }
            return rtn;
        }
        /// <summary>
        /// Sets the entry's #ofinstances value
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Dictionary<T, int> SetOrAdd<T>(Dictionary<T, int> dic, T key, int val) where T : notnull {
            Dictionary<T, int> rtn = dic;
            if (rtn.ContainsKey(key)) {
                rtn.Remove(key);
                rtn.Add(key, val);
            }
            else
                rtn.Add(key, val);

            return rtn;
        }
        /// <summary>
        /// Adds an entry to the dictionary, or increments its #ofinstances value if the entry already exists.
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Dictionary<T, int> IncrementOrAdd<T>(Dictionary<T, int> dic, T key, int val) where T : notnull {
            Dictionary<T, int> rtn = dic;
            int ct = val;
            if (rtn.ContainsKey(key)) ct += rtn[key];
            return SetOrAdd(dic, key, ct);
        }
        /// <summary>
        /// Decrements an entry's #ofinstances value, or removes the entry if its #ofinstances value reaches zero.
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Dictionary<T, int> DecrementOrAdd<T>(Dictionary<T, int> dic, T key, int val) where T : notnull {
            return IncrementOrAdd(dic, key, 0 - val);
        }

        /// <summary>
        /// Decompiles the group-by-entry dictionary to a normal list. Ex: { {1, 5}, {2, 1} } -> {1, 1, 1, 1, 1, 2}.
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static List<T> MultipleToList<T>(Dictionary<T, int> dic) where T : notnull {
            List<T> rtn = new List<T>();
            foreach (T itm in dic.Keys)
                if (dic[itm] > 1)
                    for (int i = 1; i <= dic[itm]; i++)
                        rtn.Add(itm);
                else
                    rtn.Add(itm);

            return rtn;
        }
        #endregion //Dictionary<T, int>
    }
}
