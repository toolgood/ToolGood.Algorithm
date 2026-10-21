package test

import (
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// TestFinancialTest 对应 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/FinancialTest.cs。
func TestFinancialTest(t *testing.T) {
	t.Run("PMT_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "PMT(0.08/12, 10, 10000)")
		assertFloatRound(t, -1037.0321, v, 4)
	})

	t.Run("PPMT_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "PPMT(0.08/12, 1, 10, 10000)")
		assertFloatRound(t, -970.3654, v, 4)
	})

	t.Run("IPMT_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "IPMT(0.08/12, 1, 10, 10000)")
		assertFloatRound(t, -66.6667, v, 4)
	})

	t.Run("PV_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "PV(0.08/12, 10, -1000)")
		assertFloatRound(t, 9642.90, v, 2)
	})

	t.Run("FV_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "FV(0.08/12, 10, -1000)")
		assertFloatRound(t, 10305.40, v, 2)
	})

	t.Run("NPER_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "NPER(0.08/12, -1000, 10000)")
		assertFloatRound(t, 10.38, v, 2)
	})

	t.Run("RATE_test", func(t *testing.T) {
		engine := newEngine()
		// 差异：RATE 迭代求解用 decimal + 自实现 ln/exp/pow，末位与 C# 有微小差异，
		// 按 C# 的 Math.Round(t, 4) 精度比较。
		v := evalFloat(engine, "RATE(12,-100,400,0,0,0.1)")
		assertFloatRound(t, 0.2289, v, 4)
	})

	t.Run("NPV_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "NPV(0.1, -10000, 3000, 4200, 6800)")
		assertFloatRound(t, 1188.44, v, 2)
	})

	t.Run("IRR_test", func(t *testing.T) {
		engine := newEngine()
		// 差异：IRR 迭代求解末位与 C# 有微小差异，按 C# 的 Math.Round(t, 4) 精度比较。
		v := evalFloat(engine, "IRR(array(-70000, 12000, 15000, 18000, 21000, 26000))")
		assertFloatRound(t, 0.0866, v, 4)
	})

	t.Run("MIRR_test", func(t *testing.T) {
		engine := newEngine()
		// 差异：MIRR 迭代求解末位与 C# 有微小差异，按 C# 的 Math.Round(t, 4) 精度比较。
		v := evalFloat(engine, "MIRR(array(-70000, 12000, 15000, 18000, 21000, 26000), 0.1, 0.12)")
		assertFloatRound(t, 0.0987, v, 4)
	})

	t.Run("SLN_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "SLN(30000, 7500, 10)")
		assertFloat(t, 2250.0, v)
	})

	t.Run("SYD_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "SYD(30000, 7500, 10, 1)")
		assertFloatRound(t, 4090.91, v, 2)
	})

	t.Run("DDB_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "DDB(2400, 300, 10, 2)")
		assertFloatRound(t, 384.0, v, 2)
	})

	t.Run("DB_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "DB(1000000, 100000, 6, 1)")
		assertFloatRound(t, 319000.0, v, 2)
	})

	t.Run("DB_month_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "DB(1000000, 100000, 6, 7, 7)")
		assertTrue(t, engine.LastError == "" && v > 0, "DB(...,7,7) 应成功且大于 0，实际 %v err=%q", v, engine.LastError)

		v = evalFloat(engine, "DB(1000000, 100000, 6, 8, 7)")
		assertTrue(t, engine.LastError != "", "DB(...,8,7) 应报错")

		v = evalFloat(engine, "DB(1000000, 100000, 6, 6, 12)")
		assertTrue(t, engine.LastError == "" && v > 0, "DB(...,6,12) 应成功且大于 0，实际 %v err=%q", v, engine.LastError)

		v = evalFloat(engine, "DB(1000000, 100000, 6, 1, 7)")
		assertTrue(t, engine.LastError == "" && v > 0, "DB(...,1,7) 应成功且大于 0，实际 %v err=%q", v, engine.LastError)
	})

	t.Run("XNPV_test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("values", []float64{-10000, 2750, 4250, 3250, 2750})
		engine.AddParameter("dates2", []string{"2008-1-1", "2008-3-1", "2008-10-30", "2009-2-15", "2009-4-1"})
		v := evalFloat(engine.AlgorithmEngine, "XNPV(0.09, values, dates2)")
		assertFloatRound(t, 2086.65, v, 2)
	})

	t.Run("XIRR_test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("values", []float64{-10000, 2750, 4250, 3250, 2750})
		engine.AddParameter("dates2", []string{"2008-1-1", "2008-3-1", "2008-10-30", "2009-2-15", "2009-4-1"})
		// 差异：XIRR 迭代求解末位与 C# 有微小差异，按 C# 的 Math.Round(t, 4) 精度比较。
		v := evalFloat(engine.AlgorithmEngine, "XIRR(values, dates2)")
		assertFloatRound(t, 0.3734, v, 4)
	})

	t.Run("IPMT_PPMT_per_out_of_range_test", func(t *testing.T) {
		engine := newEngine()
		_ = evalFloat(engine, "IPMT(0.08/12, 0, 10, 10000)")
		assertTrue(t, engine.LastError != "", "IPMT per=0 应报错")

		_ = evalFloat(engine, "PPMT(0.08/12, 11, 10, 10000)")
		assertTrue(t, engine.LastError != "", "PPMT per=11 应报错")
	})

	t.Run("NPV_parameter_test", func(t *testing.T) {
		engine := newEngine()
		_ = evalFloat(engine, "NPV(0.1)")
		assertTrue(t, engine.LastError != "", "NPV 仅一个参数应报错")
	})

	t.Run("MIRR_no_positive_test", func(t *testing.T) {
		engine := newEngine()
		_ = evalFloat(engine, "MIRR(array(-70000, -12000, -15000), 0.1, 0.12)")
		assertTrue(t, engine.LastError != "", "MIRR 无正现金流应报错")
	})

	t.Run("PV_type_test", func(t *testing.T) {
		engine := newEngine()
		pv0 := evalFloat(engine, "PV(0.08/12, 10, -1000)")
		pv1 := evalFloat(engine, "PV(0.08/12, 10, -1000, 0, 1)")
		assertFloatRound(t, pv0*(1+0.08/12), pv1, 4)
	})
}
