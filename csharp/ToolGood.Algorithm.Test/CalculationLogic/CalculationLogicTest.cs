using PetaTest;
using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.CalculationLogic;

namespace ToolGood.Algorithm.Test.CalculationLogic
{
	[TestFixture]
	public class CalculationLogicTest
	{
		[Test]
		public void Test()
		{
			FunctionCache functionCache = new FunctionCache();
			var logic = new CalculationLogicEngine(functionCache, true);
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

			var result = logic.ToInfoString();
			Console.WriteLine(result);
		}


	}
}
