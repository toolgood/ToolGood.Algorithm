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

        BigDecimal ft3 = BigDecimal.ONE.divide(new BigDecimal("28.316846592"), 30, RoundingMode.HALF_UP);
        map.put("ft³", ft3);
        map.put("ft3", ft3);
        map.put("cubic foot", ft3);
        map.put("cubic feet", ft3);
        map.put("立方英尺", ft3);
        map.put("cu ft", ft3);

        BigDecimal in3 = new BigDecimal("125000000").divide(new BigDecimal("2048383"), 30, RoundingMode.HALF_UP);
        map.put("in³", in3);
        map.put("in3", in3);
        map.put("cubic in", in3);
        map.put("cubic inch", in3);
        map.put("立方英寸", in3);

        BigDecimal impPt = new BigDecimal("800000").divide(new BigDecimal("454609"), 30, RoundingMode.HALF_UP);
        map.put("imperial pint", impPt);
        map.put("imperial pt", impPt);
        map.put("imperial p", impPt);

        BigDecimal impGal = new BigDecimal("100000").divide(new BigDecimal("454609"), 30, RoundingMode.HALF_UP);
        map.put("imperial gallon", impGal);
        map.put("imperial gal", impGal);

        BigDecimal impQt = new BigDecimal("400000").divide(new BigDecimal("454609"), 30, RoundingMode.HALF_UP);
        map.put("imperial quart", impQt);
        map.put("imperial qt", impQt);

        BigDecimal usPt = new BigDecimal("1000000000").divide(new BigDecimal("473176473"), 30, RoundingMode.HALF_UP);
        map.put("US pint", usPt);
        map.put("US pt", usPt);
        map.put("US p", usPt);

        BigDecimal usGal = new BigDecimal("125000000").divide(new BigDecimal("473176473"), 30, RoundingMode.HALF_UP);
        map.put("US gallon", usGal);
        map.put("US gal", usGal);

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
