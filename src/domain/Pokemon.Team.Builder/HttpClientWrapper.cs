using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
    public class HttpClientWrapper : IHttpClient, IDisposable
    {
        private HttpClient _client;

        public HttpClientWrapper(Uri baseUrl)
        {
            _client = new HttpClient
            {
                BaseAddress = baseUrl
            };
        }

        public Task<string> GetStringAsync(string requestUri)
        {
            return _client.GetStringAsync(requestUri);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
