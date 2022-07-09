using System.Text;
using DataTransfer.Interfaces;
using Newtonsoft.Json;

namespace DataTransfer.Tcp.Serializers
{
    public class JsonSerializer : ISerializer
    {
        private JsonSerializerSettings Settings { get; }

        public JsonSerializer()
        {
            Settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
        }

        public byte[] GetBytes(IData data)
        {
            var json = JsonConvert.SerializeObject(data, Settings);
            return Encoding.UTF8.GetBytes(json);
        }

        public IData GetData(byte[] bytes)
        {
            var json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<IData>(json, Settings);
        }
    }
}
