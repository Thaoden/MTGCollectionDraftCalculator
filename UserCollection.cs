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

        public UserCollection(List<string> rareDraftableSetCollectionCardNames)
        {
            Rares = new UserRareCollection(rareDraftableSetCollectionCardNames);
            BoosterPacksOwned = 0;
        }

        public UserCollection(UserCollection sourceCollection)
        {
            Rares = new UserRareCollection(sourceCollection.Rares.GetCardNames(draftable: true));

            foreach(var collected in sourceCollection.Rares.CollectedDraftables)
            {
                Rares.AddDraftable(collected);
            }

            foreach(var collected in sourceCollection.Rares.CollectedNonDraftables){
                Rares.AddNonDraftable(collected);
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
        private readonly int[] _nondraftableCollection = new int[SetCollection.Eldraine.Rares.NonDraftable];

        public UserRareCollection(List<string> rareDraftableSetCollectionCardNames)
        {
            _draftableCollection = rareDraftableSetCollectionCardNames.ToDictionary(kvp => kvp, kvp => 0);
        }

        public int NonDraftablesOwned { get => _nondraftableCollection.Sum(); }
        public int DraftablesOwned { get => _draftableCollection.Sum(kvp => kvp.Value); }
        public int DraftablesNeeded { get => SetCollection.Eldraine.Rares.Draftable - DraftablesOwned; }
        public int NonDraftablesNeeded { get => SetCollection.Eldraine.Rares.NonDraftable - NonDraftablesOwned; }

        public List<string> GetCardNames(bool draftable)
        {
            if (draftable)
            {
                return _draftableCollection.Select(kvp => kvp.Key).ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        public bool ContainsDraftable(string cardName) => _draftableCollection[cardName] != 0;
        public bool ContainsNonDraftable(int cardNumber) => _nondraftableCollection[cardNumber] != 0;

        public bool IsCompletePlayset(string cardName) => _draftableCollection[cardName] == CARDS_PER_PLAYSET;

        public int AddDraftable(string cardName)
        {
            _draftableCollection[cardName]++;
            return _draftableCollection[cardName];
        }

        public int AddNonDraftable(int cardNumber)
        {
            _nondraftableCollection[cardNumber]++;
            return _nondraftableCollection[cardNumber];
        }

        public IEnumerable<string> CollectedDraftables => _draftableCollection.Where(kvp => kvp.Value != 0).Select(kvp => kvp.Key);
        public IEnumerable<int> CollectedNonDraftables => _nondraftableCollection.Select((cd, idx) => new { cd, idx }).Where(x => x.cd != 0).Select(x => x.idx);
    }

    public class UserWildcardCollection
    {
        public int Owned { get; set; }
        public int Progress { get; set; }
    }
}
