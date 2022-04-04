using PetaTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolGood.Algorithm;
using ToolGood.Algorithm.Internals;
using static ToolGood.Algorithm.Internals.ConditionTree;

namespace ToolGood.Algorithm2.Test.ConditionTrees
{
    [TestFixture]
    public class ConditionTreeTest
    {
        [Test]
        public void Test1()
        {
            string txt = "AA.IsText()=bb";
            var t1 = ConditionTree.Parse(txt);
            Assert.AreEqual(t1.Type, ConditionType.String);
            Assert.AreEqual("AA.IsText()=bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));

            txt = "[bbb]=bb";
            t1 = ConditionTree.Parse(txt);
            Assert.AreEqual(t1.Type, ConditionType.String);
            Assert.AreEqual(txt, txt.Substring(t1.Start, t1.End - t1.Start + 1));
        }

        [Test]
        public void Test2()
        {
            string txt = "AA.IsText()=bb && dd=ss";
            var tree = ConditionTree.Parse(txt);

            Assert.AreEqual(tree.Type, ConditionType.And);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            Assert.AreEqual("AA.IsText()=bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("dd=ss", txt.Substring(t2.Start, t2.End - t2.Start + 1));
        }

        [Test]
        public void Test3()
        {
            string txt = "AA.IsText()=bb || dd=ss";
            var tree = ConditionTree.Parse(txt);

            Assert.AreEqual(tree.Type, ConditionType.Or);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            Assert.AreEqual("AA.IsText()=bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("dd=ss", txt.Substring(t2.Start, t2.End - t2.Start + 1));
        }


        [Test]
        public void Test4()
        {
            string txt = "AA.IsText()=bb || (dd=ss && tt=22)";
            var tree = ConditionTree.Parse(txt);

            Assert.AreEqual(tree.Type, ConditionType.Or);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            var t3 = t2.Nodes[0];
            var t4 = t2.Nodes[1];
            Assert.AreEqual("AA.IsText()=bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("dd=ss", txt.Substring(t3.Start, t3.End - t3.Start + 1));
            Assert.AreEqual("tt=22", txt.Substring(t4.Start, t4.End - t4.Start + 1));
        }

        [Test]
        public void Test5()
        {
            string txt = "AA.IsText()=bb || AND(dd=ss , tt=22)";
            var tree = ConditionTree.Parse(txt);

            Assert.AreEqual(tree.Type, ConditionType.Or);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            Assert.AreEqual("AA.IsText()=bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("AND(dd=ss , tt=22)", txt.Substring(t2.Start, t2.End - t2.Start + 1));
        }

        [Test]
        public void Test6()
        {
            string txt = "AA.IsText()==bb && (dd=ss || tt=22)";
            var tree = ConditionTree.Parse(txt);

            Assert.AreEqual(tree.Type, ConditionType.And);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            Assert.AreEqual(t2.Type, ConditionType.Or);
            var t3 = t2.Nodes[0];
            var t4 = t2.Nodes[1];

            Assert.AreEqual("AA.IsText()==bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("dd=ss", txt.Substring(t3.Start, t3.End - t3.Start + 1));
            Assert.AreEqual("tt=22", txt.Substring(t4.Start, t4.End - t4.Start + 1));
        }

        [Test]
        public void Test7()
        {
            string txt = "AA.IsText()==bb ? 1:2";
            var t1 = ConditionTree.Parse(txt);
            Assert.AreEqual(t1.Type, ConditionType.String);
            Assert.AreEqual(txt, txt.Substring(t1.Start, t1.End - t1.Start + 1));
        }

        [Test]
        public void Test8()
        {
            string txt = "AA.IsText()==bb && (dd=ss || {tt}=22)";
            var tree = ConditionTree.Parse(txt);

            Assert.AreEqual(tree.Type, ConditionType.And);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            Assert.AreEqual(t2.Type, ConditionType.Or);
            var t3 = t2.Nodes[0];
            var t4 = t2.Nodes[1];

            Assert.AreEqual("AA.IsText()==bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("dd=ss", txt.Substring(t3.Start, t3.End - t3.Start + 1));
            Assert.AreEqual("{tt}=22", txt.Substring(t4.Start, t4.End - t4.Start + 1));
        }

        [Test]
        public void Test9()
        {
            string txt = "AA.IsText()==bb && (dd=ss || [tt]==22)";
            var tree = ConditionTree.Parse(txt);

            Assert.AreEqual(tree.Type, ConditionType.And);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            Assert.AreEqual(t2.Type, ConditionType.Or);
            var t3 = t2.Nodes[0];
            var t4 = t2.Nodes[1];

            Assert.AreEqual("AA.IsText()==bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("dd=ss", txt.Substring(t3.Start, t3.End - t3.Start + 1));
            Assert.AreEqual("[tt]==22", txt.Substring(t4.Start, t4.End - t4.Start + 1));
        }


    }
}
