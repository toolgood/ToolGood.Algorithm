package toolgood.algorithm.unitConversion;

import java.math.BigDecimal;

public class DistanceConverter extends BaseUnitConverter {
    private static final UnitFactors units = new UnitFactors();

    static {
        units.put(new UnitFactorSynonyms("m", "metre", "\u7c73"), new BigDecimal("1"));
        units.put(new UnitFactorSynonyms("km", "kilometre", "\u5343\u7c73"), new BigDecimal("0.001"));
        units.put(new UnitFactorSynonyms("dm", "decimetre", "\u5206\u7c73"), new BigDecimal("10"));
        units.put(new UnitFactorSynonyms("cm", "centimetre", "\u5398\u7c73"), new BigDecimal("100"));
        units.put(new UnitFactorSynonyms("mm", "millimetre", "\u6beb\u7c73"), new BigDecimal("1000"));
        units.put(new UnitFactorSynonyms("ft", "foot", "feet", "\u82f1\u5c3a"), new BigDecimal("1250").divide(new BigDecimal("381")));
        units.put(new UnitFactorSynonyms("yd", "yard", "\u7801"), new BigDecimal("1250").divide(new BigDecimal("1143")));
        units.put(new UnitFactorSynonyms("mile", "\u82f1\u91cc"), new BigDecimal("125").divide(new BigDecimal("201168")));
        units.put(new UnitFactorSynonyms("in", "inch", "\u82f1\u5bf8"), new BigDecimal("5000").divide(new BigDecimal("127")));
        units.put(new UnitFactorSynonyms("au"), new BigDecimal("1").divide(new BigDecimal("149600000000")));
    }

    public DistanceConverter(String leftUnit, String rightUnit) {
        Instantiate(units, leftUnit, rightUnit);
    }

    public static boolean Exists(String leftSynonym, String rightSynonym) {
        if (units.FindUnit(leftSynonym) != null) {
            return units.FindUnit(rightSynonym) != null;
        }
        return false;
    }
}
