package toolgood.algorithm.unitConversion;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.Map;
import java.util.TreeMap;

public final class DistanceConverter {
    private static final Map<String, BigDecimal> units2;

    static {
        TreeMap<String, BigDecimal> map = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        map.put("m", BigDecimal.ONE);
        map.put("metre", BigDecimal.ONE);
        map.put("米", BigDecimal.ONE);

        map.put("km", new BigDecimal("0.001"));
        map.put("kilometre", new BigDecimal("0.001"));
        map.put("千米", new BigDecimal("0.001"));

        map.put("dm", new BigDecimal("10"));
        map.put("decimetre", new BigDecimal("10"));
        map.put("分米", new BigDecimal("10"));

        map.put("cm", new BigDecimal("100"));
        map.put("centimetre", new BigDecimal("100"));
        map.put("厘米", new BigDecimal("100"));

        map.put("mm", new BigDecimal("1000"));
        map.put("millimetre", new BigDecimal("1000"));
        map.put("毫米", new BigDecimal("1000"));

        BigDecimal ft = new BigDecimal("1250").divide(new BigDecimal("381"), 30, RoundingMode.HALF_UP);
        map.put("ft", ft);
        map.put("foot", ft);
        map.put("feet", ft);
        map.put("英尺", ft);

        BigDecimal yd = new BigDecimal("1250").divide(new BigDecimal("1143"), 30, RoundingMode.HALF_UP);
        map.put("yd", yd);
        map.put("yard", yd);
        map.put("码", yd);

        BigDecimal mile = new BigDecimal("125").divide(new BigDecimal("201168"), 30, RoundingMode.HALF_UP);
        map.put("mile", mile);
        map.put("英里", mile);

        BigDecimal inch = new BigDecimal("5000").divide(new BigDecimal("127"), 30, RoundingMode.HALF_UP);
        map.put("in", inch);
        map.put("inch", inch);
        map.put("英寸", inch);

        BigDecimal au = BigDecimal.ONE.divide(new BigDecimal("149600000000"), 30, RoundingMode.HALF_UP);
        map.put("au", au);

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
