package test

import (
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// TestAlgorithmEngineHelperTest 移植自
// csharp/ToolGood.Algorithm.Test/AlgorithmEngineHelper/AlgorithmEngineHelperTest.cs。
func TestAlgorithmEngineHelperTest(t *testing.T) {
	t.Run("GetDiyNames_test", func(t *testing.T) {
		p, err := alg.GetDiyNames("dd")
		assertNull(t, err)
		assertText(t, "dd", p.Parameters[0].Name)

		p3, err := alg.GetDiyNames("dd")
		assertNull(t, err)
		assertText(t, "dd", p3.Parameters[0].Name)

		p5, err := alg.GetDiyNames("ddd(d1,22)")
		assertNull(t, err)
		assertText(t, "ddd", p5.Functions[0].Name)
		assertText(t, "d1", p5.Parameters[0].Name)

		p6, err := alg.GetDiyNames("长")
		assertNull(t, err)
		assertText(t, "长", p6.Parameters[0].Name)

		p7, err := alg.GetDiyNames("ddd+2")
		assertNull(t, err)
		assertText(t, "ddd", p7.Parameters[0].Name)

		p8, err := alg.GetDiyNames(`{"A": 0.6,"B": 0.4,"C": 0.6,"E": 0.33,"F": 0.29,"Z": 0.15
,"EB": 0.7,"EE": 0.65,"EA": 0.85,"AB": 1.0,"BC": 1.0,"AA":1.0
,"EBC": 1.15,"BAB": 1.25,"BCB": 1.25,"BBC": 1.25,"CBB": 1.25,"EBA": 1.2,"AAA": 1.4}[瓦楞]`)
		assertNull(t, err)
		assertText(t, "瓦楞", p8.Parameters[0].Name)
	})

	t.Run("IsParameter_Test", func(t *testing.T) {
		assertFalse(t, alg.IsParameter("false"), "false 不是参数")
		assertTrue(t, alg.IsParameter("f11"), "f11 是参数")
		assertFalse(t, alg.IsParameter("f11+1"), "f11+1 不是参数")
		assertFalse(t, alg.IsParameter("f11++"), "f11++ 不是参数")
	})

	t.Run("UnitConversion_test", func(t *testing.T) {
		b, err := alg.UnitConversion(alg.DecimalOne, "米", "千米", "测试")
		assertNull(t, err)
		assertDecimal(t, "0.001", b)

		b, err = alg.UnitConversion(alg.DecimalOne, "米", "分米", "测试")
		assertNull(t, err)
		assertDecimal(t, "10", b)

		b, err = alg.UnitConversion(alg.DecimalOne, "米", "厘米", "测试")
		assertNull(t, err)
		assertDecimal(t, "100", b)

		b, err = alg.UnitConversion(alg.DecimalOne, "米", "mm", "测试")
		assertNull(t, err)
		assertDecimal(t, "1000", b)

		b, err = alg.UnitConversion(alg.DecimalOne, "m2", "dm2", "测试")
		assertNull(t, err)
		assertDecimal(t, "100", b)

		b, err = alg.UnitConversion(alg.DecimalOne, "m2", "cm2", "测试")
		assertNull(t, err)
		assertDecimal(t, "10000", b)

		b, err = alg.UnitConversion(alg.DecimalOne, "m2", "mm2", "测试")
		assertNull(t, err)
		assertDecimal(t, "1000000", b)

		b, err = alg.UnitConversion(alg.DecimalOne, "m3", "dm3", "测试")
		assertNull(t, err)
		assertDecimal(t, "1000", b)

		b, err = alg.UnitConversion(alg.DecimalOne, "m3", "cm3", "测试")
		assertNull(t, err)
		assertDecimal(t, "1000000", b)

		b, err = alg.UnitConversion(alg.DecimalOne, "m3", "mm3", "测试")
		assertNull(t, err)
		assertDecimal(t, "1000000000", b)

		b, err = alg.UnitConversion(alg.DecimalOne, "t", "kg", "测试")
		assertNull(t, err)
		assertDecimal(t, "1000", b)

		b, err = alg.UnitConversion(alg.DecimalOne, "t", "g", "测试")
		assertNull(t, err)
		assertDecimal(t, "1000000", b)
	})

	t.Run("CheckFormula_test", func(t *testing.T) {
		assertTrue(t, alg.CheckFormula("1+1"), "1+1 应合法")
		assertFalse(t, alg.CheckFormula("1+"), "1+ 应非法")
		assertFalse(t, alg.CheckFormula("@123+1"), "@123+1 应非法")
	})

	t.Run("ParseFormula_NumberOverflow_Test", func(t *testing.T) {
		// 与 C# 一致：超出 decimal 范围的数字在解析阶段报 "is invalid or out of range"。
		for _, exp := range []string{
			"12345678901234567890123456789012345",
			"12345678901234567890123456789012345M",
			"9999999999999999999999999999999",
		} {
			if _, err := alg.ParseFormula(exp); err == nil {
				t.Fatalf("ParseFormula(%q) expected out-of-range error", exp)
			} else {
				assertContains(t, err.Error(), "invalid or out of range")
			}
		}

		// 正常数字（含单位、科学计数法、负数）不受影响
		for _, exp := range []string{"12CM", "10M2", "-5", "-5M", "3E5", "1234567890123456789012345678"} {
			fn, err := alg.ParseFormula(exp)
			if err != nil {
				t.Fatalf("ParseFormula(%q) error: %v", exp, err)
			}
			assertNotNull(t, fn)
		}
	})
}
