using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MTGDraftCollectionCalculator
{
    class Program
    {
        private readonly static Random _rng = new Random();
        private readonly static int _amountOfSimulations = 1000;
        private readonly static bool _debug = true;

        static void Main(string[] args)
        {
            var userCollection = createUserCollection();
            Console.WriteLine(userCollection.ToString());

            int draftsNeeded = calculateDrafts(userCollection);

            Console.WriteLine($"Estimated runs to complete the rare collection: {draftsNeeded}");
        }

        private static int calculateDrafts(UserCollection userCollection)
        {
            int draftsNeeded = 0;

            for (int i = 0; i < _amountOfSimulations; i++)
            {
                int tmpDraftsNeeded = 0;

                var tmpCollection = new UserCollection(userCollection);

                while (tmpCollection.Rares.DraftablesNeeded + tmpCollection.Rares.NonDraftablesNeeded >= tmpCollection.BoosterPacksOwned)
                {
                    simulateSingleDraft(tmpCollection);
                    tmpDraftsNeeded++;
                }


                if (_debug)
                {
                    Console.WriteLine($"Running simulation {i + 1}, estimating {tmpDraftsNeeded} drafts");
                }

                draftsNeeded += tmpDraftsNeeded;
            }

            return draftsNeeded / _amountOfSimulations;
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

            if (_debug)
            {
                Console.WriteLine($"Current number of owned draftables: {userCollection.Rares.DraftablesOwned}{Environment.NewLine}Opened rare number {cardDrawn}");
            }

            if (!userCollection.Rares.ContainsDraftable(cardDrawn))
            {
                userCollection.Rares.AddDraftable(cardDrawn);
                Console.WriteLine($"Adding card {cardDrawn} to collection");
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

            for(int i = 0; i < draftablesOwned; i++)
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
