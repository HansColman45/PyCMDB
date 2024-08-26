using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;

namespace CMDB.Util
{
    public static class ApiClientHelper
    {
        public static ByteArrayContent ConvertObjectToHttpContent<T>(T obj)
        {
            var content = obj.ToJson();
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            return byteContent;
        }
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync();
            return dataAsString.ToObject<T>();
        }
        public static void SetBearerToken(this HttpClient apiClient, string token)
        {
            apiClient.DefaultRequestHeaders.Remove("Authorization");
            apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var content = ToJson(data);
            var responce = await httpClient.PostAsync(url, content);
            return responce;
        }
        public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var content = ToJson(data);
            var responce = await httpClient.PutAsync(url, content);
            return responce;
        }
        private static StringContent ToJson<T>(T data)
        {
            var dataAsString = data.ToJson();
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            return content;
        }
    }
}
