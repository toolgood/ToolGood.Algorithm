using System.Collections.Generic;
using System.Linq;

namespace ToolGood.Algorithm.UnitConversion
{
    internal sealed class UnitFactors : Dictionary<UnitFactorSynonyms, decimal>
    {
        // Find the key or null for a given unit
        internal UnitFactorSynonyms FindUnit(UnitFactorSynonyms synonyms)
        {
            return this.Keys.FirstOrDefault(factor => factor.Contains(synonyms));
        }

        // Get the factor for a given unit
        internal decimal FindFactor(UnitFactorSynonyms synonyms)
        {
            var unit = this.FirstOrDefault(factor => factor.Key.Contains(synonyms));
            if (unit.Key == null) {
                throw new UnitNotSupportedException(synonyms.ToString());
            }
            return unit.Value;
        }
    }
}