package toolgood.algorithm.AlgorithmEngine;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;

public class MathSum2Test {

    @Test
    public void NORMSDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NORMSDIST(1)", 0.0);
        assertEquals(0.841344746, t, 1e-6);
    }

    @Test
    public void NORMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NORMDIST(3,8,4,1)", 0.0);
        assertEquals(0.105649774, t, 1e-6);
        t = engine.TryEvaluate("NORMDIST(3,8,4,0)", 0.0);
        assertEquals(0.045662271, t, 1e-6);
    }

    @Test
    public void NORMINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NORMINV(0.8,8,3)", 0.0);
        assertEquals(10.5248637, t, 1e-6);
    }

    @Test
    public void NORMSINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NORMSINV(0.3)", 0.0);
        assertEquals(-0.524400513, t, 1e-6);
    }

    @Test
    public void BETADIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BETADIST(0.5,11,22)", 0.0);
        assertEquals(0.97494877, t, 1e-6);
    }

    @Test
    public void BETAINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BETAINV(0.5,23,45)", 0.0);
        assertEquals(0.336640759, t, 1e-6);
    }

    @Test
    public void BINOMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BINOMDIST(12,45,0.5,0)", 0.0);
        assertEquals(0.000817409, t, 1e-6);
        t = engine.TryEvaluate("BINOMDIST(12,45,0.5,1)", 0.0);
        assertEquals(0.00122945, t, 1e-6);
    }

    @Test
    public void EXPONDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("EXPONDIST(3,1,0)", 0.0);
        assertEquals(0.049787068, t, 1e-6);
        t = engine.TryEvaluate("EXPONDIST(3,1,1)", 0.0);
        assertEquals(0.950212932, t, 1e-6);
    }

    @Test
    public void FDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FDIST(0.4,2,3)", 0.0);
        assertEquals(0.701465776, t, 1e-6);
    }

    @Test
    public void FINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FINV(0.7,2,3)", 0.0);
        assertEquals(0.402651432, t, 1e-6);
    }

    @Test
    public void GAMMADIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GAMMADIST(0.5,3,4,0)", 0.0);
        assertEquals(0.001723627, t, 1e-6);
        t = engine.TryEvaluate("GAMMADIST(0.5,3,4,1)", 0.0);
        assertEquals(0.000296478, t, 1e-6);
    }

    @Test
    public void GAMMAINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GAMMAINV(0.2,3,4)", 0.0);
        assertEquals(6.140176811, t, 1e-6);
    }

    @Test
    public void GAMMALN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GAMMALN(4)", 0.0);
        assertEquals(1.791759469, t, 1e-6);
    }

    @Test
    public void HYPGEOMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("HYPGEOMDIST(23,45,45,100)", 0.0);
        assertEquals(0.08715016, t, 1e-6);
    }

    @Test
    public void LOGINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LOGINV(0.1,45,33)", 0.0);
        assertEquals(15.01122624, t, 1e-6);
    }

    @Test
    public void LOGNORMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LOGNORMDIST(15,23,45)", 0.0);
        assertEquals(0.326019201, t, 1e-6);
    }

    @Test
    public void NEGBINOMDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NEGBINOMDIST(23,45,0.7)", 0.0);
        assertEquals(0.053463314, t, 1e-6);
    }

    @Test
    public void POISSON_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("POISSON(23,23,0)", 0.0);
        assertEquals(0.082884384, t, 1e-6);
        t = engine.TryEvaluate("POISSON(23,23,1)", 0.0);
        assertEquals(0.555149936, t, 1e-6);
    }

    @Test
    public void TDIST_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("TDIST(1.2,24,1)", 0.0);
        assertEquals(0.120925677, t, 1e-6);
        t = engine.TryEvaluate("TDIST(1.2,24,2)", 0.0);
        assertEquals(0.241851353, t, 1e-6);
    }

    @Test
    public void TINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("TINV(0.12,23)", 0.0);
        assertEquals(1.614756561, t, 1e-6);
    }

    @Test
    public void WEIBULL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("WEIBULL(1,2,3,1)", 0.0);
        assertEquals(0.105160683, t, 1e-6);
        t = engine.TryEvaluate("WEIBULL(1,2,3,0)", 0.0);
        assertEquals(0.198853182, t, 1e-6);
        t = engine.TryEvaluate("WEIBULL(-1,2,3,0)", 0.0);
        t = engine.TryEvaluate("WEIBULL(-1,-2,3,0)", 0.0);
        t = engine.TryEvaluate("WEIBULL(-1,-2,-3,0)", 0.0);
        t = engine.TryEvaluate("WEIBULL(-1,-2,-3,-1)", 0.0);
    }

    @Test
    public void FISHER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FISHER(0.68)", 0.0);
        assertEquals(0.8291140383, t, 1e-6);
    }

    @Test
    public void FISHERINV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FISHERINV(0.6)", 0.0);
        assertEquals(0.537049567, t, 1e-6);
    }

    // Bessel函数

    @Test
    public void BESSELI_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BESSELI(1.5, 1)", 0.0);
        assertEquals(0.981666, t, 1e-6);

        t = engine.TryEvaluate("BESSELI(1.5, 0)", 0.0);
        assertEquals(1.646723, t, 1e-6);
    }

    @Test
    public void BESSELJ_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BESSELJ(1.5, 1)", 0.0);
        assertEquals(0.557937, t, 1e-6);

        t = engine.TryEvaluate("BESSELJ(1.5, 0)", 0.0);
        assertEquals(0.511828, t, 1e-6);
    }

    @Test
    public void BESSELK_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BESSELK(1.5, 1)", 0.0);
        assertEquals(0.277, t, 0.01);

        t = engine.TryEvaluate("BESSELK(1.5, 0)", 0.0);
        assertEquals(0.214, t, 0.01);
    }

    @Test
    public void BESSELY_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("BESSELY(1.5, 1)", 0.0);
        assertEquals(-0.412, t, 0.01);

        t = engine.TryEvaluate("BESSELY(2.5, 0)", 0.0);
        assertEquals(0.498, t, 0.01);
    }
}
