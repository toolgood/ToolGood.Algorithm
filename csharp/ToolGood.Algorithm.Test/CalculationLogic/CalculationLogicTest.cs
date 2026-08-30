using PetaTest;
using System;
using System.Globalization;
using ToolGood.Algorithm;
using ToolGood.Algorithm.CalculationLogic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Test.CalculationLogic
{
	[TestFixture]
	public class CalculationLogicTest
	{
		private CultureInfo _originalCulture;

		[SetUp]
		public void SetUp()
		{
			// Program.cs 将当前线程设置为 fr-FR 文化，引擎中 value.ToString() 会输出 "1,5"。
			// 这里固定为 InvariantCulture，使断言不依赖运行环境。
			_originalCulture = CultureInfo.CurrentCulture;
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
		}

		[TearDown]
		public void TearDown()
		{
			CultureInfo.CurrentCulture = _originalCulture;
		}

		private static CalculationLogicEngine CreateEngine(bool useCalculationLogicInfo = true)
		{
			return new CalculationLogicEngine(new FunctionCache(), useCalculationLogicInfo);
		}

		private static string NormalizeLine(string text)
		{
			return text.Replace("\r\n", "\n");
		}

		#region SetSceneName 测试

		[Test]
		public void SetSceneName_Test()
		{
			var logic = CreateEngine();
			logic.SetSceneName("场景1");
			Assert.AreEqual("===== 场景1 =====\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void SetSceneName_NoInfo_Test()
		{
			var logic = CreateEngine(false);
			logic.SetSceneName("场景1");
			Assert.AreEqual("", logic.ToInfoString());
		}

		#endregion

		#region InitValue 测试

		[Test]
		public void InitValue_Int_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("a", 1);
			logic.InitValue("b", 2);
			Assert.AreEqual("[初始] a=1;b=2;\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void InitValue_String_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("s", "hello");
			Assert.AreEqual("[初始] s=hello;\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void InitValue_Decimal_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("d", 1.5m);
			Assert.AreEqual("[初始] d=1.5;\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void InitValue_Double_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("dd", 1.5);
			Assert.AreEqual("[初始] dd=1.5;\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void InitValue_Bool_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("flag", true);
			Assert.AreEqual("[初始] flag=True;\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void InitValue_LayerRemark_Test()
		{
			// InitValue 的层级与备注不参与信息输出
			var logic = CreateEngine();
			logic.InitValue("i", 100, 2, "备注");
			Assert.AreEqual("[初始] i=100;\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void InitValue_CanBeUsedInCondition_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("a", 1);
			logic.InitValue("b", 2);
			Assert.IsTrue(logic.CheckCondition("a<b"));
			Assert.IsFalse(logic.CheckCondition("a>b"));
		}

		#endregion

		#region CheckCondition 测试

		[Test]
		public void CheckCondition_True_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("a", 1);
			logic.InitValue("b", 2);
			Assert.IsTrue(logic.CheckCondition("a<b"));
			Assert.AreEqual("[初始] a=1;b=2;\n[成功] if a<b: // 1<2\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void CheckCondition_False_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("a", 1);
			logic.InitValue("b", 2);
			Assert.IsFalse(logic.CheckCondition("a>b"));
			Assert.AreEqual("[初始] a=1;b=2;\n[失败] if a>b: // 1>2\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void CheckCondition_LayerRemark_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("a", 1);
			logic.InitValue("b", 2);
			Assert.IsTrue(logic.CheckCondition("a<b", 1, "备注"));
			Assert.AreEqual("[初始] a=1;b=2;\n[成功]    if a<b: // 1<2 // 备注\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void CheckCondition_StringCompare_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("s", "abc");
			Assert.IsTrue(logic.CheckCondition("s='abc'"));
			Assert.IsFalse(logic.CheckCondition("s='ab'"));
			Assert.AreEqual("[初始] s=abc;\n[成功] if s='abc': // \"abc\"='abc'\n[失败] if s='ab': // \"abc\"='ab'\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void CheckCondition_NonBoolean_Throws_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("a", 1);
			logic.InitValue("b", 2);
			var ex = Assert.Throws<FormatException>(() => logic.CheckCondition("a+b"));
			Assert.AreEqual("The condition must be a boolean value!", ex.Message);
			Assert.AreEqual("[初始] a=1;b=2;\n[条件][错误] if a+b: // 1+2 = The condition must be a boolean value!\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void CheckCondition_Error_Throws_Test()
		{
			var logic = CreateEngine();
			var ex = Assert.Throws<FormatException>(() => logic.CheckCondition("1/0=0"));
			Assert.AreEqual("Function '/' Div 0 error!", ex.Message);
			Assert.AreEqual("[条件][错误] if 1/0=0: // 1/0=0 = Function '/' Div 0 error!\n", NormalizeLine(logic.ToInfoString()));
		}

		#endregion

		#region SetFormula 测试

		[Test]
		public void SetFormula_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("a", 1);
			logic.InitValue("b", 2);
			logic.SetFormula("x", "a*3+b", 1, "备注");
			Assert.AreEqual("[初始] a=1;b=2;\n[赋值]    x = a*3+b = 1*3+2 = 5 // 备注\n", NormalizeLine(logic.ToInfoString()));
			// 公式结果应已写入参数
			Assert.IsTrue(logic.CheckCondition("x=5"));
		}

		[Test]
		public void SetFormula_StringConcat_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("s", "hello");
			logic.SetFormula("y", "s&'!'");
			Assert.AreEqual("[初始] s=hello;\n[赋值] y = s&'!' = \"hello\"&'!' = hello!\n", NormalizeLine(logic.ToInfoString()));
			Assert.IsTrue(logic.CheckCondition("y='hello!'"));
		}

		[Test]
		public void SetFormula_TextAndNumber_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("a", 1);
			logic.SetFormula("z", "'text'&a");
			Assert.AreEqual("[初始] a=1;\n[赋值] z = 'text'&a = 'text'&1 = text1\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void SetFormula_Error_Throws_Test()
		{
			var logic = CreateEngine();
			var ex = Assert.Throws<FormatException>(() => logic.SetFormula("x", "1/0"));
			Assert.AreEqual("Function '/' Div 0 error!", ex.Message);
			Assert.AreEqual("[赋值][错误] x = 1/0 = 1/0 // Function '/' Div 0 error!\n", NormalizeLine(logic.ToInfoString()));
		}

		#endregion

		#region SetValue 测试

		[Test]
		public void SetValue_String_Test()
		{
			var logic = CreateEngine();
			logic.SetValue("s", "文本");
			Assert.AreEqual("[赋值] s = \"文本\"\n", NormalizeLine(logic.ToInfoString()));
			Assert.IsTrue(logic.CheckCondition("s='文本'"));
		}

		[Test]
		public void SetValue_StringEscape_Test()
		{
			var logic = CreateEngine();
			logic.SetValue("sq", "a\"b");
			Assert.AreEqual("[赋值] sq = \"a\\\"b\"\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void SetValue_Decimal_Test()
		{
			var logic = CreateEngine();
			logic.SetValue("d", 1.5m);
			Assert.AreEqual("[赋值] d = 1.5\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void SetValue_Double_Test()
		{
			var logic = CreateEngine();
			logic.SetValue("dd", 1.5);
			Assert.AreEqual("[赋值] dd = 1.5\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void SetValue_Bool_Test()
		{
			var logic = CreateEngine();
			logic.SetValue("flag", true);
			Assert.AreEqual("[赋值] flag = True\n", NormalizeLine(logic.ToInfoString()));
		}

		[Test]
		public void SetValue_LayerRemark_Test()
		{
			var logic = CreateEngine();
			logic.SetValue("i", 100, 1, "备注");
			Assert.AreEqual("[赋值]    i = 100 // 备注\n", NormalizeLine(logic.ToInfoString()));
		}

		#endregion

		#region GetValue 测试

		[Test]
		public void GetValue_InitValue_Int_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("a", 1);
			var operand = logic.GetValue("a");
			Assert.IsTrue(operand.IsNumber);
			Assert.AreEqual(OperandType.NUMBER, operand.Type);
			Assert.AreEqual(1m, operand.NumberValue);
		}

		[Test]
		public void GetValue_InitValue_String_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("s", "hello");
			var operand = logic.GetValue("s");
			Assert.IsTrue(operand.IsText);
			Assert.AreEqual(OperandType.TEXT, operand.Type);
			Assert.AreEqual("hello", operand.TextValue);
		}

		[Test]
		public void GetValue_InitValue_Decimal_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("d", 1.5m);
			var operand = logic.GetValue("d");
			Assert.IsTrue(operand.IsNumber);
			Assert.AreEqual(1.5m, operand.NumberValue);
		}

		[Test]
		public void GetValue_InitValue_Double_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("dd", 1.5);
			var operand = logic.GetValue("dd");
			Assert.IsTrue(operand.IsNumber);
			Assert.AreEqual(1.5m, operand.NumberValue);
		}

		[Test]
		public void GetValue_InitValue_Bool_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("flag", true);
			var operand = logic.GetValue("flag");
			Assert.IsTrue(operand.IsBoolean);
			Assert.AreEqual(OperandType.BOOLEAN, operand.Type);
			Assert.IsTrue(operand.BooleanValue);
		}

		[Test]
		public void GetValue_SetValue_Test()
		{
			var logic = CreateEngine();
			logic.SetValue("v", "text");
			var operand = logic.GetValue("v");
			Assert.IsTrue(operand.IsText);
			Assert.AreEqual("text", operand.TextValue);
		}

		[Test]
		public void GetValue_SetFormula_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("a", 1);
			logic.InitValue("b", 2);
			logic.SetFormula("x", "a*3+b");
			var operand = logic.GetValue("x");
			Assert.IsTrue(operand.IsNumber);
			Assert.AreEqual(5m, operand.NumberValue);
		}

		[Test]
		public void GetValue_AfterSetFormulaOverwrite_Test()
		{
			var logic = CreateEngine();
			logic.InitValue("a", 1);
			logic.SetFormula("a", "a+10");
			var operand = logic.GetValue("a");
			Assert.IsTrue(operand.IsNumber);
			Assert.AreEqual(11m, operand.NumberValue);
		}

		[Test]
		public void GetValue_NotFound_ReturnsError_Test()
		{
			var logic = CreateEngine();
			var operand = logic.GetValue("missing");
			Assert.IsTrue(operand.IsError);
			Assert.AreEqual("Parameter [missing] is missing.", operand.ErrorMsg);
		}

		[Test]
		public void GetValue_NoInfo_Test()
		{
			// 关闭信息记录不影响取值
			var logic = CreateEngine(false);
			logic.InitValue("a", 1);
			Assert.AreEqual(1m, logic.GetValue("a").NumberValue);
		}

		#endregion

		#region BlankLine 测试

		[Test]
		public void BlankLine_Test()
		{
			var logic = CreateEngine();
			logic.BlankLine();
			Assert.AreEqual("\n", NormalizeLine(logic.ToInfoString()));

			logic.BlankLine();
			logic.BlankLine();
			Assert.AreEqual("\n\n\n", NormalizeLine(logic.ToInfoString()));
		}

		#endregion

		#region ToInfoString 测试

		[Test]
		public void ToInfoString_Empty_Test()
		{
			var logic = CreateEngine();
			Assert.AreEqual("", logic.ToInfoString());
		}

		[Test]
		public void ToInfoString_Disabled_Test()
		{
			// 关闭信息记录后，所有操作都不产生信息
			var logic = CreateEngine(false);
			logic.SetSceneName("场景1");
			logic.InitValue("a", 1);
			logic.CheckCondition("a>0");
			logic.SetValue("b", 2);
			logic.SetFormula("c", "a+1");
			logic.BlankLine();
			Assert.AreEqual("", logic.ToInfoString());
		}

		[Test]
		public void ToInfoString_FullFlow_Test()
		{
			var logic = CreateEngine();
			logic.SetSceneName("场景1");
			logic.InitValue("a", 1);
			logic.InitValue("b", 2);
			if(logic.CheckCondition("a>b")) {
				logic.SetValue("c", 3, 1);
				logic.SetFormula("c", "a*5+b", 1);
			} else if(logic.CheckCondition("a<b")) {
				logic.SetValue("c", 4, 1);
				logic.SetFormula("c", "a*3+b", 1);
			}
			logic.BlankLine();
			logic.SetValue("e", 5);

			var expected = "[初始] a=1;b=2;\n"
				+ "===== 场景1 =====\n"
				+ "[失败] if a>b: // 1>2\n"
				+ "[成功] if a<b: // 1<2\n"
				+ "[赋值]    c = 4\n"
				+ "[赋值]    c = a*3+b = 1*3+2 = 5\n"
				+ "\n"
				+ "[赋值] e = 5\n";
			Assert.AreEqual(expected, NormalizeLine(logic.ToInfoString()));
		}

		#endregion

	}
}
