package toolgood.algorithm.unitConversion;

import java.math.BigDecimal;

public class VolumeConverter extends BaseUnitConverter {
    private static final UnitFactors units = new UnitFactors();

    static {
        units.put(new UnitFactorSynonyms("l", "L", "lt", "ltr", "liter", "litre", "dm\u00b3", "dm3", "cubic decimetre", "cubic decimeter", "\u5347", "\u7acb\u65b9\u5206"), new BigDecimal("1"));
        units.put(new UnitFactorSynonyms("m\u00b3", "m3", "cubic metre", "cubic meter", "\u7acb\u65b9\u7c73"), new BigDecimal("0.001"));
        units.put(new UnitFactorSynonyms("km\u00b3", "km3", "cubic kilometre", "cubic kilometer", "\u7acb\u65b9\u5343\u7c73"), new BigDecimal("0.000000001"));
        units.put(new UnitFactorSynonyms("cm\u00b3", "cm3", "cubic centimetre", "cubic centimeter", "\u7acb\u65b9\u5398\u7c73", "\u6beb\u5347"), new BigDecimal("1000"));
        units.put(new UnitFactorSynonyms("mm\u00b3", "mm3", "cubic millimetre", "cubic millimeter", "\u7acb\u65b9\u6beb\u7c73"), new BigDecimal("1000000"));
        units.put(new UnitFactorSynonyms("ft\u00b3", "ft3", "cubic foot", "cubic feet", "cu ft", "\u7acb\u65b9\u82f1\u5c3a"), new BigDecimal("0.0353147"));
        units.put(new UnitFactorSynonyms("in\u00b3", "in3", "cu in", "cubic inch", "\u7acb\u65b9\u82f1\u5bf8"), new BigDecimal("61.0237"));
        units.put(new UnitFactorSynonyms("imperial pint", "imperial pt", "imperial p"), new BigDecimal("1.75975"));
        units.put(new UnitFactorSynonyms("imperial gallon", "imperial gal"), new BigDecimal("0.219969"));
        units.put(new UnitFactorSynonyms("imperial quart", "imperial qt"), new BigDecimal("0.879877"));
        units.put(new UnitFactorSynonyms("US pint", "US pt", "US p"), new BigDecimal("2.11337643513819"));
        units.put(new UnitFactorSynonyms("US gallon", "US gal"), new BigDecimal("0.264172"));
        units.put(new UnitFactorSynonyms("US quart", "US qt"), new BigDecimal("2.11338"));
    }

    public VolumeConverter(String leftUnit, String rightUnit) {
        Instantiate(units, leftUnit, rightUnit);
    }

    public static boolean Exists(String leftSynonym, String rightSynonym) {
        if (units.FindUnit(leftSynonym) != null) {
            return units.FindUnit(rightSynonym) != null;
        }
        return false;
    }
}
