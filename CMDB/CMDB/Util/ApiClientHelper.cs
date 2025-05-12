using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System;

namespace CMDB.Util
{
    /// <summary>
    /// Helper class for API operations
    /// </summary>
    public static class ApiClientHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ByteArrayContent ConvertObjectToHttpContent<T>(T obj)
        {
            var content = obj.ToJson();
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            return byteContent;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync();
            return dataAsString.ToObject<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiClient"></param>
        /// <param name="token"></param>
        public static void SetBearerToken(this HttpClient apiClient, string token)
        {
            apiClient.DefaultRequestHeaders.Remove("Authorization");
            apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var content = ToJson(data);
            var responce = await httpClient.PostAsync(url, content);
            return responce;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var content = ToJson(data);
            var responce = await httpClient.PutAsync(url, content);
            return responce;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var content = ToJson(data);
            HttpRequestMessage request = new()
            {
                Content = content,
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url, UriKind.Absolute)
            };
            return await httpClient.SendAsync(request);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static StringContent ToJson<T>(T data)
        {
            var dataAsString = data.ToJson();
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            return content;
        }
    }
}
