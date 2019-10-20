using System;
using System.Collections.Generic;
using System.Text;

namespace MTGDraftCollectionCalculator
{
    internal class DraftEstimator
    {
        private readonly static Random _rng = new Random();
        private readonly bool _debug;
        private readonly int _amountOfSimulations;

        public DraftEstimator(int amountOfSimulations, bool debug)
        {
            _amountOfSimulations = amountOfSimulations;
            _debug = debug;
        }

        public int CalculateDrafts(UserCollection userCollection)
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

        private void simulateSingleDraft(UserCollection userCollection)
        {
            for (int i = 0; i < 3; i++)
            {
                simulateSinglePack(userCollection);
            }
            userCollection.BoosterPacksOwned++;
        }

        private void simulateSinglePack(UserCollection userCollection)
        {
            var allDraftableCardNames = userCollection.Rares.GetCardNames(draftable: true);
            var cardDrawnIndex = _rng.Next(allDraftableCardNames.Count);
            var drawnCardName = allDraftableCardNames[cardDrawnIndex];

            if (_debug)
            {
                Console.WriteLine($"Current number of owned draftables: {userCollection.Rares.DraftablesOwned}{Environment.NewLine}Opened rare number {cardDrawnIndex}");
            }

            if (!userCollection.Rares.IsCompletePlayset(drawnCardName))
            {
                userCollection.Rares.AddCard(drawnCardName);

                if (_debug)
                {
                    Console.WriteLine($"Adding card {drawnCardName} to collection");
                }
            }
        }
    }
}
