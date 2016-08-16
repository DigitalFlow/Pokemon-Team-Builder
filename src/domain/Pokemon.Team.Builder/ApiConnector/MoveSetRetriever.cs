using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.ApiConnector
{
    public class MoveSetRetriever : IMoveSetRetriever
    {
        private IHttpClient _client;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public MoveSetRetriever(IHttpClient client)
        {
            _client = client;
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task RetrieveAllMoveSets()
        {
            var response = await _client.GetStringAsync("script_res/setdex_showdown.js").ConfigureAwait(false);

            var beginning = "var SETDEX_XY = {\n\t";

            response = response.Substring(beginning.Length);

            var json = JsonConvert.DeserializeObject(response);

            var test = "foo";
        }
    }
}
