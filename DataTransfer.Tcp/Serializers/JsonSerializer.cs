using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataTransfer.Interfaces;
using Newtonsoft.Json;

namespace DataTransfer.Tcp.Serializers
{
    public class JsonSerializer : ISerializer
    {
        private JsonSerializerSettings Settings { get; }
        private char Delimeter => '\n';

        public JsonSerializer()
        {
            Settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
        }

        public byte[] GetBytes(IData data)
        {
            var json = JsonConvert.SerializeObject(data, Settings);
            return Encoding.UTF8.GetBytes($"{json}{Delimeter}");
        }

        public List<IData> GetDatas(byte[] bytes)
        {
            var json = Encoding.UTF8.GetString(bytes);
            var splited = json.Split(new char[] { Delimeter }, StringSplitOptions.RemoveEmptyEntries);
            return splited.Select(x => JsonConvert.DeserializeObject<IData>(x, Settings)).ToList();
        }
    }
}
