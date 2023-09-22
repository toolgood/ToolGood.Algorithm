using PetaTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm2.Test.AlgorithmEngine_V35
{
    [TestFixture]
    public class AlgorithmEngineTest_v3
    {
        [Test]
        public void PARAM_test()
        {
            // PARAM 动态获取参数 
            Cylinder engine = new Cylinder(10, 15);
            var num = engine.TryEvaluate("PARAM('半径')", 0);
            Assert.AreEqual(num, 10);
            num = engine.TryEvaluate("PARAMETER('半径')", 0);
            Assert.AreEqual(num, 10);
            num = engine.TryEvaluate("GETPARAMETER('半径')", 0);
            Assert.AreEqual(num, 10);

            // 参数没有限制了
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

            // 使用json 方法 使用比较标准的 json格式， 不然会出错  
            str = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\")['name']", "");
            Assert.AreEqual(str, "toolgood");

            str = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\")['other']['work']", "");
            Assert.AreEqual(str, "IT");

            // 'HAS' | 'HASKEY' |'CONTAINS'|'CONTAINSKEY' 指向同一函数  只支持数组与json类型
            bool b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('age')", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasKey('age')", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").has('age')", false);
            Assert.AreEqual(b, true);

            // 注意只能获取第一层
            b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('work')", true);
            Assert.AreEqual(b, false);


            // 'HASVALUE' | 'CONTAINSVALUE' 指向同一函数   只支持数组与json类型
            b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasValue('toolgood')", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").hasValue('toolgood')", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void array_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            // 'HAS' | 'HASKEY' |'CONTAINS'|'CONTAINSKEY' 指向同一函数
            // 'HASVALUE' | 'CONTAINSVALUE' 指向同一函数 与上面的逻辑相同
            bool b = engine.TryEvaluate("{1,2,3,4,}.has('1')", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("{'abc','age'}.hasKey('age')", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("{'abc','age'}.hasValue('age')", false);
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
            // 下面是错误 演示， 因为计算时不会考虑单位，所以下面是正常通过的
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
