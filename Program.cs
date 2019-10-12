using System;

namespace MTGDraftCollectionCalculator
{
    class Program
    {
        private readonly static Random _rng = new Random();

        static void Main(string[] args)
        {
            var userCollection = createUserCollection();
            Console.WriteLine(userCollection.ToString());

            calculateNonDraftableCollectionProgress(userCollection);
            Console.WriteLine($"Owning {userCollection.BoosterPacksOwned} booster packs after non-draftable progression.{Environment.NewLine}");

            double draftsNeeded = calculateDrafts(userCollection);

            Console.WriteLine($"Estimated runs to complete the rare collection: {Math.Floor(draftsNeeded)}");
        }

        private static void calculateNonDraftableCollectionProgress(UserCollection userCollection)
        {
            Console.WriteLine($"Using {userCollection.BoosterPacksOwned} packs to advance the non-draftable collection...");

            if (userCollection.Rares.NonDraftablesNeeded > userCollection.BoosterPacksOwned)
            {
                // we don't have enough boosters to complete the non-draftableCollection
                // -> use all available booster as non-draftable collection progress
                userCollection.Rares.NonDraftablesOwned += userCollection.BoosterPacksOwned;
                userCollection.BoosterPacksOwned = 0;

                Console.WriteLine($"... still need {userCollection.Rares.NonDraftablesNeeded - userCollection.BoosterPacksOwned} non-draftable rare(s).");
            }
            else
            {
                // we have enough boosters to complete the non-draftable collection
                // -> use all necessary boosters as non-draftable collection progress, keep the remaining ones
                userCollection.BoosterPacksOwned -= userCollection.Rares.NonDraftablesNeeded;
                userCollection.Rares.NonDraftablesOwned = SetCollection.Eldraine.Rares.NonDraftable;

                Console.WriteLine($"Enough packs to complete the non-draftable collection :)");
            }
        }


        private static double calculateDrafts(UserCollection userCollection)
        {
            var draftsNeeded = 0.0;

            while (userCollection.Rares.DraftablesNeeded > userCollection.BoosterPacksOwned)
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
