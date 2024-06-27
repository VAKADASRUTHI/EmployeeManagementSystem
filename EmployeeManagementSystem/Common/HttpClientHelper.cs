using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Common
{
    public static class HttpClientHelper
    {
        public static async Task<HttpResponseMessage> MakePostRequest(HttpClient client, string url, string content)
        {
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            return await client.PostAsync(url, stringContent);
        }

        public static async Task<HttpResponseMessage> MakeGetRequest(HttpClient client, string url)
        {
            return await client.GetAsync(url);
        }
    }
}