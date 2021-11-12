using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Auth.Core.Services
{
    public interface IHttpClientService
    {
        Task<T> Get<T>(string baseUri, string queryParam = null);
        Task<T> Post<T, T1>(string baseUri, T1 body, string queryParam = null);
    }
    public interface IHttpGoogleClientService
    {
        Task<T> Get<T>(string baseUri, string queryParam = null);
        Task<T> Post<T, T1>(string baseUri, T1 body, string queryParam = null);
    }
    public class HttpClientService : IHttpClientService ,IHttpGoogleClientService
    {
        HttpClient client;
        public HttpClientService(string baseUri)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<T> Get<T>(string methodUri, string queryParam = null)
        {
            try
            {
                StringBuilder uriBuilder = new StringBuilder();
                uriBuilder.Append(methodUri);
                if (queryParam != null)
                {
                    uriBuilder.Append("?");
                    uriBuilder.Append(queryParam);
                }
                string url = uriBuilder.ToString();

                HttpResponseMessage response = await client.GetAsync(url);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                T responseT  = default;
                if (response.IsSuccessStatusCode)
                {
                    responseT = await JsonSerializer.DeserializeAsync<T>(response.Content.ReadAsStreamAsync().Result, options);
                }

                return responseT;
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw exception;
            }
        }

        public Task<T> Post<T, T1>(string methodUri, T1 body, string queryParam = null)
        {
            throw new NotImplementedException();
        }
    }
}
