package toolgood.algorithm.unitConversion;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.Map;
import java.util.TreeMap;

public final class AreaConverter {
    private static final Map<String, BigDecimal> units2;

    static {
        TreeMap<String, BigDecimal> map = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        map.put("m²", BigDecimal.ONE);
        map.put("m2", BigDecimal.ONE);
        map.put("square metre", BigDecimal.ONE);
        map.put("square meter", BigDecimal.ONE);
        map.put("centiare", BigDecimal.ONE);
        map.put("平方米", BigDecimal.ONE);
        map.put("平方公尺", BigDecimal.ONE);

        map.put("km²", new BigDecimal("0.000001"));
        map.put("km2", new BigDecimal("0.000001"));
        map.put("square kilometre", new BigDecimal("0.000001"));
        map.put("square kilometer", new BigDecimal("0.000001"));
        map.put("平方千米", new BigDecimal("0.000001"));

        map.put("dm²", new BigDecimal("100"));
        map.put("dm2", new BigDecimal("100"));
        map.put("square decimetre", new BigDecimal("100"));
        map.put("square decimeter", new BigDecimal("100"));
        map.put("平方分米", new BigDecimal("100"));

        map.put("cm²", new BigDecimal("10000"));
        map.put("cm2", new BigDecimal("10000"));
        map.put("square centimetre", new BigDecimal("10000"));
        map.put("square centimeter", new BigDecimal("10000"));
        map.put("平方厘米", new BigDecimal("10000"));

        map.put("mm²", new BigDecimal("1000000"));
        map.put("mm2", new BigDecimal("1000000"));
        map.put("square millimetre", new BigDecimal("1000000"));
        map.put("square millimeter", new BigDecimal("1000000"));
        map.put("平方毫米", new BigDecimal("1000000"));

        BigDecimal sqft = BigDecimal.ONE.divide(new BigDecimal("0.3048"), 30, RoundingMode.HALF_UP)
                .divide(new BigDecimal("0.3048"), 30, RoundingMode.HALF_UP);
        map.put("ft²", sqft);
        map.put("ft2", sqft);
        map.put("square foot", sqft);
        map.put("square feet", sqft);
        map.put("sq ft", sqft);
        map.put("平方英尺", sqft);

        BigDecimal sqyd = BigDecimal.ONE.divide(new BigDecimal("0.9144"), 30, RoundingMode.HALF_UP)
                .divide(new BigDecimal("0.9144"), 30, RoundingMode.HALF_UP);
        map.put("yd²", sqyd);
        map.put("yd2", sqyd);
        map.put("sq yd", sqyd);
        map.put("square yard", sqyd);
        map.put("平方码", sqyd);

        map.put("a", new BigDecimal("0.01"));
        map.put("are", new BigDecimal("0.01"));

        map.put("ha", new BigDecimal("0.0001"));
        map.put("hectare", new BigDecimal("0.0001"));
        map.put("公顷", new BigDecimal("0.0001"));

        BigDecimal sqin = BigDecimal.ONE.divide(new BigDecimal("0.00064516"), 30, RoundingMode.HALF_UP);
        map.put("in²", sqin);
        map.put("in2", sqin);
        map.put("sq in", sqin);
        map.put("square inch", sqin);
        map.put("平方英寸", sqin);

        BigDecimal sqmi = BigDecimal.ONE.divide(new BigDecimal("2589988.110336"), 30, RoundingMode.HALF_UP);
        map.put("mi²", sqmi);
        map.put("mi2", sqmi);
        map.put("sq mi", sqmi);
        map.put("square mile", sqmi);
        map.put("平方英里", sqmi);

        BigDecimal mu = BigDecimal.ONE.divide(new BigDecimal("666.667"), 30, RoundingMode.HALF_UP);
        map.put("亩", mu);

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
