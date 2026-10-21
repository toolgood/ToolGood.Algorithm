package datetimes

import (
	"strings"
	"testing"
	"time"

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

func dtDate(y, m, d int) litNode { return litNode{CreateMyDate(NewMyDate(y, m, d, 0, 0, 0))} }
func dtStr(v string) litNode     { return litNode{CreateString(v)} }
func dtNum(v string) litNode     { return litNode{CreateDecimal(MustDecimal(v))} }

func dtEval(name string, args ...Function) Operand {
	fn := internals.GetFunction(name, args)
	if fn == nil {
		panic("function not registered: " + name)
	}
	return fn.Evaluate(internals.NewAlgorithmEngine(true), func(string) Operand { return None })
}

func TestDateTimes(t *testing.T) {
	cases := []struct {
		name string
		args []Function
		want string
	}{
		{"ADDDAYS", []Function{dtDate(2020, 1, 31), dtNum("1")}, "2020-02-01 00:00:00"},
		{"ADDMONTHS", []Function{dtDate(2020, 1, 31), dtNum("1")}, "2020-02-29 00:00:00"},
		{"ADDYEARS", []Function{dtDate(2020, 2, 29), dtNum("1")}, "2021-02-28 00:00:00"},
		{"ADDHOURS", []Function{dtDate(2020, 1, 1), dtNum("25")}, "2020-01-02 01:00:00"},
		{"ADDMINUTES", []Function{dtDate(2020, 1, 1), dtNum("90")}, "2020-01-01 01:30:00"},
		{"ADDSECONDS", []Function{dtDate(2020, 1, 1), dtNum("90")}, "2020-01-01 00:01:30"},
		{"DATE", []Function{dtNum("2020"), dtNum("13"), dtNum("1")}, "2021-01-01 00:00:00"},
		{"DATE", []Function{dtNum("2020"), dtNum("2"), dtNum("30")}, "2020-03-01 00:00:00"},
		{"DATEDIF", []Function{dtDate(2020, 1, 1), dtDate(2021, 3, 15), dtStr("Y")}, "1"},
		{"DATEDIF", []Function{dtDate(2020, 1, 1), dtDate(2021, 3, 15), dtStr("M")}, "14"},
		{"DATEDIF", []Function{dtDate(2020, 1, 1), dtDate(2021, 3, 15), dtStr("D")}, "439"},
		{"DATEDIF", []Function{dtDate(2020, 1, 1), dtDate(2021, 3, 15), dtStr("YM")}, "2"},
		{"DATEDIF", []Function{dtDate(2020, 1, 1), dtDate(2021, 3, 15), dtStr("MD")}, "14"},
		{"DATEDIF", []Function{dtDate(2020, 3, 15), dtDate(2021, 1, 10), dtStr("YD")}, "301"},
		{"DAYS", []Function{dtDate(2020, 3, 1), dtDate(2020, 1, 1)}, "60"},
		{"DAYS360", []Function{dtDate(2020, 1, 31), dtDate(2020, 3, 31)}, "60"},
		{"DAYS360", []Function{dtDate(2020, 1, 31), dtDate(2020, 3, 31), litNode{True}}, "60"},
		{"WEEKDAY", []Function{dtDate(2020, 1, 1)}, "4"},
		{"WEEKDAY", []Function{dtDate(2020, 1, 1), dtNum("2")}, "3"},
		{"WEEKDAY", []Function{dtDate(2020, 1, 1), dtNum("17")}, "4"},
		{"WEEKNUM", []Function{dtDate(2020, 1, 1)}, "1"},
		{"WEEKNUM", []Function{dtDate(2020, 12, 31), dtNum("21")}, "53"},
		{"NETWORKDAYS", []Function{dtDate(2020, 1, 1), dtDate(2020, 1, 31)}, "23"},
		{"NETWORKDAYS", []Function{dtDate(2020, 1, 31), dtDate(2020, 1, 1)}, "-23"},
		{"NETWORKDAYS", []Function{dtDate(2020, 1, 1), dtDate(2020, 1, 31), dtDate(2020, 1, 1)}, "22"},
		{"WORKDAY", []Function{dtDate(2020, 1, 1), dtNum("5")}, "2020-01-08 00:00:00"},
		{"WORKDAY", []Function{dtDate(2020, 1, 1), dtNum("-5")}, "2019-12-25 00:00:00"},
		{"WORKDAY", []Function{dtDate(2020, 1, 1), dtNum("5"), dtDate(2020, 1, 3)}, "2020-01-09 00:00:00"},
		{"EDATE", []Function{dtDate(2020, 1, 31), dtNum("1")}, "2020-02-29 00:00:00"},
		{"EOMONTH", []Function{dtDate(2020, 1, 31), dtNum("0")}, "2020-01-31 00:00:00"},
		{"EOMONTH", []Function{dtDate(2020, 1, 15), dtNum("1")}, "2020-02-29 00:00:00"},
		{"EOMONTH", []Function{dtDate(2020, 3, 15), dtNum("-1")}, "2020-02-29 00:00:00"},
		{"YEARFRAC", []Function{dtDate(2020, 1, 1), dtDate(2020, 7, 1), dtNum("0")}, "0.5"},
		{"YEARFRAC", []Function{dtDate(2020, 1, 1), dtDate(2020, 7, 1), dtNum("1")}, "0.4972677595628415300546448087431694"},
		{"YEARFRAC", []Function{dtDate(2020, 1, 1), dtDate(2020, 7, 1), dtNum("3")}, "0.4986301369863013698630136986301370"},
		{"TIME", []Function{dtNum("12"), dtNum("30"), dtNum("15")}, "12:30:15"},
		{"TIMEVALUE", []Function{dtStr("12:30:15")}, "12:30:15"},
		{"TIMEVALUE", []Function{dtStr("-1:30:00")}, "-1:-30:00"},
		{"DATEVALUE", []Function{dtStr("2020-05-06")}, "2020-05-06 00:00:00"},
		{"DATEVALUE", []Function{dtNum("43831")}, "2020-01-01 00:00:00"},
		{"DATEVALUE", []Function{dtNum("43831"), dtNum("2")}, "2020-01-01 00:00:00"},
		{"DATEVALUE", []Function{dtNum("1577836800")}, "2020-01-01 08:00:00"},
		{"DATEVALUE", []Function{dtNum("1577836800000")}, "2020-01-01 08:00:00"},
		{"YEAR", []Function{dtDate(2020, 5, 6)}, "2020"},
		{"MONTH", []Function{dtDate(2020, 5, 6)}, "5"},
		{"DAY", []Function{dtDate(2020, 5, 6)}, "6"},
		{"HOUR", []Function{litNode{CreateMyDate(NewMyDate(0, 0, 0, 7, 8, 9))}}, "7"},
		{"MINUTE", []Function{litNode{CreateMyDate(NewMyDate(0, 0, 0, 7, 8, 9))}}, "8"},
		{"SECOND", []Function{litNode{CreateMyDate(NewMyDate(0, 0, 0, 7, 8, 9))}}, "9"},
	}
	for _, c := range cases {
		got := dtEval(c.name, c.args...)
		if g := got.String(); g != `"`+c.want+`"` && g != c.want {
			t.Errorf("%s: got %s want %q", c.name, g, c.want)
		}
	}
}

func TestTimestamp(t *testing.T) {
	ts := dtEval("TIMESTAMP", dtDate(2020, 1, 1))
	// 默认 UseLocalTime=true，本地零点换算成 UTC 会偏移时区
	want := time.Date(2020, 1, 1, 0, 0, 0, 0, time.Local).UnixMilli()
	if ts.String() != DecFromInt(want).String() {
		t.Errorf("TIMESTAMP: got %s want %d", ts.String(), want)
	}

	fn := internals.GetFunction("TIMESTAMP", []Function{dtDate(2020, 1, 1)})
	e := internals.NewAlgorithmEngine(true)
	e.UseLocalTime = false
	utc := fn.Evaluate(e, func(string) Operand { return None })
	if utc.String() != "1577836800000" {
		t.Errorf("TIMESTAMP utc: got %s", utc.String())
	}
}
