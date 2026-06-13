package toolgood.algorithm.unitConversion;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.Map;
import java.util.TreeMap;

public final class VolumeConverter {
    private static final Map<String, BigDecimal> units2;

    static {
        TreeMap<String, BigDecimal> map = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        map.put("l", BigDecimal.ONE);
        map.put("lt", BigDecimal.ONE);
        map.put("ltr", BigDecimal.ONE);
        map.put("liter", BigDecimal.ONE);
        map.put("litre", BigDecimal.ONE);
        map.put("dm³", BigDecimal.ONE);
        map.put("dm3", BigDecimal.ONE);
        map.put("cubic decimetre", BigDecimal.ONE);
        map.put("cubic decimeter", BigDecimal.ONE);
        map.put("升", BigDecimal.ONE);
        map.put("立方分米", BigDecimal.ONE);

        map.put("m³", new BigDecimal("0.001"));
        map.put("m3", new BigDecimal("0.001"));
        map.put("cubic metre", new BigDecimal("0.001"));
        map.put("cubic meter", new BigDecimal("0.001"));
        map.put("立方米", new BigDecimal("0.001"));

        BigDecimal km3 = new BigDecimal("0.001").multiply(new BigDecimal("0.001"))
                .multiply(new BigDecimal("0.001")).multiply(new BigDecimal("0.001"));
        map.put("km³", km3);
        map.put("km3", km3);
        map.put("cubic kilometre", km3);
        map.put("cubic kilometer", km3);
        map.put("立方千米", km3);

        map.put("cm³", new BigDecimal("1000"));
        map.put("cm3", new BigDecimal("1000"));
        map.put("cubic centimetre", new BigDecimal("1000"));
        map.put("cubic centimeter", new BigDecimal("1000"));
        map.put("立方厘米", new BigDecimal("1000"));
        map.put("毫升", new BigDecimal("1000"));
        map.put("ml", new BigDecimal("1000"));

        map.put("mm³", new BigDecimal("1000000"));
        map.put("mm3", new BigDecimal("1000000"));
        map.put("cubic millimetre", new BigDecimal("1000000"));
        map.put("cubic millimeter", new BigDecimal("1000000"));
        map.put("立方毫米", new BigDecimal("1000000"));

        map.put("ft³", new BigDecimal("0.0353147"));
        map.put("ft3", new BigDecimal("0.0353147"));
        map.put("cubic foot", new BigDecimal("0.0353147"));
        map.put("cubic feet", new BigDecimal("0.0353147"));
        map.put("立方英尺", new BigDecimal("0.0353147"));
        map.put("cu ft", new BigDecimal("0.0353147"));

        map.put("in³", new BigDecimal("61.0237"));
        map.put("in3", new BigDecimal("61.0237"));
        map.put("cubic in", new BigDecimal("61.0237"));
        map.put("cubic inch", new BigDecimal("61.0237"));
        map.put("立方英寸", new BigDecimal("61.0237"));

        map.put("imperial pint", new BigDecimal("1.75975"));
        map.put("imperial pt", new BigDecimal("1.75975"));
        map.put("imperial p", new BigDecimal("1.75975"));

        map.put("imperial gallon", new BigDecimal("0.219969"));
        map.put("imperial gal", new BigDecimal("0.219969"));

        map.put("imperial quart", new BigDecimal("0.879877"));
        map.put("imperial qt", new BigDecimal("0.879877"));

        map.put("US pint", new BigDecimal("2.11337643513819"));
        map.put("US pt", new BigDecimal("2.11337643513819"));
        map.put("US p", new BigDecimal("2.11337643513819"));

        map.put("US gallon", new BigDecimal("0.264172"));
        map.put("US gal", new BigDecimal("0.264172"));

        BigDecimal usqt = new BigDecimal("1000000000").divide(new BigDecimal("946352946"), 30, RoundingMode.HALF_UP);
        map.put("US quart", usqt);
        map.put("US qt", usqt);

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
