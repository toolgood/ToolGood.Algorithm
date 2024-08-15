using PetaTest;
using System;

namespace ToolGood.Algorithm
{
    [TestFixture]
    internal class AlgorithmEngineTest
    {
        [Test]
        public void Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            double t = 0.0;
            if (engine.Parse("1+2")) {
                t = (double)engine.Evaluate().NumberValue;
            }
            Assert.AreEqual(3.0, t);

            var c = engine.TryEvaluate("2+3", 0);
            Assert.AreEqual(5, c);
            c = engine.TryEvaluate("(2)+3", 0);
            Assert.AreEqual(5, c);
            c = engine.TryEvaluate("2+3*2+10/2*4", 0);
            Assert.AreEqual(28, c);

            c = engine.TryEvaluate("if(2+3*2+10/2*4,1", 0);
            Assert.AreEqual(0, c);

            c = engine.TryEvaluate("2.1e3 + 10", 0);
            Assert.AreEqual(2110, c);

            c = engine.TryEvaluate("2.1e+03 + 10", 0);
            Assert.AreEqual(2110, c);

            c = engine.TryEvaluate("2.1e+3 + 10", 0);
            Assert.AreEqual(2110, c);

            var d = engine.TryEvaluate("2.1e-3 + 10", 0.0);
            Assert.AreEqual(10.0021, d);

            var e = engine.TryEvaluate("e", 0.0);
            Assert.AreEqual(Math.E, e, 10);
            e = engine.TryEvaluate("pi", 0.0);
            Assert.AreEqual(Math.PI, e, 10);

            var b = engine.TryEvaluate("true", false);
            Assert.AreEqual(true, b);
            b = engine.TryEvaluate("false", true);
            Assert.AreEqual(false, b);

            var b1 = engine.TryEvaluate("if(true,1,2)", 0);
            Assert.AreEqual(1, b1);

            b1 = engine.TryEvaluate("if(false,1,2)", 0);
            Assert.AreEqual(2, b1);

            var b2 = engine.TryEvaluate("pi*4", 0.0);
            Assert.AreEqual(Math.PI * 4, b2, 10);
            b2 = engine.TryEvaluate("e*4", 0.0);
            Assert.AreEqual(Math.E * 4, b2, 10);

            var s = engine.TryEvaluate("'aa'&'bb'", "");
            Assert.AreEqual("aabb", s);

            s = engine.TryEvaluate("'3'+2", "");
            Assert.AreEqual("5", s);

            var r = engine.TryEvaluate("count(Array(1,2,3,4))", 0);
            Assert.AreEqual(4, r);

            r = engine.TryEvaluate("(1=1)*9+2", 0);
            Assert.AreEqual(11, r); ;
            r = engine.TryEvaluate("(1=2)*9+2", 0);
            Assert.AreEqual(2, r); ;

            var dt = engine.TryEvaluate("'2016-1-1'+1", DateTime.MinValue);
            Assert.AreEqual(DateTime.Parse("2016-1-2"), dt); ;
            dt = engine.TryEvaluate("'2016-1-1'+9*'1:0'", DateTime.MinValue);
            Assert.AreEqual(DateTime.Parse("2016-1-1 9:0"), dt); ;

            var value = engine.TryEvaluate("1 > (-2)", false);
            Assert.AreEqual(value, true);

            value = engine.TryEvaluate("(-1) > (-2）", false);
            Assert.AreEqual(value, true);

            value = engine.TryEvaluate("-1 > (-2)", false);
            Assert.AreEqual(value, true);

            value = engine.TryEvaluate("-1 > -2", false);
            Assert.AreEqual(value, true);

            var value2 = engine.TryEvaluate("-1 > -2", false);
            Assert.AreEqual(value2, true);

            var value3 = engine.TryEvaluate("-7 < -2", false);
            Assert.AreEqual(value3, true);

            var t1 = engine.TryEvaluate("-7 < -2 ?1 : 2", 0);
            Assert.AreEqual(t1, 1);
            t1 = engine.TryEvaluate("-7 < -2 ?1 ： 2", 0);
            Assert.AreEqual(t1, 1);

            t1 = engine.TryEvaluate("-7 < -2 ?1 ：2", 0);
            Assert.AreEqual(t1, 1);
            t1 = engine.TryEvaluate("-7 < -2 ？ 1 : 2", 0);
            Assert.AreEqual(t1, 1);
            t1 = engine.TryEvaluate("-7 < -2 ？1 : 2", 0);
            Assert.AreEqual(t1, 1);

            t1 = engine.TryEvaluate("-7 < -2 ？1 ： 2", 0);
            Assert.AreEqual(t1, 1);

            t1 = engine.TryEvaluate("(!(-7 < -2))？1：2", 0);
            Assert.AreEqual(t1, 2);
            t1 = engine.TryEvaluate("1>2？1：2", 0);
            Assert.AreEqual(t1, 2);

