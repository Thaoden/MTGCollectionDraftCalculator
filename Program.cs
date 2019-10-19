using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MTGDraftCollectionCalculator
{
    class Program
    {
        private readonly static Random _rng = new Random();
        private const int AMOUNT_OF_SIMULATIONS = 1000;
        private const bool DEBUG = false;

        private static readonly string _userCollectionFilename = "mtgcollection.csv";

        static async Task Main(string[] args)
        {
            var eldraineCards = await MtgJson.MtgJsonHelper.GetEldraineSet();
            var collectedCards = await parseMtgaToolDump();

            var userCollection = createUserCollection(eldraineCards, collectedCards);
            Console.WriteLine(userCollection.ToString());

            int draftsNeeded = calculateDrafts(userCollection);

            Console.WriteLine($"Estimated runs to complete the rare collection: {draftsNeeded}");
        }


        private async static Task<List<MtgaTool.MtgaToolCard>> parseMtgaToolDump()
        {
            if (!File.Exists(_userCollectionFilename))
            {
                Console.WriteLine($"MTGATool dump file with filename {_userCollectionFilename} not found, assuming empty collection");
                return new List<MtgaTool.MtgaToolCard>();

            }

            Console.WriteLine("Found MTGATool dump file, parsing it");
            var userCollectionCsv = await File.ReadAllTextAsync(_userCollectionFilename);
            var userCollection = new List<MtgaTool.MtgaToolCard>();

            foreach (var cardEntryLine in userCollectionCsv.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                var cardEntry = cardEntryLine.Split(";");
                var card = new MtgaTool.MtgaToolCard
                {
                    Name = cardEntry[0].Replace("\"", ""),
                    SetCode = cardEntry[1],
                    SetName = cardEntry[2],
                    Rarity = cardEntry[3],
                    Count = Int32.Parse(cardEntry[4])
                };

                if (card.SetCode == "ELD")
                {
                    userCollection.Add(card);
                }
            }


            return userCollection;
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
                userCollection.Rares.AddCard(drawnCardName);

                if (DEBUG)
                {
                    Console.WriteLine($"Adding card {drawnCardName} to collection");
                }
            }
        }


        private static UserCollection createUserCollection(List<MtgJson.Card> eldraineCards, List<MtgaTool.MtgaToolCard> collectedEldraineCards)
        {
            var userCollection = new UserCollection(eldraineCards);

            foreach (var collectedCard in collectedEldraineCards.Where(cc => cc.Rarity == "rare").ToList())
            {
                for (int i = 0; i < collectedCard.Count; i++)
                {
                    userCollection.Rares.AddCard(collectedCard.Name);
                }
            }

            userCollection.BoosterPacksOwned = 35;
            userCollection.Wildcards.Owned = 0;
            userCollection.Wildcards.Progress = 0;

            return userCollection;
        }
    }
}
