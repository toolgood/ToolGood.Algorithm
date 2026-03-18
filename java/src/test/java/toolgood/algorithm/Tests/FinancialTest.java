package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;

import static org.junit.Assert.assertEquals;

class FinancialTest {

    @Test
    void PMT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PMT(0.08/12, 10, 10000)", 0.0);
        assertEquals(Math.round(t * 10000) / 10000.0, Math.round(-1037.0321 * 10000) / 10000.0);
    }

    @Test
    void PPMT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PPMT(0.08/12, 1, 10, 10000)", 0.0);
        assertEquals(Math.round(t * 10000) / 10000.0, Math.round(-970.3654 * 10000) / 10000.0);
    }

    @Test
    void IPMT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("IPMT(0.08/12, 1, 10, 10000)", 0.0);
        assertEquals(Math.round(t * 10000) / 10000.0, Math.round(-66.6667 * 10000) / 10000.0);
    }

    @Test
    void PV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PV(0.08/12, 10, -1000)", 0.0);
        assertEquals(Math.round(t * 100) / 100.0, Math.round(9642.90 * 100) / 100.0);
    }

    @Test
    void FV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FV(0.08/12, 10, -1000)", 0.0);
        assertEquals(Math.round(t * 100) / 100.0, Math.round(10305.40 * 100) / 100.0);
    }

    @Test
    void NPER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NPER(0.08/12, -1000, 10000)", 0.0);
        assertEquals(Math.round(t * 100) / 100.0, Math.round(10.38 * 100) / 100.0);
    }

    @Test
    void RATE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RATE(12,-100,400,0,0,0.1)", 0.0);
        assertEquals(Math.round(t * 10000) / 10000.0, Math.round(0.2289 * 10000) / 10000.0);
    }

    @Test
    void NPV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NPV(0.1, -10000, 3000, 4200, 6800)", 0.0);
        assertEquals(Math.round(t * 100) / 100.0, Math.round(1188.44 * 100) / 100.0);
    }

    @Test
    void IRR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("IRR(array(-70000, 12000, 15000, 18000, 21000, 26000))", 0.0);
        assertEquals(Math.round(t * 10000) / 10000.0, Math.round(0.0866 * 10000) / 10000.0);
    }

    @Test
    void MIRR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MIRR(array(-70000, 12000, 15000, 18000, 21000, 26000), 0.1, 0.12)", 0.0);
        assertEquals(Math.round(t * 10000) / 10000.0, Math.round(0.0987 * 10000) / 10000.0);
    }

    @Test
    void SLN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SLN(30000, 7500, 10)", 0.0);
        assertEquals(t, 2250.0);
    }

    @Test
    void SYD_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SYD(30000, 7500, 10, 1)", 0.0);
        assertEquals(Math.round(t * 100) / 100.0, Math.round(4090.91 * 100) / 100.0);
    }

    @Test
    void DDB_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("DDB(2400, 300, 10, 2)", 0.0);
        assertEquals(Math.round(t * 100) / 100.0, Math.round(384.0 * 100) / 100.0);
    }

    @Test
    void DB_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("DB(1000000, 100000, 6, 1)", 0.0);
        assertEquals(Math.round(t * 100) / 100.0, Math.round(319000.0 * 100) / 100.0);
    }

    @Test
    void XNPV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        // Java版本可能需要不同的方式来处理数组参数
        // 这里使用字符串数组的方式
        double t = engine.TryEvaluate("XNPV(0.09, array(-10000, 2750, 4250, 3250, 2750), array('2008-1-1', '2008-3-1', '2008-10-30', '2009-2-15', '2009-4-1'))", 0.0);
        assertEquals(Math.round(t * 100) / 100.0, Math.round(2086.65 * 100) / 100.0);
    }

    @Test
    void XIRR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        // Java版本可能需要不同的方式来处理数组参数
        // 这里使用字符串数组的方式
        double t = engine.TryEvaluate("XIRR(array(-10000, 2750, 4250, 3250, 2750), array('2008-1-1', '2008-3-1', '2008-10-30', '2009-2-15', '2009-4-1'))", 0.0);
        assertEquals(Math.round(t * 10000) / 10000.0, Math.round(0.3734 * 10000) / 10000.0);
    }
}
