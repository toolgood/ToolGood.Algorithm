package toolgood.algorithm.unitConversion;



import java.math.BigDecimal;

public class MassConverter extends BaseUnitConverter {

    static UnitFactors units;

    static {
        units = new UnitFactors();
        units.put(new UnitFactorSynonyms(new String[]{"kg", "kilogram", "千克"}), new BigDecimal("1"));
        units.put(new UnitFactorSynonyms(new String[]{"gram", "g", "克"}),  new BigDecimal("1000"));
        units.put(new UnitFactorSynonyms(new String[]{"吨", "t"}), BigDecimal.valueOf(1 / 1000d));
        units.put(new UnitFactorSynonyms(new String[]{"lb", "lbs", "pound", "pounds", "英镑"}), BigDecimal.valueOf(100000000d / 45359237));
        units.put(new UnitFactorSynonyms(new String[]{"st", "stone", "石"}), BigDecimal.valueOf(50000000d / 317514659));
        units.put(new UnitFactorSynonyms(new String[]{"oz", "ounce", "盎司"}), BigDecimal.valueOf(1600000000d / 45359237));
        units.put(new UnitFactorSynonyms(new String[]{"quintal", "英担"}),  new BigDecimal("0.01"));
        units.put(new UnitFactorSynonyms(new String[]{"short ton", "net ton", "us ton", "短吨", "美吨"}),  new BigDecimal("0.00110231"));
        units.put(new UnitFactorSynonyms(new String[]{"long ton", "weight ton", "gross ton", "imperial ton", "长吨", "英吨"}), new BigDecimal("0.000984207"));


    }

    public MassConverter(String leftUnit, String rightUnit) {
        Instantiate(units, leftUnit, rightUnit);
    }

    public static Boolean Exists(String leftSynonym, String rightSynonym) {
        if (units.FindUnit(leftSynonym) != null) {
            return units.FindUnit(rightSynonym) != null;
        }
        return false;
    }

}
