using PetaTest;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Test.ParameterTypes
{
	[TestFixture]
	public class ParameterTypeTest
	{

		[Test]
		public void Test()
		{
			var fb = AlgorithmEngineHelper.ParseFormula("test+1");
			var list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);


			fb = AlgorithmEngineHelper.ParseFormula("if(test=1,2,3)");
			list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual("==", list[0].Operator);
			Assert.AreEqual("1", list[0].Value);

			fb = AlgorithmEngineHelper.ParseFormula("if(test>=1,2,3)");
			list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.NUMBER, list[0].Type);
			Assert.AreEqual(">=", list[0].Operator);
			Assert.AreEqual("1", list[0].Value);



			fb = AlgorithmEngineHelper.ParseFormula("if(test,2,3)");
			list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.BOOLEAN, list[0].Type);

			fb = AlgorithmEngineHelper.ParseFormula("if(test='123tt',2,3)");
			list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
			Assert.AreEqual("==", list[0].Operator);
			Assert.AreEqual("123tt", list[0].Value);

			fb = AlgorithmEngineHelper.ParseFormula("left(test,ww)");
			list = fb.GetParameterTypes(new AlgorithmEngine());
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("test", list[0].Name);
			Assert.AreEqual("ww", list[1].Name);
			Assert.AreEqual(Enums.OperandType.TEXT, list[0].Type);
		}
	}
}
