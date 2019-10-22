using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace MTGDraftCollectionCalculator
{
    class Program
    {
        /*
        <== PlayerInventory.GetPlayerSequenceData
<== Event.GetActiveEventsV2

<== Event.GetPlayerCoursesV2
<== PlayerInventory.GetProductCatalog
         */
        private const int AMOUNT_OF_SIMULATIONS = 1000;
        private const bool DEBUG = false;

        static async Task Main(string[] args)
        {
            var mtgaLogHelper = new MtgaLogParser.MtgaLogHelper();
            var ownedCardsFromMtgaLog = await mtgaLogHelper.GetOwnedCardCollection();
            var playerInventory = await mtgaLogHelper.GetPlayerInventory();

            using var helper = new MtgaTool.MtgaToolDatabaseHelper();
            var eldraineSetCardsFromMtgaTool = await helper.GetEldraineSetAsync();

            var eldraineSetWithoutAdventures = eldraineSetCardsFromMtgaTool.Where(c => !c.Type.Contains("Adventure")).ToList();
            var ownedCards = getOwnedCardsInSetCollectionAsync(ownedCardsFromMtgaLog, eldraineSetCardsFromMtgaTool);

            var allSetsFromMtgaTool = await helper.GetAllSetsDetails();
            int eldraineBoosters = playerInventory.Boosters.Single(b => b.CollationId == allSetsFromMtgaTool.Single(s => s.Set.ArenaCode == "ELD").Set.Collation.GetInt32()).Count;

            var userCollection = UserCollection.CreateUserCollection(eldraineSetWithoutAdventures, ownedCards, eldraineBoosters);
            Console.WriteLine(userCollection.ToString());

            int draftsNeeded = estimateNeededDrafts(userCollection);
            Console.WriteLine($"Estimated runs to complete the rare collection: {draftsNeeded}");
        }


        private static int estimateNeededDrafts(UserCollection userCollection)
        {
            var draftEstimator = new DraftEstimator(AMOUNT_OF_SIMULATIONS, DEBUG);
            int draftsNeeded = draftEstimator.CalculateDrafts(userCollection);
            return draftsNeeded;
        }


        private static List<(MtgaTool.DatabaseCard Card, int CardsOwned)> getOwnedCardsInSetCollectionAsync(List<(int CardIndex, int CardsOwned)> ownedCardsFromMtgaLog, List<MtgaTool.DatabaseCard> eldraineSetCardsFromMtgaTool)
        {
            var ownedCards = ownedCardsFromMtgaLog
                .Where(ownedCard => eldraineSetCardsFromMtgaTool.Select(c => c.Id).Contains(ownedCard.CardIndex))
                .Select(item => (eldraineSetCardsFromMtgaTool.Single(c => c.Id == item.CardIndex), item.CardsOwned))
                .ToList();

            return ownedCards;
        }
    }
}
