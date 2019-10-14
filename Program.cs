using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MTGDraftCollectionCalculator
{
    class Program
    {
        private readonly static Random _rng = new Random();
        private readonly static int _amountOfSimulations = 1000;
        private readonly static bool _debug = false;

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
            var rnd = _rng.Next(SetCollection.Eldraine.Rares.Draftable);

            if (_debug)
            {
                Console.WriteLine($"Current number of owned draftables: {userCollection.Rares.DraftablesOwned}{Environment.NewLine}Opened rare number {rnd}");
            }

            if (rnd > userCollection.Rares.DraftablesOwned)
            {
                userCollection.Rares.DraftablesOwned++;
            }
        }


        private static UserCollection createUserCollection()
        {
            var userCollection = new UserCollection();
            userCollection.Rares.NonDraftablesOwned = 0;
            userCollection.Rares.DraftablesOwned = 16;
            userCollection.BoosterPacksOwned = 25;
            userCollection.Wildcards.Owned = 0;
            userCollection.Wildcards.Progress = 0;

            return userCollection;
        }
    }
}
