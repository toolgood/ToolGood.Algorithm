using System;
using System.Linq;

namespace ToolGood.Algorithm.UnitConversion
{
    internal sealed class UnitFactorSynonyms
    {
		private string[] _synonyms;
		public UnitFactorSynonyms(params string[] items)
        {
            _synonyms = items;
        }


        // Find if some synonym of a given UnitFactor is included in this UnitFactor
        internal bool Contains(UnitFactorSynonyms synonyms)
        {
            foreach (var syn in synonyms._synonyms) {
                if (this.Contains(syn)) {
                    return true;
                }
            }
            return false;
        }

        // Find if some synonym is included in this UnitFactor
        internal bool Contains(string synonym)
        {
            return _synonyms.Contains(synonym, StringComparer.CurrentCultureIgnoreCase);
        }

        public override string ToString()
        {
            return string.Join(", ", _synonyms);
        }

        // Allow strings to be interpreted as a UnitDictionaryKey
        public static implicit operator UnitFactorSynonyms(string synonym)
        {
            return new UnitFactorSynonyms(synonym);
        }
    }
}