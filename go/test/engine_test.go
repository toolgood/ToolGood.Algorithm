package test

import (
	"math"
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// newCylinderEngine 对应 C# 的 Cylinder 类：
// 通过 AlgorithmEngineEx 注入「半径 / 直径 / 高」参数与「求面积」自定义函数。
func newCylinderEngine(radius, height int) *alg.AlgorithmEngineEx {
	e := alg.NewAlgorithmEngineEx(false)
	e.GetParameterExFunc = func(parameter string) alg.Operand {
		switch parameter {
		case "半径":
			return alg.CreateInt(radius)
		case "直径":
			return alg.CreateInt(radius * 2)
		case "高":
			return alg.CreateInt(height)
		}
		return alg.Errorf("Parameter [{0}] is missing.", parameter)
	}
	e.ExecuteDiyFunctionFunc = func(name string, args []alg.Operand) alg.Operand {
		if name == "求面积" && len(args) == 1 {
			r := args[0].ToNumber("").DoubleValue()
			return alg.CreateFloat(r * r * math.Pi)
		}
		return alg.Errorf("DiyFunction [{0}] is missing.", name)
	}
	return e
}

// TestEngineTest 对应 C# ToolGood.Algorithm.Test.EngineTest。
func TestEngineTest(t *testing.T) {
	t.Run("base_test2", func(t *testing.T) {
		e := newEngine()
		// C# 只求值、不断言结果
		_ = evalText(e, "1+(3*2+2)/2 & '11' & '11:20'*9 & isnumber(22)*3")
	})

	t.Run("base_test3", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 5, evalInt(e, "(2)+/*123456*/3"))
		assertInt(t, 5, evalInt(e, "2+3//eee"))
		assertInt(t, 0, evalInt(e, "(2)+/*123456*/3 ee22+22"))
	})

	t.Run("base_test4", func(t *testing.T) {
		e := newEngine()
		assertText(t, "", evalText(e, "'4dd'&'55' rr"))
	})

	t.Run("base_test5", func(t *testing.T) {
		e := newEngine()
		assertText(t, "4dd5", evalText(e, "'4dd'&'55'.left(1)"))
	})

	t.Run("Cylinder_Test", func(t *testing.T) {
		c := newCylinderEngine(3, 10)
		_ = c.TryEvaluateDouble("半径*半径*pi()", 0)
		_ = c.TryEvaluateDouble("直径*pi()", 0)
		_ = c.TryEvaluateDouble("半径*半径*pi()*高", 0)
		_ = c.TryEvaluateDouble("'半径'*半径*pi()*高", 0)

		got := c.TryEvaluateDouble("求面积（10）", 0)
		if math.Abs(got-10*10*math.Pi) > 10 {
			t.Fatalf("want %v (±10), got %v", 10*10*math.Pi, got)
		}

		json := `{'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'}`
		if err := c.AddParameterFromJson(json); err != nil {
			t.Fatalf("AddParameterFromJson: %v", err)
		}
		assertText(t, "L", evalText(c.AlgorithmEngine, "灰色"))
	})
}