            t1 = engine.TryEvaluate("1！=2？1：2", 0);
            Assert.AreEqual(t1, 1);

            var t2 = engine.TryEvaluate("Ａsc('ａｂｃＡＢＣ１２３')", "");
            Assert.AreEqual(t2, "abcABC123");
        }

        [Test]
        public void base_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("1+(3*2+2)/2", 0);
            Assert.AreEqual(5, t);

            t = engine.TryEvaluate("(8-3)*(3+2)", 0);
            Assert.AreEqual(25, t);

            t = engine.TryEvaluate("(8-3)*(3+2) % 7", 0);
            Assert.AreEqual(4, t);

            var b = engine.TryEvaluate("1=1", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1=2", true);
            Assert.AreEqual(false, b);

            b = engine.TryEvaluate("1<>2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1!=2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1>2", true);
            Assert.AreEqual(false, b);

            b = engine.TryEvaluate("1<2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1<=2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1>=2", true);
            Assert.AreEqual(false, b);

            b = engine.TryEvaluate("'1'='1'", false);
            Assert.AreEqual(true, b);
            b = engine.TryEvaluate("'e'='e'", false);
            Assert.AreEqual(true, b);
            b = engine.TryEvaluate("'1'='2'", true);
            Assert.AreEqual(false, b);
            b = engine.TryEvaluate("'1'!='2'", false);
            Assert.AreEqual(true, b);
        }

        [Test]
        public void base_test2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("1+(3*2+2)/2 & '11' & '11:20'*9 & isnumber(22)*3", "");
        }

        [Test]
        public void base_test3()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            var c = engine.TryEvaluate("(2)+/*123456*/3", 0);
            Assert.AreEqual(5, c);

            c = engine.TryEvaluate("2+3//eee", 0);
            Assert.AreEqual(5, c);

            c = engine.TryEvaluate("(2)+/*123456*/3 ee22+22", 0);
            Assert.AreEqual(0, c);
        }

        [Test]
        public void base_test4()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            String c = engine.TryEvaluate("'4dd'&'55' rr", "");
            Assert.AreEqual("", c);
        }

        [Test]
        public void base_test5()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            String c = engine.TryEvaluate("'4dd'&'55'.left(1)", "");
            Assert.AreEqual("4dd5", c);
        }

        [Test]
        public void Cylinder_Test()
        {
            Cylinder c = new Cylinder(3, 10);
            var t = c.TryEvaluate("[半径]*[半径]*pi()", 0.0);      //圆底面积
            var t2 = c.TryEvaluate("半径*半径*pi()", 0.0);      //圆底面积

            // v3.5 后 不支持这类定义  {} 用于定义 数组 或json
            // var t3 = c.TryEvaluate("{半径}*{半径}*pi()", 0.0);      //圆底面积

            var t4 = c.TryEvaluate("@半径*@半径*pi()", 0.0);      //圆底面积
            var t5 = c.TryEvaluate("#半径#*#半径#*pi()", 0.0);      //圆底面积
            var t6 = c.TryEvaluate("【半径】*【半径】*pi()", 0.0);      //圆底面积
            var t7 = c.TryEvaluate("【半径】*【半径】*pi（）", 0.0);      //圆底面积

            Assert.AreEqual(t, t2);
            //Assert.AreEqual(t, t3);
            Assert.AreEqual(t, t4);
            Assert.AreEqual(t, t5);
            Assert.AreEqual(t, t6);
            Assert.AreEqual(t, t7);

            t = c.TryEvaluate("[直径]*pi()", 0.0);            //圆的长
            t = c.TryEvaluate("[半径]*[半径]*pi()*[高]", 0.0); //圆的体积

            if (c.Parse("[直径1]*pi()") == false) {
                Assert.AreEqual("参数[直径1]无效!", c.LastError);
            }

            t = c.TryEvaluate("['半径']*[半径]*pi()*[高]", 0.0); //圆的体积

            t = c.TryEvaluate("求面积（10）", 0.0); //圆的体积
            Assert.AreEqual(10 * 10 * Math.PI, t, 10);

            var json = "{'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'}";
            c.AddParameterFromJson(json);

            var tt = c.TryEvaluate("['灰色']", ""); //圆的体积
            Assert.AreEqual("L", tt);

            String tt2 = c.EvaluateFormula("'圆'-[半径]-高", '-');
            Assert.AreEqual("圆-3-10", tt2);
        }

        [Test]
        public void Test5555()
        {
            Cylinder c = new Cylinder(3, 10);
            String t = c.GetSimplifiedFormula("[半径]*[半径]*pi()"); // 圆底面积
            Assert.AreEqual("3 * 3 * 3.14159265358979", t);

            String t2 = c.GetSimplifiedFormula("半径*if(半径>2,1,3)");
            Assert.AreEqual("3 * 1", t2);

            String t24 = c.GetSimplifiedFormula("半径*if(半径>2,1+4,3)");
            Assert.AreEqual("3 * 5", t24);
        }
    }
}