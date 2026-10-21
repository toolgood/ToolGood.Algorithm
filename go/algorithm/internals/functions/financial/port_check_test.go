package financial

import (
	"strings"
	"testing"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"
)

// litNode 测试用的字面量节点。
type litNode struct{ v Operand }

func (l litNode) Name() string                                     { return "lit" }
func (l litNode) Evaluate(*AlgorithmEngine, TempParameter) Operand { return l.v }
func (l litNode) ResultType() OperandType                          { return l.v.Type() }
func (l litNode) GetParameterTypes(*AlgorithmEngine, *[]ParameterType, OperandType, ...string) {
}
func (l litNode) WriteTo(sb *strings.Builder, addBrackets bool) { sb.WriteString(l.v.String()) }
func (l litNode) String() string                                { return l.v.String() }

func fNum(v string) litNode { return litNode{CreateDecimal(MustDecimal(v))} }

func fArr(v ...string) litNode {
	items := make([]Operand, len(v))
	for i, s := range v {
		items[i] = CreateDecimal(MustDecimal(s))
	}
	return litNode{CreateArray(items)}
}

func fDateArr(v ...string) litNode {
	items := make([]Operand, len(v))
	for i, s := range v {
		items[i] = CreateMyDate(ParseMyDate(s))
	}
	return litNode{CreateArray(items)}
}

func fEval(name string, args ...Function) Operand {
	fn := internals.GetFunction(name, args)
	if fn == nil {
		panic("function not registered: " + name)
	}
	return fn.Evaluate(internals.NewAlgorithmEngine(true), func(string) Operand { return None })
}

func TestFinancial(t *testing.T) {
	// 期望值由高精度 decimal 按 C# 公式独立计算得出（前若干位比较）
	cases := []struct {
		name string
		args []Function
		want string
	}{
		{"FV", []Function{fNum("0.005"), fNum("10"), fNum("-200"), fNum("-500"), fNum("1")},
			"2581.4033740601791537250068359375"},
		{"PV", []Function{fNum("0.006666666666666666666666666666666667"), fNum("240"), fNum("-1000")},
			"119554.291702376043633616077687927"},
		{"PMT", []Function{fNum("0.006666666666666666666666666666666667"), fNum("10"), fNum("10000")},
			"-1037.032089359152175652094829324"},
		{"IPMT", []Function{fNum("0.01"), fNum("1"), fNum("10"), fNum("1000")}, "-10"},
		{"PPMT", []Function{fNum("0.01"), fNum("1"), fNum("10"), fNum("1000")},
			"-95.58207655117136118232419513"},
		{"NPER", []Function{fNum("0.01"), fNum("-100"), fNum("1000")},
			"10.5886444594232359951882052688"},
		{"NPER", []Function{fNum("0"), fNum("-100"), fNum("1000")}, "10"},
		{"SLN", []Function{fNum("30000"), fNum("7500"), fNum("10")}, "2250"},
		{"SYD", []Function{fNum("30000"), fNum("7500"), fNum("10"), fNum("1")},
			"4090.909090909090909090909090909091"},
		{"DB", []Function{fNum("1000000"), fNum("100000"), fNum("6"), fNum("1"), fNum("7")},
			"186083.3333333333333333333333333333"},
		{"DDB", []Function{fNum("2400"), fNum("300"), fNum("10"), fNum("1")}, "480"},
		{"NPV", []Function{fNum("0.1"), fNum("-10000"), fNum("3000"), fNum("4200"), fNum("6800")},
			"1188.4434123352230038931766955809"},
		{"IRR", []Function{fArr("-100", "30", "40", "50")},
			"0.088963394693349935317765679643477"},
		{"XNPV", []Function{fNum("0.09"), fArr("-10000", "2750", "4250", "3250", "2750"),
			fDateArr("2008-01-01", "2008-03-01", "2008-10-30", "2009-02-15", "2009-04-01")},
			"2086.647602031536621663610094314144"},
		{"XIRR", []Function{fArr("-10000", "2750", "4250", "3250", "2750"),
			fDateArr("2008-01-01", "2008-03-01", "2008-10-30", "2009-02-15", "2009-04-01")},
			"0.3733625335188315103083924482521883"},
		{"MIRR", []Function{fArr("-1000", "300", "400", "500"), fNum("0.1"), fNum("0.12")},
			"0.0981566924463154084121919922477953"},
	}
	for _, c := range cases {
		got := fEval(c.name, c.args...)
		if g := got.String(); !strings.HasPrefix(g, c.want) {
			t.Errorf("%s: got %s want prefix %s", c.name, g, c.want)
		}
	}

	// RATE 收敛到约 0（每期 100，共 10 期，现值 1000）
	r := fEval("RATE", fNum("10"), fNum("-100"), fNum("1000"))
	if r.NumberValue().Abs().Greater(MustDecimal("0.0000001")) {
		t.Errorf("RATE: got %s", r.String())
	}
}

func TestFinancialErrors(t *testing.T) {
	cases := []struct {
		name string
		args []Function
		want string
	}{
		{"PMT", []Function{fNum("0.01"), fNum("0"), fNum("1000")}, "ERROR: Function 'PMT' Div 0 error!"},
		{"IPMT", []Function{fNum("0.01"), fNum("20"), fNum("10"), fNum("1000")}, "ERROR: Function 'IPMT' parameter 2 is error!"},
		{"DDB", []Function{fNum("2400"), fNum("300"), fNum("10"), fNum("1"), fNum("0")}, "ERROR: Function 'DDB' parameter 5 is error!"},
		{"DB", []Function{fNum("1000"), fNum("100"), fNum("6"), fNum("1"), fNum("13")}, "ERROR: Function 'DB' parameter 5 is error!"},
		{"IRR", []Function{fArr("1", "2")}, "ERROR: Function 'IRR' parameter 1 is error!"},
		{"XIRR", []Function{fArr("-1", "2"), fDateArr("2020-01-01")}, "ERROR: Function 'XIRR' parameter is error!"},
		{"SLN", []Function{fNum("100"), fNum("10"), fNum("0")}, "ERROR: Function 'SLN' Div 0 error!"},
		{"SYD", []Function{fNum("100"), fNum("10"), fNum("5"), fNum("6")}, "ERROR: Function 'SYD' parameter 4 is error!"},
		{"XNPV", []Function{fNum("-1"), fArr("100"), fDateArr("2020-01-01")}, "ERROR: Function 'XNPV' Div 0 error!"},
	}
	for _, c := range cases {
		if got := fEval(c.name, c.args...); got.String() != c.want {
			t.Errorf("%s: got %s want %s", c.name, got.String(), c.want)
		}
	}
}
