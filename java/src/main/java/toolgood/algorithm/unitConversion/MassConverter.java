package toolgood.algorithm.unitConversion;

import java.math.BigDecimal;

public class MassConverter extends BaseUnitConverter {
    private static final UnitFactors units = new UnitFactors();

    static {
        units.put(new UnitFactorSynonyms("kg", "kilogram", "\u5343\u514b"), new BigDecimal("1"));
        units.put(new UnitFactorSynonyms("gram", "g", "\u514b"), new BigDecimal("1000"));
        units.put(new UnitFactorSynonyms("ton", "t", "\u5428"), new BigDecimal("1").divide(new BigDecimal("1000")));
        units.put(new UnitFactorSynonyms("lb", "lbs", "pound", "pounds", "\u82f1\u92fc"), new BigDecimal("100000000").divide(new BigDecimal("45359237")));
        units.put(new UnitFactorSynonyms("st", "stone", "\u77f3"), new BigDecimal("50000000").divide(new BigDecimal("317514659")));
        units.put(new UnitFactorSynonyms("oz", "ounce", "\u5965\u53f8"), new BigDecimal("1600000000").divide(new BigDecimal("45359237")));
        units.put(new UnitFactorSynonyms("quintal", "\u82f1\u62c5"), new BigDecimal("0.01"));
        units.put(new UnitFactorSynonyms("short ton", "net ton", "us ton", "\u77ed\u5428", "\u7f8e\u5428"), new BigDecimal("0.00110231"));
        units.put(new UnitFactorSynonyms("long ton", "weight ton", "gross ton", "imperial ton", "\u9577\u5428", "\u82f1\u5428"), new BigDecimal("0.000984207"));
    }

    public MassConverter(String leftUnit, String rightUnit) {
        Instantiate(units, leftUnit, rightUnit);
    }

    public static boolean Exists(String leftSynonym, String rightSynonym) {
        if (units.FindUnit(leftSynonym) != null) {
            return units.FindUnit(rightSynonym) != null;
        }
        return false;
    }
}
