using System;

namespace Pokemon.Team.Builder
{
	public class TierListManager
	{
		private ITierListRetriever _tierListRetriever;

		public TierListManager (ITierListRetriever tierListRetriever)
		{
			_tierListRetriever = tierListRetriever;
		}

		public TierList GetTierList (string fileName){
			var tierList = GenericSerializer<TierList>.LoadFromFile (fileName);

			if (tierList == null) {
				tierList = new TierList(_tierListRetriever.RetrieveTierLists ());

				GenericSerializer<TierList>.SaveToFile (tierList, fileName);
			}

			return tierList;
		}
	}
}

