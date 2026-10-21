package test

import (
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// getParamTypes 等价 C# AlgorithmEngineHelper.ParseFormula(exp).GetParameterTypes(new AlgorithmEngine())。
func getParamTypes(t *testing.T, exp string) []alg.ParameterType {
	t.Helper()
	list := alg.GetParameterTypes(exp, newEngine())
	assertNotNull(t, list)
	return list
}

// assertParamType 断言参数类型。
func assertParamType(t *testing.T, want, got alg.OperandType) {
	t.Helper()
	if want != got {
		t.Fatalf("want %s, got %s", want.String(), got.String())
	}
}

// TestParameterTypeTest 对应 C# ToolGood.Algorithm.Test.ParameterTypes.ParameterTypeTest。
func TestParameterTypeTest(t *testing.T) {
	// ---------------------------------------------------------- 基础测试
	t.Run("Test_Basic_NumberParameter", func(t *testing.T) {
		list := getParamTypes(t, "test+1")
		assertInt(t, 1, len(list))
		assertText(t, "test", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Basic_BooleanParameter", func(t *testing.T) {
		list := getParamTypes(t, "if(test,2,3)")
		assertInt(t, 1, len(list))
		assertText(t, "test", list[0].Name)
		assertParamType(t, alg.OperandTypeBoolean, list[0].Type)
	})

	t.Run("Test_Basic_TextParameter", func(t *testing.T) {
		list := getParamTypes(t, "left(test,ww)")
		assertInt(t, 2, len(list))
		assertText(t, "test", list[0].Name)
		assertText(t, "ww", list[1].Name)
		assertParamType(t, alg.OperandTypeText, list[0].Type)
	})

	// ---------------------------------------------------------- 比较操作符测试
	t.Run("Test_Compare_Equal_Number", func(t *testing.T) {
		list := getParamTypes(t, "if(test=1,2,3)")
		assertInt(t, 1, len(list))
		assertText(t, "test", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertText(t, "==", list[0].Operator)
		assertText(t, "1", list[0].Value)
	})

	t.Run("Test_Compare_Equal_Text", func(t *testing.T) {
		list := getParamTypes(t, "if(test='123tt',2,3)")
		assertInt(t, 1, len(list))
		assertText(t, "test", list[0].Name)
		assertParamType(t, alg.OperandTypeText, list[0].Type)
		assertText(t, "==", list[0].Operator)
		assertText(t, "123tt", list[0].Value)
	})

	t.Run("Test_Compare_GreaterThanOrEqual", func(t *testing.T) {
		list := getParamTypes(t, "if(test>=1,2,3)")
		assertInt(t, 1, len(list))
		assertText(t, "test", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertText(t, ">=", list[0].Operator)
		assertText(t, "1", list[0].Value)
	})

	t.Run("Test_Compare_GreaterThanOrEqual_Expression", func(t *testing.T) {
		list := getParamTypes(t, "if(test>=1+1,2,3)")
		assertInt(t, 1, len(list))
		assertText(t, "test", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertText(t, ">=", list[0].Operator)
		assertText(t, "2", list[0].Value)
	})

	t.Run("Test_Compare_LessThan", func(t *testing.T) {
		list := getParamTypes(t, "if(test<100,1,0)")
		assertInt(t, 1, len(list))
		assertText(t, "test", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertText(t, "<", list[0].Operator)
		assertText(t, "100", list[0].Value)
	})

	t.Run("Test_Compare_LessThanOrEqual", func(t *testing.T) {
		list := getParamTypes(t, "if(test<=50,1,0)")
		assertInt(t, 1, len(list))
		assertText(t, "test", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertText(t, "<=", list[0].Operator)
		assertText(t, "50", list[0].Value)
	})

	t.Run("Test_Compare_GreaterThan", func(t *testing.T) {
		list := getParamTypes(t, "if(test>0,1,0)")
		assertInt(t, 1, len(list))
		assertText(t, "test", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertText(t, ">", list[0].Operator)
		assertText(t, "0", list[0].Value)
	})

	t.Run("Test_Compare_NotEqual", func(t *testing.T) {
		list := getParamTypes(t, "if(test!=0,1,0)")
		assertInt(t, 1, len(list))
		assertText(t, "test", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertText(t, "!=", list[0].Operator)
		assertText(t, "0", list[0].Value)
	})

	// ---------------------------------------------------------- 逻辑运算符测试
	t.Run("Test_Logic_And", func(t *testing.T) {
		list := getParamTypes(t, "if(a && b,1,0)")
		assertInt(t, 2, len(list))
		assertText(t, "a", list[0].Name)
		assertParamType(t, alg.OperandTypeBoolean, list[0].Type)
		assertText(t, "b", list[1].Name)
		assertParamType(t, alg.OperandTypeBoolean, list[1].Type)
	})

	t.Run("Test_Logic_Or", func(t *testing.T) {
		list := getParamTypes(t, "if(a || b,1,0)")
		assertInt(t, 2, len(list))
		assertText(t, "a", list[0].Name)
		assertParamType(t, alg.OperandTypeBoolean, list[0].Type)
		assertText(t, "b", list[1].Name)
		assertParamType(t, alg.OperandTypeBoolean, list[1].Type)
	})

	t.Run("Test_Logic_Not", func(t *testing.T) {
		list := getParamTypes(t, "if(!a,1,0)")
		assertInt(t, 1, len(list))
		assertText(t, "a", list[0].Name)
		assertParamType(t, alg.OperandTypeBoolean, list[0].Type)
	})

	t.Run("Test_Logic_Complex", func(t *testing.T) {
		list := getParamTypes(t, "if(a && b || c,1,0)")
		assertInt(t, 3, len(list))
		assertParamType(t, alg.OperandTypeBoolean, list[0].Type)
		assertParamType(t, alg.OperandTypeBoolean, list[1].Type)
		assertParamType(t, alg.OperandTypeBoolean, list[2].Type)
	})

	t.Run("Test_Logic_And_Function", func(t *testing.T) {
		list := getParamTypes(t, "and(a,b,c)")
		assertInt(t, 3, len(list))
		assertParamType(t, alg.OperandTypeBoolean, list[0].Type)
		assertParamType(t, alg.OperandTypeBoolean, list[1].Type)
		assertParamType(t, alg.OperandTypeBoolean, list[2].Type)
	})

	t.Run("Test_Logic_Or_Function", func(t *testing.T) {
		list := getParamTypes(t, "or(a,b)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeBoolean, list[0].Type)
		assertParamType(t, alg.OperandTypeBoolean, list[1].Type)
	})

	// ---------------------------------------------------------- 数学运算测试
	t.Run("Test_Math_Add", func(t *testing.T) {
		list := getParamTypes(t, "a+b")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Math_Subtract", func(t *testing.T) {
		list := getParamTypes(t, "a-b")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Math_Multiply", func(t *testing.T) {
		list := getParamTypes(t, "a*b")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Math_Divide", func(t *testing.T) {
		list := getParamTypes(t, "a/b")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Math_Mod", func(t *testing.T) {
		list := getParamTypes(t, "a%b")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Math_ABS", func(t *testing.T) {
		list := getParamTypes(t, "abs(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Math_SQRT", func(t *testing.T) {
		list := getParamTypes(t, "sqrt(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Math_ROUND", func(t *testing.T) {
		list := getParamTypes(t, "round(a,b)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Math_CEILING", func(t *testing.T) {
		list := getParamTypes(t, "ceiling(a,0.5)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Math_FLOOR", func(t *testing.T) {
		list := getParamTypes(t, "floor(a,1)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Math_POWER", func(t *testing.T) {
		list := getParamTypes(t, "power(a,b)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Math_SUM", func(t *testing.T) {
		list := getParamTypes(t, "sum(a,b,c)")
		assertInt(t, 3, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
		assertParamType(t, alg.OperandTypeNumber, list[2].Type)
	})

	t.Run("Test_Math_MAX", func(t *testing.T) {
		list := getParamTypes(t, "max(a,b,c)")
		assertInt(t, 3, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Math_MIN", func(t *testing.T) {
		list := getParamTypes(t, "min(a,b)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Math_AVERAGE", func(t *testing.T) {
		list := getParamTypes(t, "average(a,b,c)")
		assertInt(t, 3, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	// ---------------------------------------------------------- 字符串函数测试
	t.Run("Test_String_LEFT", func(t *testing.T) {
		list := getParamTypes(t, "left(a,n)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeText, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_String_RIGHT", func(t *testing.T) {
		list := getParamTypes(t, "right(a,n)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeText, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_String_MID", func(t *testing.T) {
		list := getParamTypes(t, "mid(a,p1,p2)")
		assertInt(t, 3, len(list))
		assertParamType(t, alg.OperandTypeText, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
		assertParamType(t, alg.OperandTypeNumber, list[2].Type)
	})

	t.Run("Test_String_LEN", func(t *testing.T) {
		list := getParamTypes(t, "len(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeText, list[0].Type)
	})

	t.Run("Test_String_UPPER", func(t *testing.T) {
		list := getParamTypes(t, "upper(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeText, list[0].Type)
	})

	t.Run("Test_String_LOWER", func(t *testing.T) {
		list := getParamTypes(t, "lower(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeText, list[0].Type)
	})

	t.Run("Test_String_TRIM", func(t *testing.T) {
		list := getParamTypes(t, "trim(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeText, list[0].Type)
	})

	t.Run("Test_String_REPLACE", func(t *testing.T) {
		list := getParamTypes(t, "replace(a,p1,p2,p3)")
		assertInt(t, 4, len(list))
		assertParamType(t, alg.OperandTypeText, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
		assertParamType(t, alg.OperandTypeNumber, list[2].Type)
	})

	t.Run("Test_String_SUBSTITUTE", func(t *testing.T) {
		list := getParamTypes(t, "substitute(a,'old','new')")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeText, list[0].Type)
	})

	t.Run("Test_String_FIND", func(t *testing.T) {
		list := getParamTypes(t, "find('test',a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeText, list[0].Type)
	})

	t.Run("Test_String_CONCATENATE", func(t *testing.T) {
		list := getParamTypes(t, "concatenate(a,b,c)")
		assertInt(t, 3, len(list))
		assertParamType(t, alg.OperandTypeText, list[0].Type)
	})

	t.Run("Test_String_CONNECT", func(t *testing.T) {
		list := getParamTypes(t, "a & b")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeText, list[0].Type)
		assertParamType(t, alg.OperandTypeText, list[1].Type)
	})

	// ---------------------------------------------------------- 日期函数测试
	t.Run("Test_Date_DATE", func(t *testing.T) {
		list := getParamTypes(t, "date(yr,mon,dy)")
		assertInt(t, 3, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
		assertParamType(t, alg.OperandTypeNumber, list[2].Type)
	})

	t.Run("Test_Date_DATE_WithTime", func(t *testing.T) {
		list := getParamTypes(t, "date(yr,mon,dy,hr,m1,s1)")
		assertInt(t, 6, len(list))
		for i := 0; i < 6; i++ {
			assertParamType(t, alg.OperandTypeNumber, list[i].Type)
		}
	})

	t.Run("Test_Date_YEAR", func(t *testing.T) {
		list := getParamTypes(t, "year(dt)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeDate, list[0].Type)
	})

	t.Run("Test_Date_MONTH", func(t *testing.T) {
		list := getParamTypes(t, "month(dt)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeDate, list[0].Type)
	})

	t.Run("Test_Date_DAY", func(t *testing.T) {
		list := getParamTypes(t, "day(dt)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeDate, list[0].Type)
	})

	t.Run("Test_Date_HOUR", func(t *testing.T) {
		list := getParamTypes(t, "hour(dt)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeDate, list[0].Type)
	})

	t.Run("Test_Date_MINUTE", func(t *testing.T) {
		list := getParamTypes(t, "minute(dt)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeDate, list[0].Type)
	})

	t.Run("Test_Date_SECOND", func(t *testing.T) {
		list := getParamTypes(t, "second(dt)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeDate, list[0].Type)
	})

	t.Run("Test_Date_ADDDAYS", func(t *testing.T) {
		list := getParamTypes(t, "adddays(dt,num)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeDate, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Date_DATEDIF", func(t *testing.T) {
		list := getParamTypes(t, "datedif(startDt,endDt,'d')")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeDate, list[0].Type)
		assertParamType(t, alg.OperandTypeDate, list[1].Type)
	})

	// ---------------------------------------------------------- 类型检查函数测试
	t.Run("Test_TypeCheck_ISNUMBER", func(t *testing.T) {
		list := getParamTypes(t, "isnumber(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNone, list[0].Type)
	})

	t.Run("Test_TypeCheck_ISTEXT", func(t *testing.T) {
		list := getParamTypes(t, "istext(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNone, list[0].Type)
	})

	t.Run("Test_TypeCheck_ISNULL", func(t *testing.T) {
		list := getParamTypes(t, "isnull(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNone, list[0].Type)
	})

	t.Run("Test_TypeCheck_ISNULLOREMPTY", func(t *testing.T) {
		list := getParamTypes(t, "isnullorempty(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNone, list[0].Type)
	})

	t.Run("Test_TypeCheck_ISERROR", func(t *testing.T) {
		list := getParamTypes(t, "iserror(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNone, list[0].Type)
	})

	t.Run("Test_TypeCheck_ISEVEN", func(t *testing.T) {
		list := getParamTypes(t, "iseven(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_TypeCheck_ISODD", func(t *testing.T) {
		list := getParamTypes(t, "isodd(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_TypeCheck_ISLOGICAL", func(t *testing.T) {
		list := getParamTypes(t, "islogical(a)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNone, list[0].Type)
	})

	// ---------------------------------------------------------- 流程控制函数测试
	t.Run("Test_Flow_IF_WithCondition", func(t *testing.T) {
		list := getParamTypes(t, "if(score>=60,'Pass','Fail')")
		assertInt(t, 1, len(list))
		assertText(t, "score", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertText(t, ">=", list[0].Operator)
		assertText(t, "60", list[0].Value)
	})

	t.Run("Test_Flow_IFS", func(t *testing.T) {
		list := getParamTypes(t, "ifs(a=1,'One',a=2,'Two',a=3,'Three')")
		assertInt(t, 3, len(list))
		assertText(t, "a", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Flow_IFERROR", func(t *testing.T) {
		list := getParamTypes(t, "iferror(a/b,0)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Flow_SWITCH", func(t *testing.T) {
		list := getParamTypes(t, "switch(a,b,c)")
		assertInt(t, 3, len(list))
		assertText(t, "a", list[0].Name)
		assertParamType(t, alg.OperandTypeNone, list[0].Type)
	})

	// ---------------------------------------------------------- 条件统计函数测试
	t.Run("Test_Conditional_SUMIF", func(t *testing.T) {
		list := getParamTypes(t, "sumif(values,'>10')")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeArray, list[0].Type)
	})

	t.Run("Test_Conditional_SUMIF_WithSumRange", func(t *testing.T) {
		list := getParamTypes(t, "sumif(range,'>10',sumRange)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeArray, list[0].Type)
		assertParamType(t, alg.OperandTypeArray, list[1].Type)
	})

	t.Run("Test_Conditional_COUNTIF", func(t *testing.T) {
		list := getParamTypes(t, "countif(values,'>5')")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeArray, list[0].Type)
	})

	t.Run("Test_Conditional_AVERAGEIF", func(t *testing.T) {
		list := getParamTypes(t, "averageif(values,'>0')")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeArray, list[0].Type)
	})

	// ---------------------------------------------------------- 三角函数测试
	t.Run("Test_Trig_SIN", func(t *testing.T) {
		list := getParamTypes(t, "sin(angle)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Trig_COS", func(t *testing.T) {
		list := getParamTypes(t, "cos(angle)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Trig_TAN", func(t *testing.T) {
		list := getParamTypes(t, "tan(angle)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Trig_ASIN", func(t *testing.T) {
		list := getParamTypes(t, "asin(val)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Trig_ACOS", func(t *testing.T) {
		list := getParamTypes(t, "acos(val)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Trig_ATAN2", func(t *testing.T) {
		list := getParamTypes(t, "atan2(y,x)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Trig_DEGREES", func(t *testing.T) {
		list := getParamTypes(t, "degrees(rad)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Trig_RADIANS", func(t *testing.T) {
		list := getParamTypes(t, "radians(deg)")
		assertInt(t, 1, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	// ---------------------------------------------------------- 进制转换函数测试
	t.Run("Test_Conversion_BIN2DEC", func(t *testing.T) {
		list := getParamTypes(t, "bin2dec(binary)")
		assertInt(t, 1, len(list))
	})

	t.Run("Test_Conversion_DEC2BIN", func(t *testing.T) {
		list := getParamTypes(t, "dec2bin(decimal)")
		assertInt(t, 1, len(list))
	})

	t.Run("Test_Conversion_HEX2DEC", func(t *testing.T) {
		list := getParamTypes(t, "hex2dec(hex)")
		assertInt(t, 1, len(list))
	})

	// ---------------------------------------------------------- 复杂表达式测试
	t.Run("Test_Complex_NestedIF", func(t *testing.T) {
		list := getParamTypes(t, "if(a>100,'High',if(a>50,'Medium','Low'))")
		assertInt(t, 2, len(list))
		assertText(t, "a", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Complex_MultipleParameters", func(t *testing.T) {
		list := getParamTypes(t, "(a+b)*(c-d)/e()")
		assertInt(t, 4, len(list))
		for _, p := range list {
			assertParamType(t, alg.OperandTypeNumber, p.Type)
		}
	})

	t.Run("Test_Complex_MixedTypes", func(t *testing.T) {
		list := getParamTypes(t, "if(amount>1000, 'High: ' & name, 'Low')")
		assertInt(t, 2, len(list))
		assertText(t, "amount", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertText(t, "name", list[1].Name)
		assertParamType(t, alg.OperandTypeText, list[1].Type)
	})

	t.Run("Test_Complex_DateCalculation", func(t *testing.T) {
		list := getParamTypes(t, "adddays(startDate, duration)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeDate, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Complex_ScoreGrade", func(t *testing.T) {
		list := getParamTypes(t, "ifs(score>=90,'A',score>=80,'B',score>=70,'C',score>=60,'D',true,'F')")
		assertInt(t, 4, len(list))
		assertText(t, "score", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	t.Run("Test_Complex_StringManipulation", func(t *testing.T) {
		list := getParamTypes(t, "upper(left(name,1)) & lower(mid(name,2,len(name)-1))")
		assertInt(t, 3, len(list))
		assertText(t, "name", list[0].Name)
		assertParamType(t, alg.OperandTypeText, list[0].Type)
	})

	t.Run("Test_Complex_ConditionalSum", func(t *testing.T) {
		list := getParamTypes(t, "sumif(amounts,'>0')/countif(amounts,'>0')")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeArray, list[0].Type)
	})

	t.Run("Test_Complex_BooleanExpression", func(t *testing.T) {
		list := getParamTypes(t, "if((age>=18 && age<=65) || isVIP, 'Eligible', 'Not Eligible')")
		assertInt(t, 3, len(list))
		assertText(t, "age", list[0].Name)
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertText(t, "isVIP", list[2].Name)
		assertParamType(t, alg.OperandTypeBoolean, list[2].Type)
	})

	// ---------------------------------------------------------- 金融函数测试
	t.Run("Test_Financial_PMT", func(t *testing.T) {
		list := getParamTypes(t, "pmt(arg1,arg2,arg3)")
		assertInt(t, 3, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
		assertParamType(t, alg.OperandTypeNumber, list[2].Type)
	})

	t.Run("Test_Financial_PV", func(t *testing.T) {
		list := getParamTypes(t, "pv(arg1,arg2,arg3)")
		assertInt(t, 3, len(list))
		for _, p := range list {
			assertParamType(t, alg.OperandTypeNumber, p.Type)
		}
	})

	t.Run("Test_Financial_FV", func(t *testing.T) {
		list := getParamTypes(t, "fv(arg1,arg2,arg3)")
		assertInt(t, 3, len(list))
		for _, p := range list {
			assertParamType(t, alg.OperandTypeNumber, p.Type)
		}
	})

	t.Run("Test_Financial_NPV", func(t *testing.T) {
		list := getParamTypes(t, "npv(arg1,arg2)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
	})

	// ---------------------------------------------------------- 数组函数测试
	t.Run("Test_Array_Array", func(t *testing.T) {
		list := getParamTypes(t, "array(a,b,c)")
		assertInt(t, 3, len(list))
	})

	t.Run("Test_Array_Large", func(t *testing.T) {
		list := getParamTypes(t, "large(arr,k)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeArray, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Array_Small", func(t *testing.T) {
		list := getParamTypes(t, "small(arr,k)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeArray, list[0].Type)
		assertParamType(t, alg.OperandTypeNumber, list[1].Type)
	})

	t.Run("Test_Array_Rank", func(t *testing.T) {
		list := getParamTypes(t, "rank(num,ref)")
		assertInt(t, 2, len(list))
		assertParamType(t, alg.OperandTypeNumber, list[0].Type)
		assertParamType(t, alg.OperandTypeArray, list[1].Type)
	})
}
