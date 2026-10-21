package test

import (
	"math"
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// TestUnitConversionPrecisionTest 移植自
// csharp/ToolGood.Algorithm.Test/UnitConversion/UnitConversionPrecisionTest.cs。
//
// 精确分数系数防回归测试：这些系数由 6 位近似值重构为精确分数，
// 反向换算误差应远小于 1e-9。
func TestUnitConversionPrecisionTest(t *testing.T) {
	assertClose := func(t *testing.T, src, tar string, expected float64) {
		t.Helper()
		result, err := alg.UnitConversion(alg.DecimalOne, src, tar, "精度测试")
		if err != nil {
			t.Fatalf("UnitConversion(1, %s, %s): %v", src, tar, err)
		}
		d := result.Float64()
		rel := math.Abs(d-expected) / math.Max(math.Abs(expected), 1e-30)
		if rel >= 1e-9 {
			t.Fatalf("%s -> %s: want %v, got %v (rel=%v)", src, tar, expected, d, rel)
		}
	}

	t.Run("Mass_ShortTon_Exact", func(t *testing.T) {
		assertClose(t, "short ton", "kg", 907.18474)
		assertClose(t, "kg", "short ton", 1.0/907.18474)
		assertClose(t, "short ton", "lb", 2000)
	})

	t.Run("Mass_LongTon_Exact", func(t *testing.T) {
		assertClose(t, "long ton", "kg", 1016.0469088)
		assertClose(t, "kg", "long ton", 1.0/1016.0469088)
		assertClose(t, "long ton", "lb", 2240)
	})

	t.Run("Area_Mu_Exact", func(t *testing.T) {
		assertClose(t, "亩", "m²", 666.6666666666667)
		assertClose(t, "m²", "亩", 0.0015)
	})

	t.Run("Volume_CubicFoot_Exact", func(t *testing.T) {
		assertClose(t, "ft³", "l", 28.316846592)
		assertClose(t, "l", "ft³", 0.03531466672148859)
		assertClose(t, "ft³", "in³", 1728)
	})

	t.Run("Volume_CubicInch_Exact", func(t *testing.T) {
		assertClose(t, "in³", "l", 0.016387064)
		assertClose(t, "l", "in³", 61.0237440947323)
	})

	t.Run("Volume_ImperialPint_Exact", func(t *testing.T) {
		assertClose(t, "imperial pint", "l", 0.56826125)
		assertClose(t, "l", "imperial pint", 1.7597539863927023)
	})

	t.Run("Volume_ImperialGallon_Exact", func(t *testing.T) {
		assertClose(t, "imperial gallon", "l", 4.54609)
		assertClose(t, "l", "imperial gallon", 0.2199692482990878)
		assertClose(t, "imperial gallon", "imperial pint", 8)
		assertClose(t, "imperial gallon", "imperial quart", 4)
	})

	t.Run("Volume_ImperialQuart_Exact", func(t *testing.T) {
		assertClose(t, "imperial quart", "l", 1.1365225)
		assertClose(t, "l", "imperial quart", 0.87987699319635115)
	})

	t.Run("Volume_USPint_Exact", func(t *testing.T) {
		assertClose(t, "US pint", "l", 0.473176473)
		assertClose(t, "l", "US pint", 2.1133764188651873)
	})

	t.Run("Volume_USGallon_Exact", func(t *testing.T) {
		assertClose(t, "US gallon", "l", 3.785411784)
		assertClose(t, "l", "US gallon", 0.2641720523581484)
		assertClose(t, "US gallon", "US quart", 4)
	})
}
