using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
    public interface IHttpClient : IDisposable
    {
        Task<string> GetStringAsync(string requestUri);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
		Task<HttpResponseMessage> GetAsync (string requestUri);

		Uri BaseAddress { get; set; }
    }
}
