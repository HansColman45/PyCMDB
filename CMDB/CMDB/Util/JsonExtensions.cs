using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Util
{
    /// <summary>
    /// The JSON extension class
    /// </summary>
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
        /// <summary>
        /// convert object to json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }
        /// <summary>
        /// Convert object to json with custom settings
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string ToJson(this object obj, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value, Settings);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        /// <param name="settings"></param>
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
        /// <summary>
        /// Convert a string to an object of type T asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<T> ToObjectAsync<T>(this string value)
        {
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(value, Settings));
        }
        /// <summary>
        /// Convert a string to an object of type T asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<T> ToObjectAsync<T>(this Task<string> value)
        {
            var returnValue = await value;

            return await Task.Run(() => JsonConvert.DeserializeObject<T>(returnValue, Settings));
        }
        /// <summary>
        /// Convert a string to an anonymous type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="anonymousTypeObject"></param>
        /// <returns></returns>
        public static T ToAnonymousType<T>(this string value, T anonymousTypeObject)
        {
            return value.ToObject<T>();
        }
        /// <summary>
        /// Convert a string to an object of type T
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ToObject(this string value)
        {
            return JsonConvert.DeserializeObject(value, Settings);
        }
        /// <summary>
        /// Convert a string to an object of type T
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObject(this string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type, Settings);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="state"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetState<T>(this JObject state, string key, Func<T> defaultValue = null)
        {
            var item = state.GetValue(key, StringComparison.OrdinalIgnoreCase);

            if (item == null || item.Type == JTokenType.Null)
            {
                return defaultValue != null ? defaultValue() : default;
            }
            return item.ToObject<T>(Serializer);
        }
        /// <summary>
        /// Set the state of a key in the JObject
        /// </summary>
        /// <param name="state"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static JObject SetState(this JObject state, string key, object value)
        {
            state[key] = value != null ? JToken.FromObject(value, Serializer) : null;

            return state;
        }
    }
}
