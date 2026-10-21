package test

import (
	"testing"
)

// TestCompareTest 对应 C# ToolGood.Algorithm.Test.CompareTest。
func TestCompareTest(t *testing.T) {
	t.Run("base_test", func(t *testing.T) {
		e := newEngine()
		assertBool(t, true, evalBool(e, "1=1"))
		assertBool(t, false, evalBool(e, "1=2"))
		assertBool(t, true, evalBool(e, "1<>2"))
		assertBool(t, true, evalBool(e, "1!=2"))
		assertBool(t, false, evalBool(e, "1>2"))
		assertBool(t, true, evalBool(e, "1<2"))
		assertBool(t, true, evalBool(e, "1<=2"))
		assertBool(t, false, evalBool(e, "1>=2"))

		assertBool(t, true, evalBool(e, "'1'='1'"))
		assertBool(t, true, evalBool(e, "'e'='e'"))
		assertBool(t, false, evalBool(e, "'1'='2'"))
		assertBool(t, false, evalBool(e, "'我'='你'"))
		assertBool(t, true, evalBool(e, "'1'!='2'"))
	})

	t.Run("strict_equality_test", func(t *testing.T) {
		e := newEngine()
		assertBool(t, true, evalBool(e, "1===1"))
		assertBool(t, false, evalBool(e, "1===2"))
		assertBool(t, true, evalBool(e, "'1'==='1'"))
		assertBool(t, false, evalBool(e, "'1'==='2'"))
		assertBool(t, true, evalBool(e, "1!==2"))
		assertBool(t, false, evalBool(e, "1!==1"))
		assertBool(t, true, evalBool(e, "'1'!=='2'"))
		assertBool(t, false, evalBool(e, "'1'!=='1'"))
	})

	t.Run("null_compare_test", func(t *testing.T) {
		e := newEngine()

		assertBool(t, false, evalBool(e, "1>null"))
		assertBool(t, false, evalBool(e, "1>=null"))
		assertBool(t, false, evalBool(e, "1<=null"))
		assertBool(t, false, evalBool(e, "1<null"))
		assertBool(t, false, evalBool(e, "1==null"))
		assertBool(t, true, evalBool(e, "1!=null"))

		// 两侧同为 null 属于异常数据，返回错误，TryEvaluate 返回默认值
		e = newEngine()
		assertBool(t, false, evalBoolDef(e, "null=null", false))
		assertLastError(t, e)

		e = newEngine()
		assertBool(t, true, evalBoolDef(e, "null!=null", true))
		assertLastError(t, e)

		e = newEngine()
		assertBool(t, false, evalBool(e, "'111'=null"))
		assertBool(t, true, evalBool(e, "'111'!=null"))
	})

	t.Run("null_same_type_compare_test", func(t *testing.T) {
		// 两侧同为 null 属于异常数据，所有比较运算符均返回错误，TryEvaluate 返回默认值
		e := newEngine()
		assertBool(t, true, evalBoolDef(e, "null>null", true))
		assertLastError(t, e)

		e = newEngine()
		assertBool(t, true, evalBoolDef(e, "null>=null", true))
		assertLastError(t, e)

		e = newEngine()
		assertBool(t, true, evalBoolDef(e, "null<=null", true))
		assertLastError(t, e)

		e = newEngine()
		assertBool(t, true, evalBoolDef(e, "null<null", true))
		assertLastError(t, e)
	})

	t.Run("negative_compare_test", func(t *testing.T) {
		e := newEngine()
		assertBool(t, true, evalBool(e, "1 > (-2)"))
		assertBool(t, true, evalBool(e, "(-1) > (-2）"))
		assertBool(t, true, evalBool(e, "-1 > (-2)"))
		assertBool(t, true, evalBool(e, "-1 > -2"))
		assertBool(t, true, evalBool(e, "-7 < -2"))
		// YES/NO 是内置布尔关键字（对应 C# 词法层的 YES/NO）
		assertBool(t, true, evalBool(e, "-7*Yes < -2"))
		assertBool(t, true, evalBool(e, "-7*No > -2"))
	})
}
