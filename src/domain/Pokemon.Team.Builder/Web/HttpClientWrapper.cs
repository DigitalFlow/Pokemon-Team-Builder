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

		public Uri BaseAddress { get; set; }

        public HttpClientWrapper(Uri baseUrl)
        {
            _client = new HttpClient
            {
                BaseAddress = baseUrl
            };

			BaseAddress = _client.BaseAddress;
        }

		public async Task<HttpResponseMessage> GetAsync(string requestUri)
		{
			return await _client.GetAsync(requestUri);
		}

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await _client.SendAsync(request);
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
