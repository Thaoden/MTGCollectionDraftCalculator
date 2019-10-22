using System;
using System.Collections.Generic;
using System.Text;

namespace MTGDraftCollectionCalculator
{
    internal class Rarity : IEquatable<Rarity>
    {
        internal static Rarity Rare = new Rarity("Rare");

        private readonly string _rarity;

        private Rarity(string rarity)
        {
            _rarity = rarity;
        }

        #region IEquatableImplementation

        public override bool Equals(object? obj)
        {
            if (obj is Rarity r)
            {
                return Equals(r);
            }

            return false;
        }

        public override int GetHashCode() => _rarity.GetHashCode();

        public bool Equals(Rarity other) => _rarity.Equals(other._rarity, StringComparison.CurrentCultureIgnoreCase);

        public static bool operator ==(Rarity me, Rarity other) => me.Equals(other);

        public static bool operator !=(Rarity me, Rarity other) => !me.Equals(other);

        public static bool operator ==(string me, Rarity other) => (new Rarity(me)).Equals(other);

        public static bool operator !=(string me, Rarity other) => !(new Rarity(me)).Equals(other);

        #endregion
    }
}
