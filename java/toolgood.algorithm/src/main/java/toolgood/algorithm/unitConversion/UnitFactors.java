package toolgood.algorithm.unitConversion;

import java.math.BigDecimal;
import java.util.TreeMap;

public class UnitFactors extends TreeMap<UnitFactorSynonyms, BigDecimal> {

    public UnitFactorSynonyms FindUnit(String synonyms) {
        return FindUnit(new UnitFactorSynonyms(synonyms));
    }

    public UnitFactorSynonyms FindUnit(UnitFactorSynonyms synonyms) {
        for (UnitFactorSynonyms factor : this.keySet()) {
            if (factor.Contains(synonyms)) {
                return factor;
            }
        }
        return null;
    }

    public BigDecimal FindFactor(String synonyms) throws UnitNotSupportedException {
        return FindFactor(new UnitFactorSynonyms(synonyms));
    }

    // Get the factor for a given unit
    public BigDecimal FindFactor(UnitFactorSynonyms synonyms) throws UnitNotSupportedException {
        for (UnitFactorSynonyms factor : this.keySet()) {
            if (factor.Contains(synonyms)) {
                return this.get(factor);
            }
        }
        throw new UnitNotSupportedException(synonyms.toString());
    }


}
