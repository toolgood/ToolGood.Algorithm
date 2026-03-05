using PetaTest;
using System;
using System.Collections.Generic;

namespace ToolGood.Algorithm.Test.AlgorithmEngineExTests
{
	[TestFixture]
	public class AlgorithmEngineExTest
	{
		#region 构造函数测试

		[Test]
		public void Constructor_Default_Test()
		{
			var engine = new AlgorithmEngineEx();
			Assert.IsFalse(engine.IgnoreCase);
			Assert.IsFalse(engine.UseTempDict);
		}

		[Test]
		public void Constructor_IgnoreCase_Test()
		{
			var engine = new AlgorithmEngineEx(true);
			Assert.IsTrue(engine.IgnoreCase);

			engine = new AlgorithmEngineEx(false);
			Assert.IsFalse(engine.IgnoreCase);
		}

		#endregion

		#region AddParameter 测试

		[Test]
		public void AddParameter_Bool_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("flag", true);
			var result = engine.TryEvaluate("flag", false);
			Assert.AreEqual(true, result);

			engine.AddParameter("flag", false);
			result = engine.TryEvaluate("flag", true);
			Assert.AreEqual(false, result);
		}

		[Test]
		public void AddParameter_Int_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("num", 42);
			var result = engine.TryEvaluate("num", 0);
			Assert.AreEqual(42, result);
		}

		[Test]
		public void AddParameter_Long_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("num", 123456789L);
			var result = engine.TryEvaluate("num", 0L);
			Assert.AreEqual(123456789L, result);
		}

		[Test]
		public void AddParameter_Double_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("num", 3.14159);
			var result = engine.TryEvaluate("num", 0.0);
			Assert.AreEqual(3.14159, result, 0.00001);
		}

		[Test]
		public void AddParameter_Decimal_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("num", 123.45m);
			var result = engine.TryEvaluate("num", 0m);
			Assert.AreEqual(123.45m, result);
		}

		[Test]
		public void AddParameter_String_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("name", "ToolGood");
			var result = engine.TryEvaluate("name", "");
			Assert.AreEqual("ToolGood", result);
		}

		[Test]
		public void AddParameter_DateTime_Test()
		{
			var engine = new AlgorithmEngineEx();
			var dt = new DateTime(2024, 6, 15, 10, 30, 0);
			engine.AddParameter("dt", dt);
			var result = engine.TryEvaluate("year(dt)", 0);
			Assert.AreEqual(2024, result);
			result = engine.TryEvaluate("month(dt)", 0);
			Assert.AreEqual(6, result);
			result = engine.TryEvaluate("day(dt)", 0);
			Assert.AreEqual(15, result);
		}

		[Test]
		public void AddParameter_TimeSpan_Test()
		{
			var engine = new AlgorithmEngineEx();
			var ts = new TimeSpan(10, 30, 0);
			engine.AddParameter("ti", ts);
			var result = engine.TryEvaluate("hour(ti)", 0);
			Assert.AreEqual(10, result);
			result = engine.TryEvaluate("minute(ti)", 0);
			Assert.AreEqual(30, result);
		}

		[Test]
		public void AddParameter_List_Test()
		{
			var engine = new AlgorithmEngineEx();
			var list = new List<Operand> { Operand.Create(1), Operand.Create(2), Operand.Create(3) };
			engine.AddParameter("arr", list);
			var result = engine.TryEvaluate("count(arr)", 0);
			Assert.AreEqual(3, result);
			result = engine.TryEvaluate("arr[1]", 0);
			Assert.AreEqual(1, result);
		}

		[Test]
		public void AddParameter_Collection_String_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("names", Operand.Create(new List<string> { "a", "b", "c" }));
			var result = engine.TryEvaluate("count(names)", 0);
			Assert.AreEqual(3, result);
		}

		[Test]
		public void AddParameter_Collection_Int_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("nums", Operand.Create(new List<int> { 1, 2, 3, 4, 5 }));
			var result = engine.TryEvaluate("sum(nums)", 0);
			Assert.AreEqual(15, result);
		}

		[Test]
		public void AddParameter_Collection_Double_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("nums", Operand.Create(new List<double> { 1.1, 2.2, 3.3 }));
			var result = engine.TryEvaluate("sum(nums)", 0.0);
			Assert.AreEqual(6.6, result, 0.01);
		}

		[Test]
		public void AddParameter_Collection_Bool_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("flags", Operand.Create(new List<bool> { true, false, true }));
			var result = engine.TryEvaluate("count(flags)", 0);
			Assert.AreEqual(3, result);
		}

		[Test]
		public void AddParameter_Operand_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("val", Operand.Create(100));
			var result = engine.TryEvaluate("val", 0);
			Assert.AreEqual(100, result);
		}

		#endregion

		#region AddParameterFromJson 测试

		[Test]
		public void AddParameterFromJson_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameterFromJson("{\"name\":\"test\",\"age\":25,\"active\":true}");

			var result = engine.TryEvaluate("name", "");
			Assert.AreEqual("test", result);

			var result2 = engine.TryEvaluate("age", 0);
			Assert.AreEqual(25, result2);

			var boolResult = engine.TryEvaluate("active", false);
			Assert.AreEqual(true, boolResult);
		}

		[Test]
		public void AddParameterFromJson_Complex_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameterFromJson("{\"灰色\":\"L\",\"canBookCount\":905,\"saleCount\":91}");

			var result = engine.TryEvaluate("灰色", "");
			Assert.AreEqual("L", result);

			var result2 = engine.TryEvaluate("canBookCount", 0);
			Assert.AreEqual(905, result2);
		}

		[Test]
		public void AddParameterFromJson_Invalid_Test()
		{
			var engine = new AlgorithmEngineEx();
			var hasException = false;
			try {
				engine.AddParameterFromJson("not a json");
			} catch(Exception) {
				hasException = true;
			}
			Assert.IsTrue(hasException);
		}

		#endregion

		#region ClearParameters 测试

		[Test]
		public void ClearParameters_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("num", 42);
			var result = engine.TryEvaluate("num", 0);
			Assert.AreEqual(42, result);

			engine.ClearParameters();
			result = engine.TryEvaluate("num", -1);
			Assert.AreEqual(-1, result);
		}

		#endregion

		#region UseTempDict 测试

		[Test]
		public void UseTempDict_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.UseTempDict = true;
			engine.AddParameter("num", 42);

			var result = engine.TryEvaluate("num", 0);
			Assert.AreEqual(42, result);
		}

		#endregion

		#region IgnoreCase 测试

		[Test]
		public void IgnoreCase_True_Test()
		{
			var engine = new AlgorithmEngineEx(true);
			engine.AddParameter("Name", "Test");

			var result = engine.TryEvaluate("name", "");
			Assert.AreEqual("Test", result);

			result = engine.TryEvaluate("NAME", "");
			Assert.AreEqual("Test", result);

			result = engine.TryEvaluate("Name", "");
			Assert.AreEqual("Test", result);
		}

		[Test]
		public void IgnoreCase_False_Test()
		{
			var engine = new AlgorithmEngineEx(false);
			engine.AddParameter("Name", "Test");

			var result = engine.TryEvaluate("Name", "");
			Assert.AreEqual("Test", result);

			result = engine.TryEvaluate("name", "default");
			Assert.AreEqual("default", result);
		}

		#endregion

		#region GetParameterEx 测试

		[Test]
		public void GetParameterEx_Missing_Test()
		{
			var engine = new AlgorithmEngineEx();
			var result = engine.TryEvaluate("missing_param", "default");
			Assert.AreEqual("default", result);
		}

		#endregion

		#region 综合测试

		[Test]
		public void Complex_Expression_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("a", 10);
			engine.AddParameter("b", 20);
			engine.AddParameter("c", 30);

			var result = engine.TryEvaluate("a + b + c", 0);
			Assert.AreEqual(60, result);

			result = engine.TryEvaluate("a * b - c", 0);
			Assert.AreEqual(170, result);

			result = engine.TryEvaluate("if(a > 5, b, c)", 0);
			Assert.AreEqual(20, result);
		}

		[Test]
		public void Nested_Json_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameterFromJson("{\"data\":{\"name\":\"test\",\"value\":123}}");

			var result = engine.TryEvaluate("json(data)['name']", "");
			Assert.AreEqual("test", result);
		}

		[Test]
		public void Array_Operations_Test()
		{
			var engine = new AlgorithmEngineEx();
			engine.AddParameter("arr", Operand.Create(new List<int> { 1, 2, 3, 4, 5 }));

			var result = engine.TryEvaluate("sum(arr)", 0);
			Assert.AreEqual(15, result);

			var result2 = engine.TryEvaluate("average(arr)", 0.0);
			Assert.AreEqual(3.0, result2, 0.01);

			result = engine.TryEvaluate("max(arr)", 0);
			Assert.AreEqual(5, result);

			result = engine.TryEvaluate("min(arr)", 0);
			Assert.AreEqual(1, result);

			result = engine.TryEvaluate("count(arr)", 0);
			Assert.AreEqual(5, result);
		}

		#endregion
	}
}
