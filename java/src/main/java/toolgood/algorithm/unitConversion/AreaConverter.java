package toolgood.algorithm.unitConversion;

import java.math.BigDecimal;

public class AreaConverter extends BaseUnitConverter {
    private static final UnitFactors units = new UnitFactors();

    static {
        units.put(new UnitFactorSynonyms("m²", "m2", "square metre", "square meter", "centiare", "平方米", "平方公尺"), new BigDecimal("1"));
        units.put(new UnitFactorSynonyms("km²", "km2", "square kilometre", "square kilometer", "平方千米"), new BigDecimal("0.000001"));
        units.put(new UnitFactorSynonyms("dm²", "dm2", "square decimetre", "square decimeter", "平方分米"), new BigDecimal("100"));
        units.put(new UnitFactorSynonyms("cm²", "cm2", "square centimetre", "square centimeter", "平方厘米"), new BigDecimal("10000"));
        units.put(new UnitFactorSynonyms("mm²", "mm2", "square millimetre", "square millimeter", "平方毫米"), new BigDecimal("1000000"));
        units.put(new UnitFactorSynonyms("ft²", "ft2", "square foot", "square feet", "sq ft", "平方英尺"), new BigDecimal("1").divide(new BigDecimal("0.3048").multiply(new BigDecimal("0.3048"))));
        units.put(new UnitFactorSynonyms("yd²", "yd2", "sq yd", "square yard", "平方码"), new BigDecimal("1").divide(new BigDecimal("0.9144").multiply(new BigDecimal("0.9144"))));
        units.put(new UnitFactorSynonyms("a", "are"), new BigDecimal("0.01"));
        units.put(new UnitFactorSynonyms("ha", "hectare", "公顷"), new BigDecimal("0.0001"));
        units.put(new UnitFactorSynonyms("in²", "in2", "sq in", "square inch", "平方英寸"), new BigDecimal("1").divide(new BigDecimal("0.00064516")));
        units.put(new UnitFactorSynonyms("mi²", "mi2", "sq mi", "square mile", "平方英里"), new BigDecimal("1").divide(new BigDecimal("2589988.110336")));
        units.put(new UnitFactorSynonyms("亩"), new BigDecimal("1").divide(new BigDecimal("666.667")));
    }

    public AreaConverter(String leftUnit, String rightUnit) {
        Instantiate(units, leftUnit, rightUnit);
    }

    public static boolean Exists(String leftSynonym, String rightSynonym) {
        if (units.FindUnit(leftSynonym) != null) {
            return units.FindUnit(rightSynonym) != null;
        }
        return false;
    }
}
