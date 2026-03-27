using PetaTest;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Test.ParameterTypes
{
	[TestFixture]
	public class ParameterTypeTest
	{
		#region 基础测试
		[Test]
		public void Test_Basic_NumberParameter()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("test+1");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Basic_BooleanParameter()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(test,2,3)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[0].Type);
		}

		[Test]
		public void Test_Basic_TextParameter()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("left(test,ww)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual("ww", list[1].Name);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
		}
		#endregion

		#region 比较操作符测试
		[Test]
		public void Test_Compare_Equal_Number()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(test=1,2,3)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual("==", list[0].Operator);
			Assert.AreEqual("1", list[0].Value);
		}

		[Test]
		public void Test_Compare_Equal_Text()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(test='123tt',2,3)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
			Assert.AreEqual("==", list[0].Operator);
			Assert.AreEqual("123tt", list[0].Value);
		}

		[Test]
		public void Test_Compare_GreaterThanOrEqual()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(test>=1,2,3)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(">=", list[0].Operator);
			Assert.AreEqual("1", list[0].Value);
		}

		[Test]
		public void Test_Compare_GreaterThanOrEqual_Expression()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(test>=1+1,2,3)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(">=", list[0].Operator);
			Assert.AreEqual("2", list[0].Value);
		}

		[Test]
		public void Test_Compare_LessThan()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(test<100,1,0)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual("<", list[0].Operator);
			Assert.AreEqual("100", list[0].Value);
		}

		[Test]
		public void Test_Compare_LessThanOrEqual()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(test<=50,1,0)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual("<=", list[0].Operator);
			Assert.AreEqual("50", list[0].Value);
		}

		[Test]
		public void Test_Compare_GreaterThan()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(test>0,1,0)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(">", list[0].Operator);
			Assert.AreEqual("0", list[0].Value);
		}

		[Test]
		public void Test_Compare_NotEqual()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(test!=0,1,0)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual("!=", list[0].Operator);
			Assert.AreEqual("0", list[0].Value);
		}
		#endregion

		#region 逻辑运算符测试
		[Test]
		public void Test_Logic_And()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(a && b,1,0)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("a", list[0].Name);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[0].Type);
			Assert.AreEqual("b", list[1].Name);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[1].Type);
		}

		[Test]
		public void Test_Logic_Or()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(a || b,1,0)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("a", list[0].Name);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[0].Type);
			Assert.AreEqual("b", list[1].Name);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[1].Type);
		}

		[Test]
		public void Test_Logic_Not()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(!a,1,0)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("a", list[0].Name);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[0].Type);
		}

		[Test]
		public void Test_Logic_Complex()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(a && b || c,1,0)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[0].Type);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[1].Type);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[2].Type);
		}

		[Test]
		public void Test_Logic_And_Function()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("and(a,b,c)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[0].Type);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[1].Type);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[2].Type);
		}

		[Test]
		public void Test_Logic_Or_Function()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("or(a,b)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[0].Type);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[1].Type);
		}
		#endregion

		#region 数学运算测试
		[Test]
		public void Test_Math_Add()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("a+b");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Math_Subtract()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("a-b");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Math_Multiply()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("a*b");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Math_Divide()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("a/b");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Math_Mod()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("a%b");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Math_ABS()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("abs(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Math_SQRT()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("sqrt(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Math_ROUND()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("round(a,b)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Math_CEILING()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("ceiling(a,0.5)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Math_FLOOR()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("floor(a,1)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Math_POWER()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("power(a,b)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Math_SUM()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("sum(a,b,c)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[2].Type);
		}

		[Test]
		public void Test_Math_MAX()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("max(a,b,c)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Math_MIN()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("min(a,b)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Math_AVERAGE()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("average(a,b,c)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}
		#endregion

		#region 字符串函数测试
		[Test]
		public void Test_String_LEFT()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("left(a,n)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_String_RIGHT()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("right(a,n)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_String_MID()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("mid(a,p1,p2)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[2].Type);
		}

		[Test]
		public void Test_String_LEN()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("len(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
		}

		[Test]
		public void Test_String_UPPER()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("upper(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
		}

		[Test]
		public void Test_String_LOWER()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("lower(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
		}

		[Test]
		public void Test_String_TRIM()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("trim(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
		}

		[Test]
		public void Test_String_REPLACE()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("replace(a,p1,p2,p3)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(4, list.Count);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[2].Type);
		}

		[Test]
		public void Test_String_SUBSTITUTE()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("substitute(a,'old','new')");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
		}

		[Test]
		public void Test_String_FIND()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("find('test',a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
		}

		[Test]
		public void Test_String_CONCATENATE()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("concatenate(a,b,c)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
		}

		[Test]
		public void Test_String_CONNECT()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("a & b");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
			Assert.AreEqual(Enums.OperandType.TEXT, list[1].Type);
		}
		#endregion

		#region 日期函数测试
		[Test]
		public void Test_Date_DATE()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("date(yr,mon,dy)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[2].Type);
		}

		[Test]
		public void Test_Date_DATE_WithTime()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("date(yr,mon,dy,hr,m1,s1)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(6, list.Count);
			for(int i = 0; i < 6; i++) {
				Assert.AreEqual(Enums.OperandType.NUMBER, list[i].Type);
			}
		}

		[Test]
		public void Test_Date_YEAR()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("year(dt)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.DATE, list[0].Type);
		}

		[Test]
		public void Test_Date_MONTH()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("month(dt)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.DATE, list[0].Type);
		}

		[Test]
		public void Test_Date_DAY()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("day(dt)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.DATE, list[0].Type);
		}

		[Test]
		public void Test_Date_HOUR()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("hour(dt)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.DATE, list[0].Type);
		}

		[Test]
		public void Test_Date_MINUTE()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("minute(dt)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.DATE, list[0].Type);
		}

		[Test]
		public void Test_Date_SECOND()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("second(dt)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.DATE, list[0].Type);
		}

		[Test]
		public void Test_Date_ADDDAYS()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("adddays(dt,num)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.DATE, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Date_DATEDIF()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("datedif(startDt,endDt,'d')");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.DATE, list[0].Type);
			Assert.AreEqual(Enums.OperandType.DATE, list[1].Type);
		}
		#endregion

		#region 类型检查函数测试
		[Test]
		public void Test_TypeCheck_ISNUMBER()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("isnumber(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NONE, list[0].Type);
		}

		[Test]
		public void Test_TypeCheck_ISTEXT()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("istext(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NONE, list[0].Type);
		}

		[Test]
		public void Test_TypeCheck_ISNULL()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("isnull(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NONE, list[0].Type);
		}

		[Test]
		public void Test_TypeCheck_ISNULLOREMPTY()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("isnullorempty(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NONE, list[0].Type);
		}

		[Test]
		public void Test_TypeCheck_ISERROR()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("iserror(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NONE, list[0].Type);
		}

		[Test]
		public void Test_TypeCheck_ISEVEN()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("iseven(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_TypeCheck_ISODD()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("isodd(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_TypeCheck_ISLOGICAL()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("islogical(a)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NONE, list[0].Type);
		}
		#endregion

		#region 流程控制函数测试
		[Test]
		public void Test_Flow_IF_WithCondition()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(score>=60,'Pass','Fail')");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("score", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(">=", list[0].Operator);
			Assert.AreEqual("60", list[0].Value);
		}

		[Test]
		public void Test_Flow_IFS()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("ifs(a=1,'One',a=2,'Two',a=3,'Three')");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual("a", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Flow_IFERROR()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("iferror(a/b,0)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Flow_SWITCH()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("switch(a,b,c)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual("a", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NONE, list[0].Type);
		}
		#endregion

		#region 条件统计函数测试
		[Test]
		public void Test_Conditional_SUMIF()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("sumif(values,'>10')");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.ARRAY, list[0].Type);
		}

		[Test]
		public void Test_Conditional_SUMIF_WithSumRange()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("sumif(range,'>10',sumRange)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.ARRAY, list[0].Type);
			Assert.AreEqual(Enums.OperandType.ARRAY, list[1].Type);
		}

		[Test]
		public void Test_Conditional_COUNTIF()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("countif(values,'>5')");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.ARRAY, list[0].Type);
		}

		[Test]
		public void Test_Conditional_AVERAGEIF()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("averageif(values,'>0')");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.ARRAY, list[0].Type);
		}
		#endregion

		#region 三角函数测试
		[Test]
		public void Test_Trig_SIN()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("sin(angle)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Trig_COS()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("cos(angle)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Trig_TAN()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("tan(angle)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Trig_ASIN()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("asin(val)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Trig_ACOS()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("acos(val)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Trig_ATAN2()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("atan2(y,x)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Trig_DEGREES()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("degrees(rad)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Trig_RADIANS()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("radians(deg)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}
		#endregion

		#region 进制转换函数测试
		[Test]
		public void Test_Conversion_BIN2DEC()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("bin2dec(binary)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
		}

		[Test]
		public void Test_Conversion_DEC2BIN()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("dec2bin(decimal)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
		}

		[Test]
		public void Test_Conversion_HEX2DEC()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("hex2dec(hex)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
		}
		#endregion

		#region 复杂表达式测试
		[Test]
		public void Test_Complex_NestedIF()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(a>100,'High',if(a>50,'Medium','Low'))");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("a", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Complex_MultipleParameters()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("(a+b)*(c-d)/e()");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(4, list.Count);
			foreach(var p in list) {
				Assert.AreEqual(Enums.OperandType.NUMBER, p.Type);
			}
		}

		[Test]
		public void Test_Complex_MixedTypes()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if(amount>1000, 'High: ' & name, 'Low')");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("amount", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual("name", list[1].Name);
			Assert.AreEqual(Enums.OperandType.TEXT, list[1].Type);
		}

		[Test]
		public void Test_Complex_DateCalculation()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("adddays(startDate, duration)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.DATE, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Complex_ScoreGrade()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("ifs(score>=90,'A',score>=80,'B',score>=70,'C',score>=60,'D',true,'F')");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(4, list.Count);
			Assert.AreEqual("score", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}

		[Test]
		public void Test_Complex_StringManipulation()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("upper(left(name,1)) & lower(mid(name,2,len(name)-1))");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual("name", list[0].Name);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
		}

		[Test]
		public void Test_Complex_ConditionalSum()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("sumif(amounts,'>0')/countif(amounts,'>0')");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.ARRAY, list[0].Type);
		}

		[Test]
		public void Test_Complex_BooleanExpression()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("if((age>=18 && age<=65) || isVIP, 'Eligible', 'Not Eligible')");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual("age", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual("isVIP", list[2].Name);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[2].Type);
		}
		#endregion

		#region 金融函数测试
		[Test]
		public void Test_Financial_PMT()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("pmt(arg1,arg2,arg3)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[2].Type);
		}

		[Test]
		public void Test_Financial_PV()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("pv(arg1,arg2,arg3)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			foreach(var p in list) {
				Assert.AreEqual(Enums.OperandType.NUMBER, p.Type);
			}
		}

		[Test]
		public void Test_Financial_FV()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("fv(arg1,arg2,arg3)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
			foreach(var p in list) {
				Assert.AreEqual(Enums.OperandType.NUMBER, p.Type);
			}
		}

		[Test]
		public void Test_Financial_NPV()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("npv(arg1,arg2)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
		}
		#endregion

		#region 数组函数测试
		[Test]
		public void Test_Array_Array()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("array(a,b,c)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(3, list.Count);
		}

		[Test]
		public void Test_Array_Large()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("large(arr,k)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.ARRAY, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Array_Small()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("small(arr,k)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.ARRAY, list[0].Type);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[1].Type);
		}

		[Test]
		public void Test_Array_Rank()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("rank(num,ref)");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(Enums.OperandType.ARRAY, list[1].Type);
		}
		#endregion
	}
}
