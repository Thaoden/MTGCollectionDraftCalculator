using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGDraftCollectionCalculator
{
    public class UserCollection
    {
        public UserRareCollection Rares { get; }
        public UserWildcardCollection Wildcards { get; } = new UserWildcardCollection();

        public int BoosterPacksOwned { get; set; }

        public UserCollection(List<Card> eldraineCards)
        {
            var rareCards = eldraineCards.Where(c => c.Rarity == Rarity.Rare).ToLookup(c => c.IsStarter);
            Rares = new UserRareCollection(rareCards);
            BoosterPacksOwned = 0;
        }

        public UserCollection(UserCollection sourceCollection)
        {
            Rares = new UserRareCollection(sourceCollection.Rares.GetCardNames(draftable: true), sourceCollection.Rares.GetCardNames(draftable: false));

            foreach (var collected in sourceCollection.Rares.CollectedDraftables)
            {
                Rares.AddCard(collected, draftable: true);
            }

            foreach (var collected in sourceCollection.Rares.CollectedNonDraftables)
            {
                Rares.AddCard(collected, draftable: false);
            }

            Wildcards.Owned = sourceCollection.Wildcards.Owned;
            Wildcards.Progress = sourceCollection.Wildcards.Progress;

            BoosterPacksOwned = sourceCollection.BoosterPacksOwned;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"{Environment.NewLine}Current Collection stats:{Environment.NewLine}{Environment.NewLine}");
            sb.Append($"RARES{Environment.NewLine}");
            sb.Append($"------{Environment.NewLine}");
            sb.Append($"\t\tOwned\tNeeded{Environment.NewLine}");
            sb.Append($"Draftable:\t{Rares.DraftablesOwned}\t{Rares.DraftablesNeeded}{Environment.NewLine}");
            sb.Append($"Non-Draftable:\t{Rares.NonDraftablesOwned}\t{Rares.NonDraftablesNeeded}{Environment.NewLine}{Environment.NewLine}");

            sb.Append($"Boosters{Environment.NewLine}".ToUpper());
            sb.Append($"------{Environment.NewLine}");
            sb.Append($"Owned:\t{BoosterPacksOwned}{Environment.NewLine}");


            return sb.ToString();
        }
    }

    public class UserRareCollection
    {
        private const int CARDS_PER_PLAYSET = 4;

        private readonly Dictionary<string, int> _draftableCollection = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _nondraftableCollection = new Dictionary<string, int>();

        public UserRareCollection(ILookup<bool, Card> rareCards)
        {
            _draftableCollection = rareCards[false].Select(c => c.Name).ToDictionary(c => c, c => 0);
            _nondraftableCollection = rareCards[true].Select(c => c.Name).ToDictionary(c => c, c => 0);
        }

        public UserRareCollection(List<string> draftableCardNames, List<string> nonDraftableCardNames)
        {
            _draftableCollection = draftableCardNames.ToDictionary(kvp => kvp, kvp => 0);
            _nondraftableCollection = nonDraftableCardNames.ToDictionary(kvp => kvp, kvp => 0);
        }

        public int NonDraftablesOwned { get => _nondraftableCollection.Sum(kvp => kvp.Value); }
        public int DraftablesOwned { get => _draftableCollection.Sum(kvp => kvp.Value); }
        public int DraftablesNeeded { get => _draftableCollection.Count * CARDS_PER_PLAYSET - DraftablesOwned; }
        public int NonDraftablesNeeded { get => _nondraftableCollection.Count * CARDS_PER_PLAYSET - NonDraftablesOwned; }

        public List<string> GetCardNames(bool draftable) =>
            draftable
                ? _draftableCollection.Select(kvp => kvp.Key).ToList()
                : _nondraftableCollection.Select(kvp => kvp.Key).ToList();

        public bool IsCompletePlayset(string cardName) => _draftableCollection[cardName] == CARDS_PER_PLAYSET;

        public int AddCard(string cardName, bool draftable) => draftable ? addDraftable(cardName) : addNonDraftable(cardName);

        private int addDraftable(string cardName)
        {
            _draftableCollection[cardName]++;
            return _draftableCollection[cardName];
        }

        private int addNonDraftable(string cardName)
        {
            _nondraftableCollection[cardName]++;
            return _nondraftableCollection[cardName];
        }

        public IEnumerable<string> CollectedDraftables => _draftableCollection.Where(kvp => kvp.Value != 0).Select(kvp => kvp.Key);
        public IEnumerable<string> CollectedNonDraftables => _nondraftableCollection.Where(kvp => kvp.Value != 0).Select(kvp => kvp.Key);
    }

    public class UserWildcardCollection
    {
        public int Owned { get; set; }
        public int Progress { get; set; }
    }
}
