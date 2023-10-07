package toolgood.algorithm.unitConversion;


import java.math.BigDecimal;

public class AreaConverter extends BaseUnitConverter {
    static UnitFactors units;

    static {
        units = new UnitFactors();
        units.put(new UnitFactorSynonyms(new String[]{"m²", "m2", "square metre","square meter", "centiare", "平方米", "平方公尺"}), new BigDecimal(1));
        units.put(new UnitFactorSynonyms(new String[]{"km²", "km2", "square kilometre","square kilometer", "平方千米"}), new BigDecimal("0.000001"));
        units.put(new UnitFactorSynonyms(new String[]{"dm²", "dm2", "square centimetre", "square centimeter","平方分米"}), new BigDecimal("100"));
        units.put(new UnitFactorSynonyms(new String[]{"cm²", "cm2", "square centimetre","square centimeter",  "平方厘米"}), new BigDecimal("10000"));
        units.put(new UnitFactorSynonyms(new String[]{"mm²", "mm2", "square millimetre",  "square millimeter", "平方毫米"}), new BigDecimal("1000000"));
        units.put(new UnitFactorSynonyms(new String[]{"ft²", "ft2", "square foot", "square feet", "sq ft", "平方英尺"}), BigDecimal.valueOf(1d / 0.3048d / 0.3048d));
        units.put(new UnitFactorSynonyms(new String[]{"yd²", "yd2", "sq yd", "square yard", "平方码"}), BigDecimal.valueOf(1d / 0.9144d / 0.9144d));
        units.put(new UnitFactorSynonyms(new String[]{"a", "are"}), new BigDecimal("0.01"));
        units.put(new UnitFactorSynonyms(new String[]{"ha", "hectare", "公顷"}), new BigDecimal("0.0001"));
        units.put(new UnitFactorSynonyms(new String[]{"in²", "in2", "sq in", "square inch", "平方英寸"}), BigDecimal.valueOf(1d / 0.00064516d));
        units.put(new UnitFactorSynonyms(new String[]{"mi²", "mi2", "sq mi", "square mile", "平方英里"}), BigDecimal.valueOf(1d / 2589988.110336d));
        units.put(new UnitFactorSynonyms(new String[]{"亩"}), BigDecimal.valueOf(1d / 666.667d));
    }


    public AreaConverter(String leftUnit, String rightUnit) {
        Instantiate(units, leftUnit, rightUnit);
    }

    public static Boolean Exists(String leftSynonym, String rightSynonym) {
        if (units.FindUnit(leftSynonym) != null) {
            return units.FindUnit(rightSynonym) != null;
        }
        return false;
    }

}
