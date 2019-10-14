using System;
using System.Collections.Generic;
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
            Rares.DraftablesOwned = sourceCollection.Rares.DraftablesOwned;
            Rares.NonDraftablesOwned = sourceCollection.Rares.NonDraftablesOwned;

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
        public int NonDraftablesOwned { get; set; }
        public int DraftablesOwned { get; set; }
        public int DraftablesNeeded { get => SetCollection.Eldraine.Rares.Draftable - DraftablesOwned; }
        public int NonDraftablesNeeded { get => SetCollection.Eldraine.Rares.NonDraftable - NonDraftablesOwned; }
    }

    public class UserWildcardCollection
    {
        public int Owned { get; set; }
        public int Progress { get; set; }
    }
}
