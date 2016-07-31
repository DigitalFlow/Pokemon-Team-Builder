using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
    public class RetrievePokemonResponse
    {
		/// <summary>
		/// Overall result count of request
		/// </summary>
		/// <value>The count.</value>
        public int Count { get; set; }

		/// <summary>
		/// Previous page of results
		/// </summary>
		/// <value>The previous.</value>
        public object Previous { get; set; }

		/// <summary>
		/// List of results in current page
		/// </summary>
		/// <value>The results.</value>
        public List<SimplePokemonData> Results { get; set; }

		/// <summary>
		/// Next page of results
		/// </summary>
		/// <value>The next.</value>
        public string Next { get; set; }        
    }
}
