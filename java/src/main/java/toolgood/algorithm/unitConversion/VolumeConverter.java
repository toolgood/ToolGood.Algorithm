package toolgood.algorithm.unitConversion;


import java.math.BigDecimal;

public class VolumeConverter extends BaseUnitConverter {
    static UnitFactors units;

    static {
        units = new UnitFactors();
        units.put(new UnitFactorSynonyms(new String[]{"l", "L", "lt", "ltr", "liter", "litre", "dm³", "dm3", "cubic decimetre","cubic decimeter", "�?, "立方分米"}), new BigDecimal("1"));
        units.put(new UnitFactorSynonyms(new String[]{"m³", "m3", "cubic metre","cubic meter", "立方�?}), new BigDecimal("0.001"));
        units.put(new UnitFactorSynonyms(new String[]{"km³", "km3", "cubic kilometre","cubic kilometer", "立方千米"}), new BigDecimal("0.000000000001"));
        units.put(new UnitFactorSynonyms(new String[]{"cm³", "cm3", "cubic centimetre","cubic centimeter", "立方厘米", "毫升"}), new BigDecimal("1000"));
        units.put(new UnitFactorSynonyms(new String[]{"mm³", "mm3", "cubic millimetre","cubic millimeter", "立方毫米"}), new BigDecimal("1000000"));
        units.put(new UnitFactorSynonyms(new String[]{"ft³", "ft3", "cubic foot", "cubic feet", "cu ft", "立方英尺"}),new BigDecimal("0.0353147"));
        units.put(new UnitFactorSynonyms(new String[]{"in³", "in3", "cu in", "cubic inch", "立方英寸"}), new BigDecimal("61.0237"));
        units.put(new UnitFactorSynonyms(new String[]{"imperial pint", "imperial pt", "imperial p"}), new BigDecimal("1.75975"));
        units.put(new UnitFactorSynonyms(new String[]{"imperial gallon", "imperial gal"}), new BigDecimal("0.219969"));
        units.put(new UnitFactorSynonyms(new String[]{"imperial quart", "imperial qt"}),new BigDecimal("0.879877"));
        units.put(new UnitFactorSynonyms(new String[]{"US pint", "US pt", "US p"}), new BigDecimal("2.11337643513819"));
        units.put(new UnitFactorSynonyms(new String[]{"US gallon", "US gal"}), new BigDecimal("0.264172"));
        units.put(new UnitFactorSynonyms(new String[]{"US quart", "US qt"}),new BigDecimal("2.11338"));
    }


    public VolumeConverter(String leftUnit, String rightUnit) {
        Instantiate(units, leftUnit, rightUnit);
    }

    public static Boolean Exists(String leftSynonym, String rightSynonym) {
        if (units.FindUnit(leftSynonym) != null) {
            return units.FindUnit(rightSynonym) != null;
        }
        return false;
    }

}
