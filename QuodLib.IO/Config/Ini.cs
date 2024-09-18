using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.IO.Config {
    public static class Ini {
        public static Dictionary<string, string> Load(string filename) {
            string[] entries = File.ReadAllText(filename).Replace("\r\n", "\n").Split('\n');
            Dictionary<string, string> rtn = new Dictionary<string, string>();
            foreach (string entry in entries)
                if (entry != "") {
                    string[] splEnt = entry.Split('=');
                    rtn.Add(splEnt[0], splEnt[1]);
                }

            return rtn;
        }
        public static void Save(Dictionary<string, string> settings, string filename) {
            using StreamWriter wrt = new(filename);
            foreach (string key in settings.Keys)
                wrt.WriteLine(key + "=" + settings[key]);
        }
        public static void Update(string key, string value, string filename, bool WriteIfNotFound) {
            Dictionary<string, string> settings = Load(filename);
            if (settings.ContainsKey(key))
                settings[key] = value;
            else {
                if (WriteIfNotFound) settings.Add(key, value);
                else throw new Exception("Error: Key \"" + key + "\" not found in \"" + filename.Split("\n", -1) + "\".");
            }
            Save(settings, filename);
        }
        public static void Remove(string key, string filename, bool ignoreAlreadyMissing) {
            Dictionary<string, string> settings = Load(filename);
            if (settings.ContainsKey(key))
                settings.Remove(key);
            else if (!ignoreAlreadyMissing) throw new Exception("Error: Key \"" + key + "\" not found in \"" + filename.Split("\n", -1) + "\".");

            Save(settings, filename);
        }
    }
}
