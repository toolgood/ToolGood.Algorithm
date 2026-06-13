package toolgood.algorithm.algorithmengine;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;

import toolgood.algorithm.AlgorithmEngine;

public class MathSumTest {

    // 简单统计

    @Test
    public void MAX_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("max(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(4.0, t, 0.0);
    }

    @Test
    public void MEDIAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MEDIAN(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(2.0, t, 0.0);

        t = engine.TryEvaluate("MEDIAN(1,1,2,2)", 0.0);
        assertEquals(1.5, t, 1e-6);
    }

    @Test
    public void MIN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MIN(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(1.0, t, 0.0);
    }

    @Test
    public void QUARTILE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),0)", 0.0);
        assertEquals(1.0, t, 0.0);
        t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),1)", 0.0);
        assertEquals(1.75, t, 1e-6);
        t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),2)", 0.0);
        assertEquals(2.0, t, 0.0);
        t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),3)", 0.0);
        assertEquals(3.25, t, 1e-6);
        t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),4)", 0.0);
        assertEquals(4.0, t, 0.0);
    }

    @Test
    public void MODE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MODE(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(2.0, t, 0.0);
    }

    @Test
    public void PERCENTILE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PERCENTILE(array(1,2,3,4,2,2,1,4)，0.4)", 0.0);
        assertEquals(2.0, t, 0.0);
    }

    @Test
    public void PERCENTRANK_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4)，3)", 0.0);
        assertEquals(0.714, t, 1e-6);
    }

    @Test
    public void AVERAGE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("AVERAGE(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(2.375, t, 1e-6);
    }

    @Test
    public void GEOMEAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GEOMEAN(1,2,3,4)", 0.0);
        assertEquals(2.213363839, t, 1e-6);
    }

    @Test
    public void HARMEAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("HARMEAN(1,2,3,4)", 0.0);
        assertEquals(1.92, t, 1e-6);
    }

    @Test
    public void COUNT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COUNT(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(8.0, t, 0.0);
    }

    @Test
    public void AVEDEV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("AVEDEV(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(0.96875, t, 1e-6);
    }

    @Test
    public void STDEV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("STDEV(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(1.187734939, t, 1e-6);
    }

    @Test
    public void STDEVP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("STDEVP(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(1.111024302, t, 1e-6);
    }

    @Test
    public void DEVSQ_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("DEVSQ(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(9.875, t, 1e-6);
    }

    @Test
    public void VAR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("VAR(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(1.410714286, t, 1e-6);
    }

    @Test
    public void VARP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("VARP(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(1.234375, t, 1e-6);
    }

    // 统计函数

    @Test
    public void RANK_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("RANK(3, array(1, 2, 3, 4, 5))", 0);
        assertEquals(3, t);

        t = engine.TryEvaluate("RANK(3, array(1, 2, 3, 4, 5), 0)", 0);
        assertEquals(3, t);

        t = engine.TryEvaluate("RANK(3, array(1, 2, 3, 4, 5), 1)", 0);
        assertEquals(3, t);

        t = engine.TryEvaluate("RANK(5, array(1, 2, 3, 4, 5))", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("RANK(1, array(1, 2, 3, 4, 5))", 0);
        assertEquals(5, t);
    }

    @Test
    public void FORECAST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FORECAST(30, array(6, 7, 9, 15, 21), array(20, 28, 31, 38, 40))", 0.0);
        assertEquals(10.6073, t, 1e-4);
    }

    @Test
    public void INTERCEPT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("INTERCEPT(array(2, 3, 9, 1, 8), array(6, 5, 11, 7, 5))", 0.0);
        assertEquals(0.0484, t, 1e-4);
    }

    @Test
    public void SLOPE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SLOPE(array(2, 3, 9, 1, 8), array(6, 5, 11, 7, 5))", 0.0);
        assertEquals(0.6694, t, 1e-4);
    }

    @Test
    public void CORREL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("CORREL(array(3, 2, 4, 5, 6), array(9, 7, 12, 15, 17))", 0.0);
        assertEquals(0.9971, t, 1e-4);
    }

    @Test
    public void PEARSON_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PEARSON(array(3, 2, 4, 5, 6), array(9, 7, 12, 15, 17))", 0.0);
        assertEquals(0.9971, t, 1e-4);
    }

    @Test
    public void LARGE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LARGE(array(1,2,3,4,2,2,1,4),3)", 0.0);
        assertEquals(3.0, t, 1e-6);
    }

    @Test
    public void SMALL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SMALL(array(1,2,3,4,2,2,1,4),3)", 0.0);
        assertEquals(2.0, t, 1e-6);
    }

    @Test
    public void COVAR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COVAR(array(3,7,6,11),array(5,15,13,9))", 0.0);
        double t1 = engine.TryEvaluate("COVARIANCE.P(array(3,7,6,11),array(5,15,13,9))", 0.0);
        assertEquals(3.375, t, 1e-6);
        assertEquals(3.375, t1, 1e-6);
    }

    @Test
    public void COVARIANCES_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COVARIANCE.S(array(3,7,6,11),array(5,15,13,9))", 0.0);
        assertEquals(4.5, t, 1e-6);
    }

    // 条件统计

    @Test
    public void COUNTIF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
        assertEquals(6.0, t, 0.0);
        t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>=1')", 0.0);
        assertEquals(8.0, t, 0.0);
        t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'!=1')", 0.0);
        assertEquals(6.0, t, 0.0);
        t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'=1')", 0.0);
        assertEquals(2.0, t, 0.0);
        t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'<=1')", 0.0);
        assertEquals(2.0, t, 0.0);
        t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'<2')", 0.0);
        assertEquals(2.0, t, 0.0);
    }

    @Test
    public void SUMIF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
        assertEquals(17.0, t, 0.0);
        t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(6.0, t, 0.0);
        t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>=2',array(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(6.0, t, 0.0);
        t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'<1',array(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(0.0, t, 0.0);
        t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'==1',array(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(2.0, t, 0.0);
    }

    @Test
    public void AVERAGEIF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
        assertEquals(2.833333333, t, 1e-6);

        t = engine.TryEvaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(1.0, t, 1e-6);
    }

    // 数组运算

    @Test
    public void SERIESSUM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SERIESSUM(2, 0, 1, array(1, 1, 1, 1))", 0.0);
        assertEquals(15.0, t, 0.0);
    }

    @Test
    public void SUMPRODUCT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMPRODUCT(array(1, 2, 3), array(4, 5, 6))", 0.0);
        assertEquals(32.0, t, 0.0);
    }

    @Test
    public void SUMX2MY2_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMX2MY2(array(1, 2, 3), array(4, 5, 6))", 0.0);
        assertEquals(-63.0, t, 0.0);
    }

    @Test
    public void SUMX2PY2_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMX2PY2(array(1, 2, 3), array(4, 5, 6))", 0.0);
        assertEquals(91.0, t, 0.0);
    }

    @Test
    public void SUMXMY2_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMXMY2(array(1, 2, 3), array(4, 5, 6))", 0.0);
        assertEquals(27.0, t, 0.0);
    }
}
