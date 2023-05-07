using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;


namespace IDeal.Szx.CsharpUtilibs.Serialization {
    public static class Json {
        static readonly DataContractJsonSerializerSettings JsonSettings = new DataContractJsonSerializerSettings {
            UseSimpleDictionaryFormat = true
        };

        public static void save<T>(string path, T obj) {
            using (FileStream fs = File.Open(path,
                FileMode.Create, FileAccess.Write, FileShare.Read)) {
                serialize<T>(fs, obj);
            }
        }

        public static T load<T>(string path) where T : new() {
            if (!File.Exists(path)) { return new T(); }
            using (FileStream fs = File.Open(path,
                FileMode.Open, FileAccess.Read, FileShare.Read)) {
                return deserialize<T>(fs);
            }
        }

        public static void serialize<T>(Stream stream, T obj) {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T), JsonSettings);
            js.WriteObject(stream, obj);
        }

        public static T deserialize<T>(Stream stream) {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T), JsonSettings);
            return (T)js.ReadObject(stream);
        }

        public static string toJsonString<T>(T obj) {
            using (MemoryStream ms = new MemoryStream()) {
                serialize(ms, obj);
                return ms.toString();
            }
        }

        public static T fromJsonString<T>(string json) {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json))) {
                return deserialize<T>(ms);
            }
        }
    }
}
