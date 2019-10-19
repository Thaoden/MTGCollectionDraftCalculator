using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace MTGDraftCollectionCalculator
{
    class Program
    {
        private readonly static Random _rng = new Random();
        private const int AMOUNT_OF_SIMULATIONS = 1000;
        private const bool DEBUG = false;

        private readonly string _userCollectionFilename = "mtgcollection.csv";

        static async Task Main(string[] args)
        {
            var eldraineCards = await MtgJsonHelper.GetEldraineSet();

            var userCollection = createUserCollection(eldraineCards);
            Console.WriteLine(userCollection.ToString());

            int draftsNeeded = calculateDrafts(userCollection);

            Console.WriteLine($"Estimated runs to complete the rare collection: {draftsNeeded}");
        }




        private static int calculateDrafts(UserCollection userCollection)
        {
            int draftsNeeded = 0;

            for (int i = 0; i < AMOUNT_OF_SIMULATIONS; i++)
            {
                int tmpDraftsNeeded = 0;

                var tmpCollection = new UserCollection(userCollection);

                while (tmpCollection.Rares.DraftablesNeeded + tmpCollection.Rares.NonDraftablesNeeded >= tmpCollection.BoosterPacksOwned)
                {
                    simulateSingleDraft(tmpCollection);
                    tmpDraftsNeeded++;
                }


                if (DEBUG)
                {
                    Console.WriteLine($"Running simulation {i + 1}, estimating {tmpDraftsNeeded} drafts");
                }

                draftsNeeded += tmpDraftsNeeded;
            }

            return draftsNeeded / AMOUNT_OF_SIMULATIONS;
        }

        private static void simulateSingleDraft(UserCollection userCollection)
        {
            for (int i = 0; i < 3; i++)
            {
                simulateSinglePack(userCollection);
            }
            userCollection.BoosterPacksOwned++;
        }

        private static void simulateSinglePack(UserCollection userCollection)
        {
            var allDraftableCardNames = userCollection.Rares.GetCardNames(draftable: true);
            var cardDrawnIndex = _rng.Next(allDraftableCardNames.Count);
            var drawnCardName = allDraftableCardNames[cardDrawnIndex];

            if (DEBUG)
            {
                Console.WriteLine($"Current number of owned draftables: {userCollection.Rares.DraftablesOwned}{Environment.NewLine}Opened rare number {cardDrawnIndex}");
            }

            if (!userCollection.Rares.IsCompletePlayset(drawnCardName))
            {
                userCollection.Rares.AddCard(drawnCardName, draftable: true);

                if (DEBUG)
                {
                    Console.WriteLine($"Adding card {drawnCardName} to collection");
                }
            }
        }


        private static UserCollection createUserCollection(List<Card> eldraineCards)
        {
            var userCollection = new UserCollection(eldraineCards);


            userCollection.BoosterPacksOwned = 0;
            userCollection.Wildcards.Owned = 0;
            userCollection.Wildcards.Progress = 0;

            return userCollection;
        }
    }
}
