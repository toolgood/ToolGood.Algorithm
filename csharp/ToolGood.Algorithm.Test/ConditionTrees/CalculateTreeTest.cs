using PetaTest;
using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Test.ConditionTrees
{
	[TestFixture]
	public class CalculateTreeTest
	{
		[Test]
		public void Test1()
		{
			string txt = "A1+1";
			var t1 = CalculateTree.Parse(txt);
			Assert.AreEqual(t1.Type, CalculateTreeType.Add);
			Assert.AreEqual("A1+1", txt.Substring(t1.Start, t1.End - t1.Start + 1));
			Assert.AreEqual("A1+1", t1.ConditionString);


			txt = "A1-(1+1)";
			t1 = CalculateTree.Parse(txt);
			Assert.AreEqual(t1.Type, CalculateTreeType.Sub);
			Assert.AreEqual("1+1", t1.Nodes[1].ConditionString);


			txt = "A1*(1+1)";
			t1 = CalculateTree.Parse(txt);
			Assert.AreEqual(t1.Type, CalculateTreeType.Mul);

			txt = "A1/(1+1)";
			t1 = CalculateTree.Parse(txt);
			Assert.AreEqual(t1.Type, CalculateTreeType.Div);


			txt = "A1%(1+1)";
			t1 = CalculateTree.Parse(txt);
			Assert.AreEqual(t1.Type, CalculateTreeType.Mod);

			txt = "A1&(1+1)";
			t1 = CalculateTree.Parse(txt);
			Assert.AreEqual(t1.Type, CalculateTreeType.Connect);

			txt = "A1(1+1)";
			t1 = CalculateTree.Parse(txt);
			Assert.AreEqual(t1.Type, CalculateTreeType.String);

			txt = "A1(1+1)-";
			t1 = CalculateTree.Parse(txt);
			Assert.AreEqual(t1.Type, CalculateTreeType.Error);

			txt = "-1+(1+1)";
			t1 = CalculateTree.Parse(txt);
			Assert.AreEqual(t1.Type, CalculateTreeType.Add);

			txt = "-1";
			t1 = CalculateTree.Parse(txt);
			Assert.AreEqual(t1.Type, CalculateTreeType.String);
		}

	}
}
