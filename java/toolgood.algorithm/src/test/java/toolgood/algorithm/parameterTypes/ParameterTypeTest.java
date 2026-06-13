package toolgood.algorithm.parametertypes;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngineHelper;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.ParameterType;

import java.util.List;

public class ParameterTypeTest
{
	//region 基础测试
	@Test
	public void Test_Basic_NumberParameter() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("test+1");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals("test", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Basic_BooleanParameter() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(test,2,3)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals("test", list.get(0).Name);
		assertEquals(OperandType.BOOLEAN, list.get(0).Type);
	}

	@Test
	public void Test_Basic_TextParameter() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("left(test,ww)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals("test", list.get(0).Name);
		assertEquals("ww", list.get(1).Name);
		assertEquals(OperandType.TEXT, list.get(0).Type);
	}
	//endregion

	//region 比较操作符测试
	@Test
	public void Test_Compare_Equal_Number() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(test=1,2,3)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals("test", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals("==", list.get(0).Operator);
		assertEquals("1", list.get(0).Value);
	}

	@Test
	public void Test_Compare_Equal_Text() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(test='123tt',2,3)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals("test", list.get(0).Name);
		assertEquals(OperandType.TEXT, list.get(0).Type);
		assertEquals("==", list.get(0).Operator);
		assertEquals("123tt", list.get(0).Value);
	}

	@Test
	public void Test_Compare_GreaterThanOrEqual() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(test>=1,2,3)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals("test", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(">=", list.get(0).Operator);
		assertEquals("1", list.get(0).Value);
	}

	@Test
	public void Test_Compare_GreaterThanOrEqual_Expression() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(test>=1+1,2,3)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals("test", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(">=", list.get(0).Operator);
		assertEquals("2", list.get(0).Value);
	}

	@Test
	public void Test_Compare_LessThan() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(test<100,1,0)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals("test", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals("<", list.get(0).Operator);
		assertEquals("100", list.get(0).Value);
	}

	@Test
	public void Test_Compare_LessThanOrEqual() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(test<=50,1,0)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals("test", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals("<=", list.get(0).Operator);
		assertEquals("50", list.get(0).Value);
	}

	@Test
	public void Test_Compare_GreaterThan() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(test>0,1,0)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals("test", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(">", list.get(0).Operator);
		assertEquals("0", list.get(0).Value);
	}

	@Test
	public void Test_Compare_NotEqual() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(test!=0,1,0)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals("test", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals("!=", list.get(0).Operator);
		assertEquals("0", list.get(0).Value);
	}
	//endregion

	//region 逻辑运算符测试
	@Test
	public void Test_Logic_And() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(a && b,1,0)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals("a", list.get(0).Name);
		assertEquals(OperandType.BOOLEAN, list.get(0).Type);
		assertEquals("b", list.get(1).Name);
		assertEquals(OperandType.BOOLEAN, list.get(1).Type);
	}

	@Test
	public void Test_Logic_Or() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(a || b,1,0)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals("a", list.get(0).Name);
		assertEquals(OperandType.BOOLEAN, list.get(0).Type);
		assertEquals("b", list.get(1).Name);
		assertEquals(OperandType.BOOLEAN, list.get(1).Type);
	}

	@Test
	public void Test_Logic_Not() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(!a,1,0)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals("a", list.get(0).Name);
		assertEquals(OperandType.BOOLEAN, list.get(0).Type);
	}

	@Test
	public void Test_Logic_Complex() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(a && b || c,1,0)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals(OperandType.BOOLEAN, list.get(0).Type);
		assertEquals(OperandType.BOOLEAN, list.get(1).Type);
		assertEquals(OperandType.BOOLEAN, list.get(2).Type);
	}

	@Test
	public void Test_Logic_And_Function() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("and(a,b,c)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals(OperandType.BOOLEAN, list.get(0).Type);
		assertEquals(OperandType.BOOLEAN, list.get(1).Type);
		assertEquals(OperandType.BOOLEAN, list.get(2).Type);
	}

	@Test
	public void Test_Logic_Or_Function() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("or(a,b)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.BOOLEAN, list.get(0).Type);
		assertEquals(OperandType.BOOLEAN, list.get(1).Type);
	}
	//endregion

	//region 数学运算测试
	@Test
	public void Test_Math_Add() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("a+b");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Math_Subtract() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("a-b");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Math_Multiply() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("a*b");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Math_Divide() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("a/b");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Math_Mod() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("a%b");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Math_ABS() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("abs(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Math_SQRT() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("sqrt(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Math_ROUND() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("round(a,b)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Math_CEILING() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("ceiling(a,0.5)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Math_FLOOR() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("floor(a,1)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Math_POWER() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("power(a,b)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Math_SUM() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("sum(a,b,c)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
		assertEquals(OperandType.NUMBER, list.get(2).Type);
	}

	@Test
	public void Test_Math_MAX() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("max(a,b,c)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Math_MIN() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("min(a,b)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Math_AVERAGE() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("average(a,b,c)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}
	//endregion

	//region 字符串函数测试
	@Test
	public void Test_String_LEFT() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("left(a,n)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.TEXT, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_String_RIGHT() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("right(a,n)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.TEXT, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_String_MID() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("mid(a,p1,p2)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals(OperandType.TEXT, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
		assertEquals(OperandType.NUMBER, list.get(2).Type);
	}

	@Test
	public void Test_String_LEN() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("len(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.TEXT, list.get(0).Type);
	}

	@Test
	public void Test_String_UPPER() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("upper(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.TEXT, list.get(0).Type);
	}

	@Test
	public void Test_String_LOWER() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("lower(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.TEXT, list.get(0).Type);
	}

	@Test
	public void Test_String_TRIM() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("trim(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.TEXT, list.get(0).Type);
	}

	@Test
	public void Test_String_REPLACE() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("replace(a,p1,p2,p3)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(4, list.size());
		assertEquals(OperandType.TEXT, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
		assertEquals(OperandType.NUMBER, list.get(2).Type);
	}

	@Test
	public void Test_String_SUBSTITUTE() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("substitute(a,'old','new')");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.TEXT, list.get(0).Type);
	}

	@Test
	public void Test_String_FIND() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("find('test',a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.TEXT, list.get(0).Type);
	}

	@Test
	public void Test_String_CONCATENATE() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("concatenate(a,b,c)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals(OperandType.TEXT, list.get(0).Type);
	}

	@Test
	public void Test_String_CONNECT() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("a & b");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.TEXT, list.get(0).Type);
		assertEquals(OperandType.TEXT, list.get(1).Type);
	}
	//endregion

	//region 日期函数测试
	@Test
	public void Test_Date_DATE() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("date(yr,mon,dy)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
		assertEquals(OperandType.NUMBER, list.get(2).Type);
	}

	@Test
	public void Test_Date_DATE_WithTime() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("date(yr,mon,dy,hr,m1,s1)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(6, list.size());
		for(int i = 0; i < 6; i++) {
			assertEquals(OperandType.NUMBER, list.get(i).Type);
		}
	}

	@Test
	public void Test_Date_YEAR() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("year(dt)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.DATE, list.get(0).Type);
	}

	@Test
	public void Test_Date_MONTH() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("month(dt)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.DATE, list.get(0).Type);
	}

	@Test
	public void Test_Date_DAY() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("day(dt)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.DATE, list.get(0).Type);
	}

	@Test
	public void Test_Date_HOUR() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("hour(dt)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.DATE, list.get(0).Type);
	}

	@Test
	public void Test_Date_MINUTE() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("minute(dt)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.DATE, list.get(0).Type);
	}

	@Test
	public void Test_Date_SECOND() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("second(dt)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.DATE, list.get(0).Type);
	}

	@Test
	public void Test_Date_ADDDAYS() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("adddays(dt,num)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.DATE, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Date_DATEDIF() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("datedif(startDt,endDt,'d')");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.DATE, list.get(0).Type);
		assertEquals(OperandType.DATE, list.get(1).Type);
	}
	//endregion

	//region 类型检查函数测试
	@Test
	public void Test_TypeCheck_ISNUMBER() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("isnumber(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NONE, list.get(0).Type);
	}

	@Test
	public void Test_TypeCheck_ISTEXT() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("istext(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NONE, list.get(0).Type);
	}

	@Test
	public void Test_TypeCheck_ISNULL() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("isnull(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NONE, list.get(0).Type);
	}

	@Test
	public void Test_TypeCheck_ISNULLOREMPTY() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("isnullorempty(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NONE, list.get(0).Type);
	}

	@Test
	public void Test_TypeCheck_ISERROR() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("iserror(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NONE, list.get(0).Type);
	}

	@Test
	public void Test_TypeCheck_ISEVEN() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("iseven(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_TypeCheck_ISODD() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("isodd(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_TypeCheck_ISLOGICAL() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("islogical(a)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NONE, list.get(0).Type);
	}
	//endregion

	//region 流程控制函数测试
	@Test
	public void Test_Flow_IF_WithCondition() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(score>=60,'Pass','Fail')");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals("score", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(">=", list.get(0).Operator);
		assertEquals("60", list.get(0).Value);
	}

	@Test
	public void Test_Flow_IFS() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("ifs(a=1,'One',a=2,'Two',a=3,'Three')");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals("a", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Flow_IFERROR() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("iferror(a/b,0)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Flow_SWITCH() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("switch(a,b,c)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals("a", list.get(0).Name);
		assertEquals(OperandType.NONE, list.get(0).Type);
	}
	//endregion

	//region 条件统计函数测试
	@Test
	public void Test_Conditional_SUMIF() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("sumif(values,'>10')");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.ARRAY, list.get(0).Type);
	}

	@Test
	public void Test_Conditional_SUMIF_WithSumRange() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("sumif(range,'>10',sumRange)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.ARRAY, list.get(0).Type);
		assertEquals(OperandType.ARRAY, list.get(1).Type);
	}

	@Test
	public void Test_Conditional_COUNTIF() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("countif(values,'>5')");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.ARRAY, list.get(0).Type);
	}

	@Test
	public void Test_Conditional_AVERAGEIF() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("averageif(values,'>0')");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.ARRAY, list.get(0).Type);
	}
	//endregion

	//region 三角函数测试
	@Test
	public void Test_Trig_SIN() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("sin(angle)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Trig_COS() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("cos(angle)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Trig_TAN() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("tan(angle)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Trig_ASIN() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("asin(val)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Trig_ACOS() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("acos(val)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Trig_ATAN2() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("atan2(y,x)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Trig_DEGREES() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("degrees(rad)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Trig_RADIANS() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("radians(deg)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}
	//endregion

	//region 进制转换函数测试
	@Test
	public void Test_Conversion_BIN2DEC() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("bin2dec(binary)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
	}

	@Test
	public void Test_Conversion_DEC2BIN() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("dec2bin(decimal)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
	}

	@Test
	public void Test_Conversion_HEX2DEC() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("hex2dec(hex)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(1, list.size());
	}
	//endregion

	//region 复杂表达式测试
	@Test
	public void Test_Complex_NestedIF() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(a>100,'High',if(a>50,'Medium','Low'))");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals("a", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Complex_MultipleParameters() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("(a+b)*(c-d)/e()");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(4, list.size());
		for(ParameterType p : list) {
			assertEquals(OperandType.NUMBER, p.Type);
		}
	}

	@Test
	public void Test_Complex_MixedTypes() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if(amount>1000, 'High: ' & name, 'Low')");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals("amount", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals("name", list.get(1).Name);
		assertEquals(OperandType.TEXT, list.get(1).Type);
	}

	@Test
	public void Test_Complex_DateCalculation() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("adddays(startDate, duration)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.DATE, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Complex_ScoreGrade() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("ifs(score>=90,'A',score>=80,'B',score>=70,'C',score>=60,'D',true,'F')");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(4, list.size());
		assertEquals("score", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}

	@Test
	public void Test_Complex_StringManipulation() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("upper(left(name,1)) & lower(mid(name,2,len(name)-1))");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals("name", list.get(0).Name);
		assertEquals(OperandType.TEXT, list.get(0).Type);
	}

	@Test
	public void Test_Complex_ConditionalSum() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("sumif(amounts,'>0')/countif(amounts,'>0')");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.ARRAY, list.get(0).Type);
	}

	@Test
	public void Test_Complex_BooleanExpression() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("if((age>=18 && age<=65) || isVIP, 'Eligible', 'Not Eligible')");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals("age", list.get(0).Name);
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals("isVIP", list.get(2).Name);
		assertEquals(OperandType.BOOLEAN, list.get(2).Type);
	}
	//endregion

	//region 金融函数测试
	@Test
	public void Test_Financial_PMT() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("pmt(arg1,arg2,arg3)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
		assertEquals(OperandType.NUMBER, list.get(2).Type);
	}

	@Test
	public void Test_Financial_PV() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("pv(arg1,arg2,arg3)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		for(ParameterType p : list) {
			assertEquals(OperandType.NUMBER, p.Type);
		}
	}

	@Test
	public void Test_Financial_FV() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("fv(arg1,arg2,arg3)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
		for(ParameterType p : list) {
			assertEquals(OperandType.NUMBER, p.Type);
		}
	}

	@Test
	public void Test_Financial_NPV() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("npv(arg1,arg2)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
	}
	//endregion

	//region 数组函数测试
	@Test
	public void Test_Array_Array() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("array(a,b,c)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(3, list.size());
	}

	@Test
	public void Test_Array_Large() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("large(arr,k)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.ARRAY, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Array_Small() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("small(arr,k)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.ARRAY, list.get(0).Type);
		assertEquals(OperandType.NUMBER, list.get(1).Type);
	}

	@Test
	public void Test_Array_Rank() throws Exception
	{
		FunctionBase fb = AlgorithmEngineHelper.ParseFormula("rank(num,ref)");
		List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());
		assertNotNull(list);
		assertEquals(2, list.size());
		assertEquals(OperandType.NUMBER, list.get(0).Type);
		assertEquals(OperandType.ARRAY, list.get(1).Type);
	}
	//endregion
}
