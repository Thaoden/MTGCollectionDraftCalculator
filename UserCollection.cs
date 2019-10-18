using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGDraftCollectionCalculator
{
    public class UserCollection
    {
        public UserRareCollection Rares { get; } = new UserRareCollection();
        public UserWildcardCollection Wildcards { get; } = new UserWildcardCollection();

        public int BoosterPacksOwned { get; set; }

        public UserCollection()
        {
            BoosterPacksOwned = 0;
        }

        public UserCollection(UserCollection sourceCollection)
        {
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
        private readonly int[] _draftableCollection = new int[SetCollection.Eldraine.Rares.Draftable];
        private readonly int[] _nondraftableCollection = new int[SetCollection.Eldraine.Rares.NonDraftable];

        public int NonDraftablesOwned { get => _nondraftableCollection.Sum(); }
        public int DraftablesOwned { get => _draftableCollection.Sum(); }
        public int DraftablesNeeded { get => SetCollection.Eldraine.Rares.Draftable - DraftablesOwned; }
        public int NonDraftablesNeeded { get => SetCollection.Eldraine.Rares.NonDraftable - NonDraftablesOwned; }

        public bool ContainsDraftable(int cardNumber) => _draftableCollection[cardNumber] != 0;
        public bool ContainsNonDraftable(int cardNumber) => _nondraftableCollection[cardNumber] != 0;

        public int AddDraftable(int cardNumber)
        {
            _draftableCollection[cardNumber]++;
            return _draftableCollection[cardNumber];
        }

        public int AddNonDraftable(int cardNumber)
        {
            _nondraftableCollection[cardNumber]++;
            return _nondraftableCollection[cardNumber];
        }

        public IEnumerable<int> CollectedDraftables => _draftableCollection.Select((cd, idx) => new { cd, idx }).Where(x => x.cd != 0).Select(x => x.idx);
        public IEnumerable<int> CollectedNonDraftables => _nondraftableCollection.Select((cd, idx) => new { cd, idx }).Where(x => x.cd != 0).Select(x => x.idx);
    }

    public class UserWildcardCollection
    {
        public int Owned { get; set; }
        public int Progress { get; set; }
    }
}
