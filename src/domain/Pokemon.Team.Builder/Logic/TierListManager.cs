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

		public TierList GetTierList (){
			var tierList = TierListSerializer.LoadTierListFromFile ("tierList.xml");

			if (tierList == null) {
				tierList = _tierListRetriever.RetrieveTierLists ();

				TierListSerializer.SaveTierListToFile (tierList, "tierList.xml");
			}

			return new TierList(tierList);
		}
	}
}

