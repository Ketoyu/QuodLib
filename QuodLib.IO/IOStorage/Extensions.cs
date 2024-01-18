using QuodLib.Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.IO.IOStorage {
    public static class Extensions {
        public static byte[] Serialize<T>(this T data) where T : class, IProto {
            using var ms = new MemoryStream();
            Serializer.Serialize<T>(ms, data);
            return ms.ToArray();
        }

        public static void WriteToFile<T>(this T data, string filename) where T : class, IProto
            => File.WriteAllBytes(filename, data.Serialize());

        public static T ReadFromFile<T>(string filename) where T : class, IProto<T> {
            using var fl = File.OpenRead(filename);
            return Serializer.Deserialize<T>(fl);
        }

        public static IProto ReadProtoFromFile(string filename) {
            using var fl = File.OpenRead(filename);
            return Serializer.Deserialize<IProto>(fl);
        }

        public static T Deserialize<T>(this Stream data) where T : class, IProto<T>
            => Serializer.Deserialize<T>(data);
    }
}
