package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;

import static org.junit.Assert.assertEquals;

public class MathSumTest {

    // 简单统计
    @Test
    public void MAX_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("max(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(4.0, t, 0.0001);
    }

    @Test
    public void MEDIAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MEDIAN(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(2.0, t, 0.0001);

        t = engine.TryEvaluate("MEDIAN(1,1,2,2)", 0.0);
        assertEquals(1.5, t, 0.0001);
    }

    @Test
    public void MIN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MIN(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(1.0, t, 0.0001);
    }

    @Test
    public void QUARTILE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("QUARTILE(ARRAY(1,2,3,4,2,2,1,4),0)", 0.0);
        assertEquals(1.0, t, 0.0001);
        t = engine.TryEvaluate("QUARTILE(ARRAY(1,2,3,4,2,2,1,4),1)", 0.0);
        assertEquals(1.75, t, 0.0001);
        t = engine.TryEvaluate("QUARTILE(ARRAY(1,2,3,4,2,2,1,4),2)", 0.0);
        assertEquals(2.0, t, 0.0001);
        t = engine.TryEvaluate("QUARTILE(ARRAY(1,2,3,4,2,2,1,4),3)", 0.0);
        assertEquals(3.25, t, 0.0001);
        t = engine.TryEvaluate("QUARTILE(ARRAY(1,2,3,4,2,2,1,4),4)", 0.0);
        assertEquals(4.0, t, 0.0001);
    }

    @Test
    public void MODE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MODE(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(2.0, t, 0.0001);
    }

    @Test
    public void PERCENTILE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PERCENTILE(ARRAY(1,2,3,4,2,2,1,4),0.4)", 0.0);
        assertEquals(2.0, t, 0.0001);
    }

    @Test
    public void PERCENTRANK_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PERCENTRANK(ARRAY(1,2,3,4,2,2,1,4),3)", 0.0);
        assertEquals(0.714, t, 0.001);
    }

    @Test
    public void AVERAGE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("AVERAGE(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(2.375, t, 0.0001);
    }

    @Test
    public void GEOMEAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GEOMEAN(1,2,3,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(2.213363839 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void HARMEAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("HARMEAN(1,2,3,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.92 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void COUNT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COUNT(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(8.0, t, 0.0001);
    }

    @Test
    public void AVEDEV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("AVEDEV(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(0.96875, t, 0.0001);
    }

    @Test
    public void STDEV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("STDEV(1,2,3,4,2,2,1,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.187734939 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void STDEVP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("STDEVP(1,2,3,4,2,2,1,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.111024302 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void DEVSQ_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("DEVSQ(1,2,3,4,2,2,1,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(9.875 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void VAR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("VAR(1,2,3,4,2,2,1,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.410714286 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void VARP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("VARP(1,2,3,4,2,2,1,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.234375 * 1000000) / 1000000.0, rounded);
    }

    // 统计函数
    @Test
    public void RANK_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RANK(3, ARRAY(1, 2, 3, 4, 5))", 0.0);
        assertEquals(3, t, 0.0001);

        t = engine.TryEvaluate("RANK(3, ARRAY(1, 2, 3, 4, 5), 0)", 0.0);
        assertEquals(3, t, 0.0001);

        t = engine.TryEvaluate("RANK(3, ARRAY(1, 2, 3, 4, 5), 1)", 0.0);
        assertEquals(3, t, 0.0001);

        t = engine.TryEvaluate("RANK(5, ARRAY(1, 2, 3, 4, 5))", 0.0);
        assertEquals(1, t, 0.0001);

        t = engine.TryEvaluate("RANK(1, ARRAY(1, 2, 3, 4, 5))", 0.0);
        assertEquals(5, t, 0.0001);
    }

    @Test
    public void FORECAST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FORECAST(30, ARRAY(6, 7, 9, 15, 21), ARRAY(20, 28, 31, 38, 40))", 0.0);
        double rounded = Math.round(t * 10000) / 10000.0;
        assertEquals(Math.round(10.6073 * 10000) / 10000.0, rounded);
    }

    @Test
    public void INTERCEPT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("INTERCEPT(ARRAY(2, 3, 9, 1, 8), ARRAY(6, 5, 11, 7, 5))", 0.0);
        double rounded = Math.round(t * 10000) / 10000.0;
        assertEquals(Math.round(0.0484 * 10000) / 10000.0, rounded);
    }

    @Test
    public void SLOPE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SLOPE(ARRAY(2, 3, 9, 1, 8), ARRAY(6, 5, 11, 7, 5))", 0.0);
        double rounded = Math.round(t * 10000) / 10000.0;
        assertEquals(Math.round(0.6694 * 10000) / 10000.0, rounded);
    }

    @Test
    public void CORREL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("CORREL(ARRAY(3, 2, 4, 5, 6), ARRAY(9, 7, 12, 15, 17))", 0.0);
        double rounded = Math.round(t * 10000) / 10000.0;
        assertEquals(Math.round(0.9971 * 10000) / 10000.0, rounded);
    }

    @Test
    public void PEARSON_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PEARSON(ARRAY(3, 2, 4, 5, 6), ARRAY(9, 7, 12, 15, 17))", 0.0);
        double rounded = Math.round(t * 10000) / 10000.0;
        assertEquals(Math.round(0.9971 * 10000) / 10000.0, rounded);
    }

    @Test
    public void LARGE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LARGE(ARRAY(1,2,3,4,2,2,1,4),3)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(3.0 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void SMALL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SMALL(ARRAY(1,2,3,4,2,2,1,4),3)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(2.0 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void COVAR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COVAR(ARRAY(3,7,6,11),ARRAY(5,15,13,9))", 0.0);
        double t1 = engine.TryEvaluate("COVARIANCE.P(ARRAY(3,7,6,11),ARRAY(5,15,13,9))", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        double rounded1 = Math.round(t1 * 1000000) / 1000000.0;
        assertEquals(Math.round(3.375 * 1000000) / 1000000.0, rounded);
        assertEquals(Math.round(3.375 * 1000000) / 1000000.0, rounded1);
    }

    @Test
    public void COVARIANCES_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COVARIANCE.S(ARRAY(3,7,6,11),ARRAY(5,15,13,9))", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(4.5 * 1000000) / 1000000.0, rounded);
    }

    // 条件统计
    @Test
    public void COUNTIF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COUNTIF(ARRAY(1,2,3,4,2,2,1,4),'>1')", 0.0);
        assertEquals(t, 6.0);
        t = engine.TryEvaluate("COUNTIF(ARRAY(1,2,3,4,2,2,1,4),'>=1')", 0.0);
        assertEquals(t, 8.0);
        t = engine.TryEvaluate("COUNTIF(ARRAY(1,2,3,4,2,2,1,4),'!=1')", 0.0);
        assertEquals(t, 6.0);
        t = engine.TryEvaluate("COUNTIF(ARRAY(1,2,3,4,2,2,1,4),'=1')", 0.0);
        assertEquals(t, 2.0);
        t = engine.TryEvaluate("COUNTIF(ARRAY(1,2,3,4,2,2,1,4),'<=1')", 0.0);
        assertEquals(t, 2.0);
        t = engine.TryEvaluate("COUNTIF(ARRAY(1,2,3,4,2,2,1,4),'<2')", 0.0);
        assertEquals(t, 2.0);
    }

    @Test
    public void SUMIF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMIF(ARRAY(1,2,3,4,2,2,1,4),'>1')", 0.0);
        assertEquals(t, 17);
        t = engine.TryEvaluate("SUMIF(ARRAY(1,2,3,4,2,2,1,4),'>1',ARRAY(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(t, 6);
        t = engine.TryEvaluate("SUMIF(ARRAY(1,2,3,4,2,2,1,4),'>=2',ARRAY(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(t, 6);
        t = engine.TryEvaluate("SUMIF(ARRAY(1,2,3,4,2,2,1,4),'<1',ARRAY(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(t, 0);
        t = engine.TryEvaluate("SUMIF(ARRAY(1,2,3,4,2,2,1,4),'==1',ARRAY(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(t, 2);
    }

    @Test
    public void AVERAGEIF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("AVERAGEIF(ARRAY(1,2,3,4,2,2,1,4),'>1')", 0.0);
        assertEquals(t, 2.833333333, 6);

        t = engine.TryEvaluate("AVERAGEIF(ARRAY(1,2,3,4,2,2,1,4),'>1',ARRAY(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(t, 1);
    }

    // 数组运算
    @Test
    public void SERIESSUM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SERIESSUM(2, 0, 1, ARRAY(1, 1, 1, 1))", 0.0);
        assertEquals(t, 15.0);
    }

    @Test
    public void SUMPRODUCT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMPRODUCT(ARRAY(1, 2, 3), ARRAY(4, 5, 6))", 0.0);
        assertEquals(t, 32.0);
    }

    @Test
    public void SUMX2MY2_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMX2MY2(ARRAY(1, 2, 3), ARRAY(4, 5, 6))", 0.0);
        assertEquals(t, -63.0);
    }

    @Test
    public void SUMX2PY2_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMX2PY2(ARRAY(1, 2, 3), ARRAY(4, 5, 6))", 0.0);
        assertEquals(t, 91.0);
    }

    @Test
    public void SUMXMY2_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMXMY2(ARRAY(1, 2, 3), ARRAY(4, 5, 6))", 0.0);
        assertEquals(t, 27.0);
    }
}
