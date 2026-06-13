package toolgood.algorithm.algorithmengine;

import java.util.Arrays;
import java.util.List;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.AlgorithmEngineEx;
import toolgood.algorithm.Operand;

public class FinancialTest {
    @Test
    public void PMT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PMT(0.08/12, 10, 10000)", 0.0);
        assertEquals(-1037.0321, t, 1e-4);
    }

    @Test
    public void PPMT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PPMT(0.08/12, 1, 10, 10000)", 0.0);
        assertEquals(-970.3654, t, 1e-4);
    }

    @Test
    public void IPMT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("IPMT(0.08/12, 1, 10, 10000)", 0.0);
        assertEquals(-66.6667, t, 1e-4);
    }

    @Test
    public void PV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PV(0.08/12, 10, -1000)", 0.0);
        assertEquals(9642.90, t, 1e-2);
    }

    @Test
    public void FV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FV(0.08/12, 10, -1000)", 0.0);
        assertEquals(10305.40, t, 1e-2);
    }

    @Test
    public void NPER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NPER(0.08/12, -1000, 10000)", 0.0);
        assertEquals(10.38, t, 1e-2);
    }

    @Test
    public void RATE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RATE(12,-100,400,0,0,0.1)", 0.0);
        assertEquals(0.2289, t, 1e-4);
    }

    @Test
    public void NPV_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("NPV(0.1, -10000, 3000, 4200, 6800)", 0.0);
        assertEquals(1188.44, t, 1e-2);
    }

    @Test
    public void IRR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("IRR(array(-70000, 12000, 15000, 18000, 21000, 26000))", 0.0);
        assertEquals(0.0866, t, 1e-4);
    }

    @Test
    public void MIRR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MIRR(array(-70000, 12000, 15000, 18000, 21000, 26000), 0.1, 0.12)", 0.0);
        assertEquals(0.0987, t, 1e-4);
    }

    @Test
    public void SLN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SLN(30000, 7500, 10)", 0.0);
        assertEquals(2250.0, t, 1e-10);
    }

    @Test
    public void SYD_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SYD(30000, 7500, 10, 1)", 0.0);
        assertEquals(4090.91, t, 1e-2);
    }

    @Test
    public void DDB_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("DDB(2400, 300, 10, 2)", 0.0);
        assertEquals(384.0, t, 1e-2);
    }

    @Test
    public void DB_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("DB(1000000, 100000, 6, 1)", 0.0);
        assertEquals(319000.0, t, 1e-2);
    }

    @Test
    public void XNPV_test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameter("values", Arrays.asList(
            Operand.Create(-10000.0), Operand.Create(2750.0), Operand.Create(4250.0), Operand.Create(3250.0), Operand.Create(2750.0)));
        engine.AddParameter("dates2", Arrays.asList(
            Operand.Create("2008-1-1"), Operand.Create("2008-3-1"), Operand.Create("2008-10-30"), Operand.Create("2009-2-15"), Operand.Create("2009-4-1")));
        double t = engine.TryEvaluate("XNPV(0.09, values, dates2)", 0.0);
        assertEquals(2086.65, t, 1e-2);
    }

    @Test
    public void XIRR_test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameter("values", Arrays.asList(
            Operand.Create(-10000.0), Operand.Create(2750.0), Operand.Create(4250.0), Operand.Create(3250.0), Operand.Create(2750.0)));
        engine.AddParameter("dates2", Arrays.asList(
            Operand.Create("2008-1-1"), Operand.Create("2008-3-1"), Operand.Create("2008-10-30"), Operand.Create("2009-2-15"), Operand.Create("2009-4-1")));
        double t = engine.TryEvaluate("XIRR(values, dates2)", 0.0);
        assertEquals(0.3734, t, 1e-4);
    }
}
