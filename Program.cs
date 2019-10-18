using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace MTGDraftCollectionCalculator
{
    class Program
    {
        private readonly static Random _rng = new Random();
        private const int AMOUNT_OF_SIMULATIONS = 1000;
        private const bool DEBUG = false;

        static async Task Main(string[] args)
        {
            var eldraineCollection = await getMtgJsonSet("ELD.json");

            var userCollection = createUserCollection();
            Console.WriteLine(userCollection.ToString());

            int draftsNeeded = calculateDrafts(userCollection);

            Console.WriteLine($"Estimated runs to complete the rare collection: {draftsNeeded}");
        }

        private static async Task<MtgJsonSet> getMtgJsonSet(string set)
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true,
            };
            string json = "";

            using var _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://www.mtgjson.com/json/");

            using var response = await _httpClient.GetAsync(set);
            if (response.IsSuccessStatusCode)
            {
                //var collection = await response.Content.ReadAsAsync<MtgJsonSet>();
                json = await response.Content.ReadAsStringAsync();
            }
            var collection = JsonSerializer.Deserialize<MtgJsonSet>(json, jsonSerializerOptions);
            return collection;
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
            var cardDrawn = _rng.Next(SetCollection.Eldraine.Rares.Draftable);

            if (DEBUG)
            {
                Console.WriteLine($"Current number of owned draftables: {userCollection.Rares.DraftablesOwned}{Environment.NewLine}Opened rare number {cardDrawn}");
            }

            if (!userCollection.Rares.ContainsDraftable(cardDrawn))
            {
                userCollection.Rares.AddDraftable(cardDrawn);

                if (DEBUG)
                {
                    Console.WriteLine($"Adding card {cardDrawn} to collection");
                }
            }
        }


        private static UserCollection createUserCollection()
        {
            var userCollection = new UserCollection();
            int nonDraftablesOwned = 0;
            int draftablesOwned = 31;

            for (int i = 0; i < nonDraftablesOwned; i++)
            {
                userCollection.Rares.AddNonDraftable(i);
            }

            for (int i = 0; i < draftablesOwned; i++)
            {
                userCollection.Rares.AddDraftable(i);
            }
            userCollection.BoosterPacksOwned = 33;
            userCollection.Wildcards.Owned = 0;
            userCollection.Wildcards.Progress = 0;

            return userCollection;
        }
    }
}
