using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.ApiConnector
{
    public interface ISmogonStatRetriever : IDisposable
    {
        Task RetrieveStats(string tier);
    }
}
