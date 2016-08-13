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
    public interface IHttpClient : IDisposable
    {
		Task<string> GetStringAsync(string requestUri);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
		Task<HttpResponseMessage> GetAsync (string requestUri);
		Task<byte[]> GetByteArrayAsync (string requestUri);
		Task<Stream> GetStreamAsync (string requestUri);
        TimeSpan TimeOut { get; set; }

        Uri BaseAddress { get; set; }
    }
}
