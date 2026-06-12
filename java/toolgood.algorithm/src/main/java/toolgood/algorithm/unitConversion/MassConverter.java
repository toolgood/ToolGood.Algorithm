package toolgood.algorithm.unitConversion;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.Map;
import java.util.TreeMap;

final class MassConverter {
    private static final Map<String, BigDecimal> units2;

    static {
        TreeMap<String, BigDecimal> map = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        map.put("kg", BigDecimal.ONE);
        map.put("kilogram", BigDecimal.ONE);
        map.put("千克", BigDecimal.ONE);

        map.put("gram", new BigDecimal("1000"));
        map.put("g", new BigDecimal("1000"));
        map.put("克", new BigDecimal("1000"));

        BigDecimal ton = BigDecimal.ONE.divide(new BigDecimal("1000"), 30, RoundingMode.HALF_UP);
        map.put("ton", ton);
        map.put("t", ton);
        map.put("吨", ton);

        BigDecimal lb = new BigDecimal("100000000").divide(new BigDecimal("45359237"), 30, RoundingMode.HALF_UP);
        map.put("lb", lb);
        map.put("lbs", lb);
        map.put("pound", lb);
        map.put("pounds", lb);
        map.put("英镑", lb);

        BigDecimal st = new BigDecimal("50000000").divide(new BigDecimal("317514659"), 30, RoundingMode.HALF_UP);
        map.put("st", st);
        map.put("stone", st);
        map.put("石", st);

        BigDecimal oz = new BigDecimal("1600000000").divide(new BigDecimal("45359237"), 30, RoundingMode.HALF_UP);
        map.put("oz", oz);
        map.put("ounce", oz);
        map.put("盎司", oz);

        map.put("quintal", new BigDecimal("0.01"));
        map.put("英担", new BigDecimal("0.01"));

        map.put("short ton", new BigDecimal("0.00110231"));
        map.put("net ton", new BigDecimal("0.00110231"));
        map.put("us ton", new BigDecimal("0.00110231"));
        map.put("短吨", new BigDecimal("0.00110231"));
        map.put("美吨", new BigDecimal("0.00110231"));

        map.put("long ton", new BigDecimal("0.000984207"));
        map.put("weight ton", new BigDecimal("0.000984207"));
        map.put("gross ton", new BigDecimal("0.000984207"));
        map.put("imperial ton", new BigDecimal("0.000984207"));
        map.put("长吨", new BigDecimal("0.000984207"));
        map.put("英吨", new BigDecimal("0.000984207"));

        units2 = map;
    }

    public static BigDecimal TryConvert(String leftSynonym, String rightSynonym, BigDecimal left) {
        BigDecimal l = units2.get(leftSynonym);
        if (l != null) {
            BigDecimal r = units2.get(rightSynonym);
            if (r != null) {
                return left.divide(l, 30, RoundingMode.HALF_UP).multiply(r);
            }
        }
        return null;
    }
}
