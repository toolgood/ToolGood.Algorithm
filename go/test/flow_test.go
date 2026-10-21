// 本文件移植 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/FlowTest.cs。
package test

import (
	"testing"
)

// TestFlowTest 对应 C# FlowTest 测试类。
func TestFlowTest(t *testing.T) {
	t.Run("If_Test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 1, evalInt(engine, "if(1=1,1,2)"))
		assertInt(t, 1, evalInt(engine, "if(1=1,1)"))
		assertInt(t, 1, evalInt(engine, "if(1=1，1)"))
		assertInt(t, 1, evalInt(engine, "if('1',1,2)"))
		assertInt(t, 2, evalInt(engine, "if(0,1,2)"))
	})

	t.Run("iferror_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 1, evalInt(engine, "iferror(1/0,1,2)"))
		assertInt(t, 1, evalInt(engine, "iferror(1-'rrr',1,2)"))
	})

	t.Run("iserror_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 1, evalInt(engine, "iserror(1/0,1)"))
		assertInt(t, 1, evalInt(engine, "iserror(1-'rrr',1)"))
	})

	t.Run("ifnull_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 1, evalInt(engine, "isnull(null,1)"))
		assertInt(t, 1, evalInt(engine, "isnull(1,2)"))
	})

	t.Run("isnullorerror_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 1, evalInt(engine, "isnullorerror(null,1)"))
		assertInt(t, 1, evalInt(engine, "isnullorerror(1/0,1)"))
		assertInt(t, 1, evalInt(engine, "isnullorerror(1,2)"))
	})

	t.Run("ISNUMBER_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 1, evalInt(engine, "if(ISNUMBER(1),1,2)"))
		assertInt(t, 2, evalInt(engine, "if(ISNUMBER('e'),1,2)"))
		assertInt(t, 2, evalInt(engine, "if(ISNUMBER('11'),1,2)"))
		assertInt(t, 2, evalInt(engine, "if(ISNUMBER('2016-1-2'),1,2)"))
	})

	t.Run("ISTEXT_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 2, evalInt(engine, "if(ISTEXT(1),1,2)"))
		assertInt(t, 1, evalInt(engine, "if(ISTEXT('e'),1,2)"))
		assertInt(t, 1, evalInt(engine, "if(ISTEXT('11'),1,2)"))
		assertInt(t, 1, evalInt(engine, "if(ISTEXT('2016-1-2'),1,2)"))
	})

	t.Run("ISNONTEXT_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 1, evalInt(engine, "if(ISNONTEXT(1),1,2)"))
		assertInt(t, 2, evalInt(engine, "if(ISNONTEXT('e'),1,2)"))
		assertInt(t, 2, evalInt(engine, "if(ISNONTEXT('11'),1,2)"))
		assertInt(t, 2, evalInt(engine, "if(ISNONTEXT('2016-1-2'),1,2)"))
	})

	t.Run("ISLOGICAL_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 2, evalInt(engine, "if(ISLOGICAL(1),1,2)"))
		assertInt(t, 2, evalInt(engine, "if(ISLOGICAL('e'),1,2)"))
		assertInt(t, 1, evalInt(engine, "if(ISLOGICAL(true),1,2)"))
		assertInt(t, 1, evalInt(engine, "if(ISLOGICAL(false),1,2)"))
	})

	t.Run("ISEVEN_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 2, evalInt(engine, "if(ISEVEN(1),1,2)"))
		assertInt(t, 1, evalInt(engine, "if(ISEVEN(2),1,2)"))
		assertInt(t, 2, evalInt(engine, "if(ISEVEN('e'),1,2)"))
	})

	t.Run("ISODD_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 1, evalInt(engine, "if(ISODD(1),1,2)"))
		assertInt(t, 2, evalInt(engine, "if(ISODD(2),1,2)"))
		assertInt(t, 2, evalInt(engine, "if(ISODD('e'),1,2)"))
	})

	t.Run("ISODD_negative_test", func(t *testing.T) {
		engine := newEngine()
		// C# 取模符号跟随被除数,-1%2==-1,修复前负数奇数被误判为偶数
		assertBool(t, true, evalBool(engine, "ISODD(-1)"))
		assertBool(t, false, evalBool(engine, "ISODD(-2)"))
		// 先截断再判断
		assertBool(t, true, evalBool(engine, "ISODD(-1.5)"))
		assertBool(t, false, evalBool(engine, "ISODD(-0.5)"))
	})

	t.Run("And_Test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "and(true(),1=1)"))
		assertBool(t, true, evalBool(engine, "and(true(),1)"))
		assertBool(t, false, evalBoolDef(engine, "and(true(),false(),1=1)", true))
		assertBool(t, false, evalBoolDef(engine, "and(true(),false(),1)", true))
		assertBool(t, false, evalBoolDef(engine, "and(true(),0)", true))
	})

	t.Run("Or_Test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "or(true(),1=1)"))
		assertBool(t, true, evalBool(engine, "or(true(),1)"))
		assertBool(t, true, evalBool(engine, "or(true(),false(),1=1)"))
		assertBool(t, true, evalBool(engine, "or(true(),false(),1)"))
		assertBool(t, true, evalBool(engine, "or(true(),0)"))
		assertBool(t, false, evalBoolDef(engine, "or(false(),1=2)", true))
	})

	t.Run("Not_Test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, false, evalBoolDef(engine, "not(true())", true))
		assertBool(t, true, evalBoolDef(engine, "not(false())", false))
	})

	t.Run("true_Test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "true()"))
	})

	t.Run("false_Test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, false, evalBoolDef(engine, "false()", true))
	})

	t.Run("andor_Test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "1=1 && 2==2 && 3=3"))
		assertBool(t, false, evalBoolDef(engine, "1=1 && 2!=2", true))
		assertBool(t, true, evalBool(engine, "1=1 || 2!=2"))
	})

	t.Run("IFS_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "a", evalText(engine, "IFS(1=1, 'a', 1=2, 'b')"))
		assertText(t, "b", evalText(engine, "IFS(1=2, 'a', 2=2, 'b')"))
		assertInt(t, 100, evalInt(engine, "IFS(1=1, 100, 1=2, 200)"))
	})

	t.Run("SWITCH_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "one", evalText(engine, "SWITCH(1, 1, 'one', 2, 'two')"))
		assertText(t, "two", evalText(engine, "SWITCH(2, 1, 'one', 2, 'two')"))
		assertInt(t, 1, evalInt(engine, "SWITCH('a', 'a', 1, 'b', 2)"))
		assertText(t, "", evalText(engine, "SWITCH(5, 1, 'one', 2, 'two')"))
	})

	t.Run("SWITCH_ExcelCompatible_test", func(t *testing.T) {
		engine := newEngine()
		// 有默认值的情况
		assertText(t, "unknown", evalText(engine, "SWITCH(3, 1, 'one', 2, 'two', 'unknown')"))
		assertText(t, "one", evalText(engine, "SWITCH(1, 1, 'one', 2, 'two', 'unknown')"))

		// 数字类型默认值
		assertInt(t, 0, evalInt(engine, "SWITCH(5, 1, 100, 2, 200, 0)"))

		// 多个 value/result 对 + 默认值
		assertText(t, "D", evalText(engine, "SWITCH(4, 1, 'A', 2, 'B', 3, 'C', 'D')"))
		assertText(t, "C", evalText(engine, "SWITCH(3, 1, 'A', 2, 'B', 3, 'C', 'D')"))

		// 字符串匹配 + 默认值
		assertInt(t, -1, evalInt(engine, "SWITCH('x', 'a', 1, 'b', 2, -1)"))
	})

	t.Run("XOR_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "XOR(true(), false())"))
		assertBool(t, false, evalBoolDef(engine, "XOR(true(), true())", true))
		assertBool(t, false, evalBoolDef(engine, "XOR(false(), false())", true))
		assertBool(t, true, evalBool(engine, "XOR(true(), true(), true())"))
		assertBool(t, true, evalBool(engine, "XOR(true(), false(), false())"))
		assertBool(t, true, evalBool(engine, "XOR(1, 0)"))
		assertBool(t, false, evalBoolDef(engine, "XOR(1, 1)", true))
	})

	t.Run("MethodStyle_ISNUMBER_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "123.ISNUMBER()"))
		assertBool(t, false, evalBoolDef(engine, "'abc'.ISNUMBER()", true))
		assertBool(t, false, evalBoolDef(engine, "true.ISNUMBER()", true))
	})

	t.Run("MethodStyle_ISTEXT_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "'abc'.ISTEXT()"))
		assertBool(t, false, evalBoolDef(engine, "123.ISTEXT()", true))
	})

	t.Run("MethodStyle_ISNONTEXT_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "123.ISNONTEXT()"))
		assertBool(t, false, evalBoolDef(engine, "'abc'.ISNONTEXT()", true))
	})

	t.Run("MethodStyle_ISLOGICAL_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "true.ISLOGICAL()"))
		assertBool(t, true, evalBool(engine, "false.ISLOGICAL()"))
		assertBool(t, false, evalBoolDef(engine, "123.ISLOGICAL()", true))
	})

	t.Run("MethodStyle_ISNULL_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "null.ISNULL()"))
		assertBool(t, false, evalBoolDef(engine, "123.ISNULL()", true))
	})

	t.Run("MethodStyle_ISNULLORERROR_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "null.ISNULLORERROR()"))
	})
}
