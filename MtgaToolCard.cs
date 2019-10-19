using System;
using System.Collections.Generic;
using System.Text;

namespace MTGDraftCollectionCalculator.MtgaTool
{
    internal class MtgaToolCard
    {
        internal string Name { get; set; } = String.Empty;
        internal string SetCode { get; set; } = String.Empty;
        internal string SetName { get; set; } = String.Empty;
        internal string Rarity { get; set; } = String.Empty;
        internal int Count { get; set; }
    }
}
