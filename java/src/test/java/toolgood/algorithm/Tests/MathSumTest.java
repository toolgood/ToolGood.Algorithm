package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;

import static org.junit.Assert.assertEquals;

class MathSumTest {

    // 简单统计
    @Test
    void MAX_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("max(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(4.0, t, 0.0001);
    }

    @Test
    void MEDIAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MEDIAN(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(2.0, t, 0.0001);

        t = engine.TryEvaluate("MEDIAN(1,1,2,2)", 0.0);
        assertEquals(1.5, t, 0.0001);
    }

    @Test
    void MIN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MIN(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(1.0, t, 0.0001);
    }

    @Test
    void QUARTILE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),0)", 0.0);
        assertEquals(1.0, t, 0.0001);
        t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),1)", 0.0);
        assertEquals(1.75, t, 0.0001);
        t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),2)", 0.0);
        assertEquals(2.0, t, 0.0001);
        t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),3)", 0.0);
        assertEquals(3.25, t, 0.0001);
        t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),4)", 0.0);
        assertEquals(4.0, t, 0.0001);
    }

    @Test
    void MODE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MODE(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(2.0, t, 0.0001);
    }

    @Test
    void PERCENTILE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PERCENTILE(array(1,2,3,4,2,2,1,4),0.4)", 0.0);
        assertEquals(2.0, t, 0.0001);
    }

    @Test
    void PERCENTRANK_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3)", 0.0);
        assertEquals(0.714, t, 0.001);
    }

    @Test
    void AVERAGE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("AVERAGE(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(2.375, t, 0.0001);
    }

    @Test
    void GEOMEAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GEOMEAN(1,2,3,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(2.213363839 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void HARMEAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("HARMEAN(1,2,3,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.92 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void COUNT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COUNT(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(8.0, t, 0.0001);
    }

    @Test
    void AVEDEV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("AVEDEV(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(0.96875, t, 0.0001);
    }

    @Test
    void STDEV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("STDEV(1,2,3,4,2,2,1,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.187734939 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void STDEVP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("STDEVP(1,2,3,4,2,2,1,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.111024302 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void DEVSQ_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("DEVSQ(1,2,3,4,2,2,1,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(9.875 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void VAR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("VAR(1,2,3,4,2,2,1,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.410714286 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void VARP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("VARP(1,2,3,4,2,2,1,4)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.234375 * 1000000) / 1000000.0, rounded);
    }

    // 统计函数
    @Test
    void RANK_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RANK(3, array(1, 2, 3, 4, 5))", 0.0);
        assertEquals(3, t, 0.0001);

        t = engine.TryEvaluate("RANK(3, array(1, 2, 3, 4, 5), 0)", 0.0);
        assertEquals(3, t, 0.0001);

        t = engine.TryEvaluate("RANK(3, array(1, 2, 3, 4, 5), 1)", 0.0);
        assertEquals(3, t, 0.0001);

        t = engine.TryEvaluate("RANK(5, array(1, 2, 3, 4, 5))", 0.0);
        assertEquals(1, t, 0.0001);

        t = engine.TryEvaluate("RANK(1, array(1, 2, 3, 4, 5))", 0.0);
        assertEquals(5, t, 0.0001);
    }

    @Test
    void FORECAST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FORECAST(30, array(6, 7, 9, 15, 21), array(20, 28, 31, 38, 40))", 0.0);
        double rounded = Math.round(t * 10000) / 10000.0;
        assertEquals(Math.round(10.6073 * 10000) / 10000.0, rounded);
    }

    @Test
    void INTERCEPT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("INTERCEPT(array(2, 3, 9, 1, 8), array(6, 5, 11, 7, 5))", 0.0);
        double rounded = Math.round(t * 10000) / 10000.0;
        assertEquals(Math.round(0.0484 * 10000) / 10000.0, rounded);
    }

    @Test
    void SLOPE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SLOPE(array(2, 3, 9, 1, 8), array(6, 5, 11, 7, 5))", 0.0);
        double rounded = Math.round(t * 10000) / 10000.0;
        assertEquals(Math.round(0.6694 * 10000) / 10000.0, rounded);
    }

    @Test
    void CORREL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("CORREL(array(3, 2, 4, 5, 6), array(9, 7, 12, 15, 17))", 0.0);
        double rounded = Math.round(t * 10000) / 10000.0;
        assertEquals(Math.round(0.9971 * 10000) / 10000.0, rounded);
    }

    @Test
    void PEARSON_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PEARSON(array(3, 2, 4, 5, 6), array(9, 7, 12, 15, 17))", 0.0);
        double rounded = Math.round(t * 10000) / 10000.0;
        assertEquals(Math.round(0.9971 * 10000) / 10000.0, rounded);
    }

    @Test
    void LARGE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LARGE(array(1,2,3,4,2,2,1,4),3)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(3.0 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void SMALL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SMALL(array(1,2,3,4,2,2,1,4),3)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(2.0 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void COVAR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COVAR(array(3,7,6,11),array(5,15,13,9))", 0.0);
        double t1 = engine.TryEvaluate("COVARIANCE.P(array(3,7,6,11),array(5,15,13,9))", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        double rounded1 = Math.round(t1 * 1000000) / 1000000.0;
        assertEquals(Math.round(3.375 * 1000000) / 1000000.0, rounded);
        assertEquals(Math.round(3.375 * 1000000) / 1000000.0, rounded1);
    }

    @Test
    void COVARIANCES_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COVARIANCE.S(array(3,7,6,11),array(5,15,13,9))", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(4.5 * 1000000) / 1000000.0, rounded);
    }

    // 条件统计
    @Test
    void COUNTIF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
        assertEquals(t, 6.0);
        t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>=1')", 0.0);
        assertEquals(t, 8.0);
        t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'!=1')", 0.0);
        assertEquals(t, 6.0);
        t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'=1')", 0.0);
        assertEquals(t, 2.0);
        t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'<=1')", 0.0);
        assertEquals(t, 2.0);
        t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'<2')", 0.0);
        assertEquals(t, 2.0);
    }

    @Test
    void SUMIF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
        assertEquals(t, 17);
        t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(t, 6);
        t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>=2',array(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(t, 6);
        t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'<1',array(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(t, 0);
        t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'==1',array(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(t, 2);
    }

    @Test
    void AVERAGEIF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
        assertEquals(t, 2.833333333, 6);

        t = engine.TryEvaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(t, 1);
    }

    // 数组运算
    @Test
    void SERIESSUM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SERIESSUM(2, 0, 1, array(1, 1, 1, 1))", 0.0);
        assertEquals(t, 15.0);
    }

    @Test
    void SUMPRODUCT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMPRODUCT(array(1, 2, 3), array(4, 5, 6))", 0.0);
        assertEquals(t, 32.0);
    }

    @Test
    void SUMX2MY2_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMX2MY2(array(1, 2, 3), array(4, 5, 6))", 0.0);
        assertEquals(t, -63.0);
    }

    @Test
    void SUMX2PY2_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMX2PY2(array(1, 2, 3), array(4, 5, 6))", 0.0);
        assertEquals(t, 91.0);
    }

    @Test
    void SUMXMY2_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMXMY2(array(1, 2, 3), array(4, 5, 6))", 0.0);
        assertEquals(t, 27.0);
    }
}
