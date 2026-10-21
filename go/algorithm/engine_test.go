package algorithm_test

import (
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// TestEngineSmoke 基本表达式求值（对照 C# / Java 版 README 与测试用例）。
func TestEngineSmoke(t *testing.T) {
	engine := alg.NewAlgorithmEngine(false)
	cases := []struct {
		exp  string
		want string
	}{
		{"1+2*3", "7"},
		{"(1+2)*3", "9"},
		{"SUM(1,2,3)", "6"},
		{"AVERAGE(1,2,3,4)", "2.5"},
		{"MAX(1,5,3)", "5"},
		{"ABS(-3)", "3"},
		{"ROUND(1.2345,2)", "1.23"},
		{"LEFT('abcdef',3)", "abc"},
		{"RIGHT('abcdef',2)", "ef"},
		{"MID('abcdef',2,3)", "bcd"},
		{"'abc'.UPPER()", "ABC"},
		{"CONCATENATE('a','b','c')", "abc"},
		{"IF(1>2,1,2)", "2"},
		{"IFS(1>2,'a',2>1,'b')", "b"},
		{"1=1", "TRUE"},
		{"1<>1", "FALSE"},
		{"TRUE && FALSE", "FALSE"},
		{"NOT(TRUE)", "FALSE"},
		{"1KM", "1000"},
		{"100CM", "1"},
		{"LEN('中国abc')", "5"},
		{"MD5('abc')", "900150983CD24FB0D6963F7D28E17F72"},
		{"TEXT(1234.5,'#,##0.00')", "1,234.50"},
		{"DATE(2020,1,2).YEAR()", "2020"},
		{"DATE(2020,1,2).MONTH()", "1"},
		{"ADDDAYS(DATE(2020,1,31),1).DAY()", "1"},
		{"[1,2,3]", "[1,2,3]"},
	}
	for _, c := range cases {
		got := engine.TryEvaluateText(c.exp, "")
		if got != c.want {
			t.Errorf("%s => %q, want %q (LastError=%q)", c.exp, got, c.want, engine.LastError)
		}
	}
}

// TestEngineNumbers 数值求值。
func TestEngineNumbers(t *testing.T) {
	engine := alg.NewAlgorithmEngine(false)
	if v := engine.TryEvaluateInt("1+2*3", 0); v != 7 {
		t.Fatalf("1+2*3 = %d, LastError=%s", v, engine.LastError)
	}
	if v := engine.TryEvaluateDouble("SQRT(2)", 0); v < 1.4142135 || v > 1.4142136 {
		t.Fatalf("SQRT(2) = %v", v)
	}
	if v := engine.TryEvaluateNumber("10/4", alg.DecimalZero); !v.Equal(alg.MustDecimal("2.5")) {
		t.Fatalf("10/4 = %s", v.String())
	}
	if v := engine.TryEvaluateBool("1=1", false); !v {
		t.Fatalf("1=1 should be true")
	}
}

// TestEngineJson JSON 与下标访问。
func TestEngineJson(t *testing.T) {
	engine := alg.NewAlgorithmEngine(false)
	exp := "json('{\"a\":1}')['a']"
	if v := engine.TryEvaluateInt(exp, -1); v != 1 {
		t.Fatalf("%s = %d, LastError=%s", exp, v, engine.LastError)
	}
	exp = "JSON('[1,2,3]')[1]"
	if v := engine.TryEvaluateInt(exp, -1); v != 1 {
		t.Fatalf("%s = %d, LastError=%s", exp, v, engine.LastError)
	}
}

// TestEngineParameter 参数与自定义函数。
func TestEngineParameter(t *testing.T) {
	engine := alg.NewAlgorithmEngine(false)
	engine.GetParameterFunc = func(name string) alg.Operand {
		switch name {
		case "a":
			return alg.CreateInt(10)
		case "b":
			return alg.CreateInt(20)
		}
		return alg.Error("Parameter [" + name + "] is missing.")
	}
	if v := engine.TryEvaluateInt("a+b", 0); v != 30 {
		t.Fatalf("a+b = %d, LastError=%s", v, engine.LastError)
	}

	engine.ExecuteDiyFunctionFunc = func(name string, args []alg.Operand) alg.Operand {
		if name == "MYFUNC" && len(args) == 2 {
			return alg.CreateDecimal(args[0].NumberValue().Add(args[1].NumberValue()))
		}
		return alg.Error("DiyFunction [" + name + "] is missing.")
	}
	if v := engine.TryEvaluateInt("MYFUNC(3,4)", 0); v != 7 {
		t.Fatalf("MYFUNC(3,4) = %d, LastError=%s", v, engine.LastError)
	}
}

// TestEngineErrors 错误路径。
func TestEngineErrors(t *testing.T) {
	engine := alg.NewAlgorithmEngine(false)
	if v := engine.TryEvaluateInt("1/0", -1); v != -1 {
		t.Fatalf("1/0 should fall back to default")
	}
	if engine.LastError == "" {
		t.Fatalf("LastError should be set")
	}
	if v := engine.TryEvaluateInt("UNKNOWN(1)", -1); v != -1 {
		t.Fatalf("unknown function should fail")
	}
}

// TestEngineToString 节点文本输出。
func TestEngineToString(t *testing.T) {
	fn, err := alg.ParseFunction("1+2*3")
	if err != nil {
		t.Fatal(err)
	}
	if s := alg.ToString(fn); s != "1 + (2 * 3)" {
		t.Fatalf("ToString = %q", s)
	}
}
