using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

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

		/// Using ConfigureAwait for avoiding deadlocks
		public async Task<Stream> GetStreamAsync(string requestUri) {
			return await _client.GetStreamAsync (requestUri).ConfigureAwait (false);
		}

		public async Task<byte[]> GetByteArrayAsync(string requestUri) {
			return await _client.GetByteArrayAsync (requestUri).ConfigureAwait (false);
		}

		public async Task<HttpResponseMessage> GetAsync(string requestUri)
		{
			return await _client.GetAsync(requestUri).ConfigureAwait (false);
		}

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
			return await _client.SendAsync(request).ConfigureAwait (false);
        }

        public async Task<string> GetStringAsync(string requestUri)
        {
			return await _client.GetStringAsync(requestUri).ConfigureAwait (false);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
