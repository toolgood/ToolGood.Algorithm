package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;

import static org.junit.Assert.assertEquals;

class MathSum2Test {

    @Test
    void NORMSDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NORMSDIST(1)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.841344746 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void NORMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NORMDIST(3,8,4,1)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.105649774 * 1000000) / 1000000.0, rounded);
        t = engine.TryEvaluate("NORMDIST(3,8,4,0)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.045662271 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void NORMINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NORMINV(0.8,8,3)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(10.5248637 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void NORMSINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NORMSINV(0.3)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(-0.524400513 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void BETADIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BETADIST(0.5,11,22)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.97494877 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void BETAINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BETAINV(0.5,23,45)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.336640759 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void BINOMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BINOMDIST(12,45,0.5,0)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.000817409 * 1000000) / 1000000.0, rounded);
        t = engine.TryEvaluate("BINOMDIST(12,45,0.5,1)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.00122945 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void EXPONDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("EXPONDIST(3,1,0)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.049787068 * 1000000) / 1000000.0, rounded);
        t = engine.TryEvaluate("EXPONDIST(3,1,1)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.950212932 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void FDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FDIST(0.4,2,3)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.701465776 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void FINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FINV(0.7,2,3)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.402651432 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void GAMMADIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GAMMADIST(0.5,3,4,0)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.001723627 * 1000000) / 1000000.0, rounded);
        t = engine.TryEvaluate("GAMMADIST(0.5,3,4,1)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.000296478 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void GAMMAINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GAMMAINV(0.2,3,4)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(6.140176811 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void GAMMALN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GAMMALN(4)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.791759469 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void HYPGEOMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("HYPGEOMDIST(23,45,45,100)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.08715016 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void LOGINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LOGINV(0.1,45,33)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(15.01122624 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void LOGNORMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LOGNORMDIST(15,23,45)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.326019201 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void NEGBINOMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NEGBINOMDIST(23,45,0.7)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.053463314 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void POISSON_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("POISSON(23,23,0)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.082884384 * 1000000) / 1000000.0, rounded);
        t = engine.TryEvaluate("POISSON(23,23,1)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.555149936 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void TDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("TDIST(1.2,24,1)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.120925677 * 1000000) / 1000000.0, rounded);
        t = engine.TryEvaluate("TDIST(1.2,24,2)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.241851353 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void TINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("TINV(0.12,23)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.614756561 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void WEIBULL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("WEIBULL(1,2,3,1)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.105160683 * 1000000) / 1000000.0, rounded);
        t = engine.TryEvaluate("WEIBULL(1,2,3,0)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.198853182 * 1000000) / 1000000.0, rounded);
        t = engine.TryEvaluate("WEIBULL(-1,2,3,0)", 0.0);
        t = engine.TryEvaluate("WEIBULL(-1,-2,3,0)", 0.0);
        t = engine.TryEvaluate("WEIBULL(-1,-2,-3,0)", 0.0);
        t = engine.TryEvaluate("WEIBULL(-1,-2,-3,-1)", 0.0);
    }

    @Test
    void FISHER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FISHER(0.68)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.8291140383 * 1000000) / 1000000.0, rounded);
    }

    @Test
    void FISHERINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FISHERINV(0.6)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.537049567 * 1000000) / 1000000.0, rounded);
    }

    // Bessel函数
    @Test
    void BESSELI_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BESSELI(1.5, 1)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(0.981666, rounded);

        t = engine.TryEvaluate("BESSELI(1.5, 0)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(1.646723, rounded);
    }

    @Test
    void BESSELJ_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BESSELJ(1.5, 1)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(0.557937, rounded);

        t = engine.TryEvaluate("BESSELJ(1.5, 0)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(0.511828, rounded);
    }

    @Test
    void BESSELK_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BESSELK(1.5, 1)", 0.0);
        double rounded = Math.round((Double) t * 1000) / 1000.0;
        assertEquals(0.277, rounded, 0.01);

        t = engine.TryEvaluate("BESSELK(1.5, 0)", 0.0);
        rounded = Math.round((Double) t * 1000) / 1000.0;
        assertEquals(0.214, rounded, 0.01);
    }

    @Test
    void BESSELY_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BESSELY(1.5, 1)", 0.0);
        double rounded = Math.round((Double) t * 1000) / 1000.0;
        assertEquals(-0.412, rounded, 0.01);

        t = engine.TryEvaluate("BESSELY(2.5, 0)", 0.0);
        rounded = Math.round((Double) t * 1000) / 1000.0;
        assertEquals(0.498, rounded, 0.01);
    }
}
