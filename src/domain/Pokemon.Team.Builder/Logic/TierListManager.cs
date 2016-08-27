using System;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
	public class TierListManager
	{
		private ITierListRetriever _tierListRetriever;

		public TierListManager (ITierListRetriever tierListRetriever)
		{
			_tierListRetriever = tierListRetriever;
		}

		public async Task<TierList> GetTierList (string fileName){
			var tierList = await GenericSerializer<TierList>.LoadFromFile (fileName).ConfigureAwait(false);

			if (tierList == null) {
				tierList = new TierList(_tierListRetriever.RetrieveTierLists ());

				await GenericSerializer<TierList>.SaveToFile (tierList, fileName).ConfigureAwait(false);
			}

			return tierList;
		}
	}
}

