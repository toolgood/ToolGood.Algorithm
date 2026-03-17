using PetaTest;
using System;

namespace ToolGood.Algorithm.Test.Value
{
    [TestFixture]
    internal class ValueTest
    {
        [Test]
        public void constant_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var e = engine.TryEvaluate("e", 0.0);
            Assert.AreEqual(Math.E, e, 10);
            e = engine.TryEvaluate("pi", 0.0);
            Assert.AreEqual(Math.PI, e, 10);

            var b = engine.TryEvaluate("true", false);
            Assert.AreEqual(true, b);
            b = engine.TryEvaluate("false", true);
            Assert.AreEqual(false, b);
        }

        [Test]
        public void boolean_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var b1 = engine.TryEvaluate("if(true,1,2)", 0);
            Assert.AreEqual(1, b1);

            b1 = engine.TryEvaluate("if(false,1,2)", 0);
            Assert.AreEqual(2, b1);
        }

        [Test]
        public void array_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("count(Array(1,2,3,4))", 0);
            Assert.AreEqual(4, r);

            r = engine.TryEvaluate("(1=1)*9+2", 0);
            Assert.AreEqual(11, r);
            r = engine.TryEvaluate("(1=2)*9+2", 0);
            Assert.AreEqual(2, r);
        }

        [Test]
        public void TestVersion()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            string t25 = engine.TryEvaluate("Engineversion", "");
            Assert.AreEqual("ToolGood.Algorithm 6.2", t25);
			string t26 = engine.TryEvaluate("Algorithmversion", "");
            Assert.AreEqual("ToolGood.Algorithm 6.2", t26);
        }

        [Test]
        public void Test_Json()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.Parse(@"{'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'}");
            var c = engine.Evaluate(t).ToString();
            Assert.AreEqual("{\"灰色\":\"L\",\"canBookCount\":905,\"saleCount\":91,\"specId\":\"43b0e72e98731aed69e1f0cc7d64bf4d\"}", c);
        }

           [Test]
        public void PARAM_test()
        {
            Cylinder engine = new Cylinder(10, 15);
            var num = engine.TryEvaluate("PARAM('半径')", 0);
            Assert.AreEqual(num, 10);
            num = engine.TryEvaluate("PARAMETER('半径')", 0);
            Assert.AreEqual(num, 10);
            num = engine.TryEvaluate("GETPARAMETER('半径')", 0);
            Assert.AreEqual(num, 10);

            num = engine.TryEvaluate("半径", 0);
            Assert.AreEqual(num, 10);
        }

        [Test]
        public void Error_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var num = engine.TryEvaluate("Error('出错了')", "");
            Assert.AreEqual(num, "");
            Assert.AreEqual(engine.LastError, "出错了");
        }

        [Test]
        public void Json_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var str = engine.TryEvaluate("{name:'toolgood', age:'12',}['name']", "");
            Assert.AreEqual(str, "toolgood");

            str = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}['other']['work']", "");
            Assert.AreEqual(str, "IT");

            str = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\")['name']", "");
            Assert.AreEqual(str, "toolgood");

            str = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\")['other']['work']", "");
            Assert.AreEqual(str, "IT");

            bool b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('age')", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasKey('age')", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("{e:'toolgood', pi:'12',other:{work:'IT'}}.hasKey('e')", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").has('age')", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('work')", true);
            Assert.AreEqual(b, false);

            b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasValue('toolgood')", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").hasValue('toolgood')", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void array_test2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseExcelIndex = true;
            int num = engine.TryEvaluate("[1,2,3,4,][2]", 0);
            Assert.AreEqual(num, 2);

            string str = engine.TryEvaluate("[1,2,3,4,'555'][5]", "");
            Assert.AreEqual(str, "555");

            bool b = engine.TryEvaluate("[1,2,3,4,].has('1')", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("['abc','age'].hasKey('age')", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("['abc','age'].hasValue('age')", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("json(\"['abc','age']\").has('age')", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("json(\"['abc','age']\").hasValue('age')", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Distance_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            bool b = engine.TryEvaluate("1=1m", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10dm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=100cm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000mm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.001km", false);
            Assert.AreEqual(b, true);

            engine.DistanceUnit = Algorithm.Enums.DistanceUnitType.DM;
            b = engine.TryEvaluate("1=0.1m", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1dm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10cm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=100mm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.0001km", false);
            Assert.AreEqual(b, true);

            engine.DistanceUnit = Algorithm.Enums.DistanceUnitType.CM;
            b = engine.TryEvaluate("1=0.01m", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.1dm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1cm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10mm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.00001km", false);
            Assert.AreEqual(b, true);

            engine.DistanceUnit = Algorithm.Enums.DistanceUnitType.MM;
            b = engine.TryEvaluate("1=0.001m", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.01dm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.1cm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1mm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.000001km", false);
            Assert.AreEqual(b, true);

            engine.DistanceUnit = Algorithm.Enums.DistanceUnitType.KM;

            b = engine.TryEvaluate("1=1m*1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10dm*1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=100cm*1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000mm*1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.001km*1000", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Area_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            bool b = engine.TryEvaluate("1=1m*1m", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m2=1m*1m", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m2=10dm*10dm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m2=100cm*100cm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m2=1000mm*1000mm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m2=0.001km*0.001km", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1m2", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=100dm2", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10000cm2", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000mm2", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.000001km2", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1km2=1km*1km", false);
            Assert.AreEqual(b, true);

            engine.AreaUnit = Algorithm.Enums.AreaUnitType.DM2;

            b = engine.TryEvaluate("1=1m2/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=100dm2/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10000cm2/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000mm2/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.000001km2/100", false);
            Assert.AreEqual(b, true);

            engine.AreaUnit = Algorithm.Enums.AreaUnitType.CM2;

            b = engine.TryEvaluate("1=1m2/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=100dm2/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10000cm2/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000mm2/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.000001km2/100/100", false);
            Assert.AreEqual(b, true);

            engine.AreaUnit = Algorithm.Enums.AreaUnitType.MM2;

            b = engine.TryEvaluate("1=1m2/100/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=100dm2/100/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10000cm2/100/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000mm2/100/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.000001km2/100/100/100", false);
            Assert.AreEqual(b, true);

            engine.AreaUnit = Algorithm.Enums.AreaUnitType.KM2;

            b = engine.TryEvaluate("1=1m2*1000*1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=100dm2*1000*1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10000cm2*1000*1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000mm2*1000*1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.000001km2*1000*1000", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Volume_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            bool b = engine.TryEvaluate("1=1m*1m*1m", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m3=1m*1m*1m", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m3=1m2*1m", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m3=1000L", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1L=1000ml", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m3=1000ml*1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m3=10dm*10dm*10dm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m3=100cm*100cm*100cm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m3=1000mm*1000mm*1000mm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1m3=0.001km*0.001km*0.001km", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1m3", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000dm3", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000cm3", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000000mm3", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1km3/1000/1000/1000", false);
            Assert.AreEqual(b, true);

            engine.VolumeUnit = Algorithm.Enums.VolumeUnitType.DM3;
            b = engine.TryEvaluate("1=1m3/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000dm3/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000cm3/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000000mm3/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000", false);
            Assert.AreEqual(b, true);

            engine.VolumeUnit = Algorithm.Enums.VolumeUnitType.CM3;
            b = engine.TryEvaluate("1=1m3/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000dm3/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000cm3/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000000mm3/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000/1000", false);
            Assert.AreEqual(b, true);

            engine.VolumeUnit = Algorithm.Enums.VolumeUnitType.MM3;
            b = engine.TryEvaluate("1=1m3/1000/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000dm3/1000/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000cm3/1000/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000000mm3/1000/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000/1000/1000", false);
            Assert.AreEqual(b, true);

            engine.VolumeUnit = Algorithm.Enums.VolumeUnitType.KM3;
            b = engine.TryEvaluate("1=1m3*1000*1000*1000", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("1=1000dm3*1000*1000*1000", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("1=1000000cm3*1000*1000*1000", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("1=1000000000mm3*1000*1000*1000", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("1=1km3", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Mass_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            bool b = engine.TryEvaluate("1=1kg", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000g", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.001t", false);
            Assert.AreEqual(b, true);

            engine.MassUnit = Algorithm.Enums.MassUnitType.G;

            b = engine.TryEvaluate("1=1kg/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000g/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.001t/1000", false);
            Assert.AreEqual(b, true);

            engine.MassUnit = Algorithm.Enums.MassUnitType.T;

            b = engine.TryEvaluate("1=1kg*1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000g*1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.001t*1000", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Unit_Error_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            bool b = engine.TryEvaluate("1m=1kg", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("1m=1m2", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("1m=1m3", false);
            Assert.AreEqual(b, true);
        }
    }
}
