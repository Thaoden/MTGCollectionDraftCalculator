using System;

namespace MTGDraftCollectionCalculator
{
    class Program
    {
        private readonly static Random _rng = new Random();
        private readonly static bool _debug = false;

        static void Main(string[] args)
        {
            var userCollection = createUserCollection();
            Console.WriteLine(userCollection.ToString());

            int draftsNeeded = calculateDrafts(userCollection);

            Console.WriteLine($"Estimated runs to complete the rare collection: {draftsNeeded}");
            Console.WriteLine(userCollection.ToString());
        }

        private static int calculateDrafts(UserCollection userCollection)
        {
            int draftsNeeded = 0;

            while (userCollection.Rares.DraftablesNeeded + userCollection.Rares.NonDraftablesNeeded >= userCollection.BoosterPacksOwned)
            {
                simulateSingleDraft(userCollection);
                draftsNeeded++;
            }

            return draftsNeeded;
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
            userCollection.Rares.DraftablesOwned = 12;
            userCollection.BoosterPacksOwned = 19;
            userCollection.Wildcards.Owned = 0;
            userCollection.Wildcards.Progress = 0;

            return userCollection;
        }
    }
}
