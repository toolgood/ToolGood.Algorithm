using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.UnitConversion
{
    sealed class UnitFactorSynonyms
    {
        public UnitFactorSynonyms(params string[] items)
        {
            _synonyms.AddRange(items);
        }

        private List<string> _synonyms = new List<string>();

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
            return String.Join(", ", _synonyms);
        }

        // Allow strings to be interpreted as a UnitDictionaryKey
        public static implicit operator UnitFactorSynonyms(string synonym)
        {
            return new UnitFactorSynonyms(synonym);
        }
    }

}
