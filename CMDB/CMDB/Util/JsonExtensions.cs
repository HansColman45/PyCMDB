using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Util
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerSettings Settings = new()
        {
            Formatting = Formatting.None,
            ContractResolver = new CustomResolver(),
            //ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        private static readonly JsonSerializer Serializer = JsonSerializer.Create(Settings);

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        public static string ToJson(this object obj, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T ToObject<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value, Settings);
        }

        public static T ToObject<T>(this Stream stream)
        {
            if (stream == null || stream.CanRead == false) return default;

            using (var sr = new StreamReader(stream))
            {
                using (var jtr = new JsonTextReader(sr))
                {
                    return Serializer.Deserialize<T>(jtr);
                }
            }
        }
        public static void SerializeObjectIntoStream(this object value, Stream stream, JsonSerializerSettings settings = null)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            {
                using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
                {
                    if (settings != null)
                    {
                        JsonSerializer.Create(settings).Serialize(jtw, value);
                    }
                    else
                    {
                        Serializer.Serialize(jtw, value);
                    }

                    jtw.Flush();
                }
            }
        }
        public static async Task<T> ToObjectAsync<T>(this string value)
        {
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(value, Settings));
        }
        public static async Task<T> ToObjectAsync<T>(this Task<string> value)
        {
            var returnValue = await value;

            return await Task.Run(() => JsonConvert.DeserializeObject<T>(returnValue, Settings));
        }
        public static T ToAnonymousType<T>(this string value, T anonymousTypeObject)
        {
            return value.ToObject<T>();
        }
        public static object ToObject(this string value)
        {
            return JsonConvert.DeserializeObject(value, Settings);
        }
        public static object ToObject(this string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type, Settings);
        }
        public static T GetState<T>(this JObject state, string key, Func<T> defaultValue = null)
        {
            var item = state.GetValue(key, StringComparison.OrdinalIgnoreCase);

            if (item == null || item.Type == JTokenType.Null)
            {
                return defaultValue != null ? defaultValue() : default;
            }
            return item.ToObject<T>(Serializer);
        }
        public static JObject SetState(this JObject state, string key, object value)
        {
            state[key] = value != null ? JToken.FromObject(value, Serializer) : null;

            return state;
        }
    }
}
