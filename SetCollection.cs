using System;
using System.Collections.Generic;
using System.Text;

namespace MTGDraftCollectionCalculator
{
    public class SetCollection
    {
        public static SetCollection Eldraine { get => new SetCollection("Eldraine"); }
        public SetRareCollection Rares { get; }
        public string Name { get; }

        private SetCollection(string collectionName)
        {
            Name = collectionName;
            Rares = new SetRareCollection
            {
                Total = 276,
                Draftable = 212
            };
        }
    }

    public class SetRareCollection
    {
        public int Total { get; set; }
        public int Draftable { get; set; }
        public int NonDraftable { get => Total - Draftable; }
    }
}
