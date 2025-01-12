package toolgood.algorithm.Tests;

import static org.junit.Assert.assertEquals;

import java.math.BigDecimal;

import org.junit.Test;

import toolgood.algorithm.AlgorithmEngine;

//@SuppressWarnings("deprecation")
public class AlgorithmEngineTest_sum {
    @Test
    public void MAX_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("max(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(t, 4.0, 0.001);
    }

    @Test
    public void MEDIAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MEDIAN(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(t, 2.0, 0.001);
    }

    @Test
    public void MIN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MIN(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(t, 1.0, 0.001);
    }

    @Test
    public void QUARTILE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),0)", 0.0);
        assertEquals(t, 1.0, 0.001);
        t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),1)", 0.0);
        assertEquals(t, 1.75, 0.001);
        t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),2)", 0.0);
        assertEquals(t, 2.0, 0.001);
        t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),3)", 0.0);
        assertEquals(t, 3.25, 0.001);
        t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),4)", 0.0);
        assertEquals(t, 4.0, 0.001);
    }

    @Test
    public void MODE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MODE(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(t, 2.0, 0.001);
    }

    @Test
    public void PERCENTILE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PERCENTILE(array(1,2,3,4,2,2,1,4)，0.4)", 0.0);
        assertEquals(t, 2.0, 0.001);
    }

    @Test
    public void PERCENTRANK_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4)，3)", 0.0);
        assertEquals(t, 0.714, 0.001);
    }

    @Test
    public void AVERAGE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("AVERAGE(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(t, 2.375, 0.001);
    }

    @Test
    public void GEOMEAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GEOMEAN(1,2,3,4)", 0.0);
        assertEquals(round(t, 6), round(2.213363839, 6), 0.001);
    }

    @Test
    public void HARMEAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("HARMEAN(1,2,3,4)", 0.0);
        assertEquals(round(t, 6), round(1.92, 6), 0.001);
    }

    @Test
    public void COUNT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COUNT(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(t, 8.0, 0.001);
    }

    @Test
    public void AVEDEV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("AVEDEV(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(t, 0.96875, 0.001);
    }

    @Test
    public void STDEV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("STDEV(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(round(t, 6), round(1.187734939, 6), 0.001);
    }

    @Test
    public void STDEVP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("STDEVP(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(round(t, 6), round(1.111024302, 6), 0.001);
    }

    @Test
    public void DEVSQ_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("DEVSQ(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(round(t, 6), round(9.875, 6), 0.001);
    }

    @Test
    public void VAR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("VAR(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(round(t, 6), round(1.410714286, 6), 0.001);
    }

    @Test
    public void VARP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("VARP(1,2,3,4,2,2,1,4)", 0.0);
        assertEquals(round(t, 6), round(1.234375, 6), 0.001);
    }

    @Test
    public void NORMSDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NORMSDIST(1)", 0.0);
        assertEquals(round(t, 6), round(0.841344746, 6), 0.001);
    }

    @Test
    public void NORMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NORMDIST(3,8,4,1)", 0.0);
        assertEquals(round(t, 6), round(0.105649774, 6), 0.001);
        t = engine.TryEvaluate("NORMDIST(3,8,4,0)", 0.0);
        assertEquals(round(t, 6), round(0.045662271, 6), 0.001);
    }

    @Test
    public void NORMINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NORMINV(0.8,8,3)", 0.0);
        assertEquals(round(t, 6), round(10.5248637, 6), 0.001);
    }

    @Test
    public void NORMSINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NORMSINV(0.3)", 0.0);
        assertEquals(round(t, 6), round(-0.524400513, 6), 0.001);
    }

    @Test
    public void BETADIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BETADIST(0.5,11,22)", 0.0);
        assertEquals(round(t, 6), round(0.97494877, 6), 0.001);
    }

    @Test
    public void BETAINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BETAINV(0.5,23,45)", 0.0);
        assertEquals(round(t, 6), round(0.336640759, 6), 0.001);
    }

    @Test
    public void BINOMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BINOMDIST(12,45,0.5,0)", 0.0);
        assertEquals(round(t, 6), round(0.000817409, 6), 0.001);
        t = engine.TryEvaluate("BINOMDIST(12,45,0.5,1)", 0.0);
        assertEquals(round(t, 6), round(0.00122945, 6), 0.001);
    }

    @Test
    public void EXPONDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("EXPONDIST(3,1,0)", 0.0);
        assertEquals(round(t, 6), round(0.049787068, 6), 0.001);
        t = engine.TryEvaluate("EXPONDIST(3,1,1)", 0.0);
        assertEquals(round(t, 6), round(0.950212932, 6), 0.001);
    }

    @Test
    public void FDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FDIST(0.4,2,3)", 0.0);
        assertEquals(round(t, 6), round(0.701465776, 6), 0.001);
    }

    @Test
    public void FINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FINV(0.7,2,3)", 0.0);
        assertEquals(round(t, 6), round(0.402651432, 6), 0.001);
    }

    @Test
    public void GAMMADIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GAMMADIST(0.5,3,4,0)", 0.0);
        assertEquals(round(t, 6), round(0.001723627, 6), 0.001);
        t = engine.TryEvaluate("GAMMADIST(0.5,3,4,1)", 0.0);
        assertEquals(round(t, 6), round(0.000296478, 6), 0.001);
    }

    @Test
    public void GAMMAINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GAMMAINV(0.2,3,4)", 0.0);
        assertEquals(round(t, 6), round(6.140176811, 6), 0.001);
    }

    @Test
    public void GAMMALN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GAMMALN(4)", 0.0);
        assertEquals(round(t, 6), round(1.791759469, 6), 0.001);
    }

    @Test
    public void HYPGEOMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("HYPGEOMDIST(23,45,45,100)", 0.0);
        assertEquals(round(t, 6), round(0.08715016, 6), 0.001);
    }

    @Test
    public void LOGINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LOGINV(0.1,45,33)", 0.0);
        assertEquals(round(t, 6), round(15.01122624, 6), 0.001);
    }

    @Test
    public void LOGNORMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LOGNORMDIST(15,23,45)", 0.0);
        assertEquals(round(t, 6), round(0.326019201, 6), 0.001);
    }

    @Test
    public void NEGBINOMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NEGBINOMDIST(23,45,0.7)", 0.0);
        assertEquals(round(t, 6), round(0.053463314, 6), 0.001);
    }

    @Test
    public void POISSON_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("POISSON(23,23,0)", 0.0);
        assertEquals(round(t, 6), round(0.082884384, 6), 0.001);
        t = engine.TryEvaluate("POISSON(23,23,1)", 0.0);
        assertEquals(round(t, 6), round(0.555149936, 6), 0.001);
    }

    @Test
    public void TDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("TDIST(1.2,24,1)", 0.0);
        assertEquals(round(t, 6), round(0.120925677, 6), 0.001);
        t = engine.TryEvaluate("TDIST(1.2,24,2)", 0.0);
        assertEquals(round(t, 6), round(0.241851353, 6), 0.001);
    }

    @Test
    public void TINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("TINV(0.12,23)", 0.0);
        assertEquals(round(t, 6), round(1.614756561, 6), 0.001);
    }

    @Test
    public void WEIBULL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("WEIBULL(1,2,3,1)", 0.0);
        assertEquals(round(t, 6), round(0.105160683, 6), 0.001);
        t = engine.TryEvaluate("WEIBULL(1,2,3,0)", 0.0);
        assertEquals(round(t, 6), round(0.198853182, 6), 0.001);
        t = engine.TryEvaluate("WEIBULL(-1,2,3,0)", 0.0);
        t = engine.TryEvaluate("WEIBULL(-1,-2,3,0)", 0.0);
        t = engine.TryEvaluate("WEIBULL(-1,-2,-3,0)", 0.0);
        t = engine.TryEvaluate("WEIBULL(-1,-2,-3,-1)", 0.0);

    }

    @Test
    public void FISHER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FISHER(0.68)", 0.0);
        assertEquals(round(t, 6), round(0.8291140383, 6), 0.001);
    }

    @Test
    public void FISHERINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FISHERINV(0.6)", 0.0);
        assertEquals(round(t, 6), round(0.537049567, 6), 0.001);
    }

    @Test
    public void LARGE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LARGE(array(1,2,3,4,2,2,1,4),3)", 0.0);
        assertEquals(round(t, 6), round(3.0, 6), 0.001);
    }

    @Test
    public void SMALL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SMALL(array(1,2,3,4,2,2,1,4),3)", 0.0);
        assertEquals(round(t, 6), round(2.0, 6), 0.001);
    }

    @Test
    public void COUNTIF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
        assertEquals(t, 6.0, 0.001);
    }

    @Test
    public void SUMIF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
        assertEquals(t, 17, 0.001);
        t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(t, 6, 0.001);
    }

    @Test
    public void AVERAGEIF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
        assertEquals(round(t, 6), round(2.833333333, 6), 0.001);
        t = engine.TryEvaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0);
        assertEquals(t, 1, 0.001);
    }

    @Test
    public void COVAR_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COVAR(array(3,7,6,11),array(5,15,13,9))", 0.0);
        double t1 = engine.TryEvaluate("COVARIANCE.P(array(3,7,6,11),array(5,15,13,9))", 0.0);
        assertEquals(round(t, 6), round(3.375, 6), 0.001);
        assertEquals(round(t1, 6), round(3.375, 6), 0.001);
    }

    @Test
    public void COVARIANCES_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("COVARIANCE.S(array(3,7,6,11),array(5,15,13,9))", 0.0);
        assertEquals(round(t, 6), round(4.5, 6), 0.001);
    }
    // @SuppressWarnings("deprecation")
    private double round(final double value, final int p) {
        final BigDecimal bigD = new BigDecimal(value);
        return bigD.setScale(p, BigDecimal.ROUND_HALF_UP).doubleValue();
    }
}