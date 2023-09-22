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
            Cylinder engine = new Cylinder(10,15);
            var num = engine.TryEvaluate("PARAM('半径')", 0);
            Assert.AreEqual(num, 10);
            num = engine.TryEvaluate("PARAMETER('半径')", 0);
            Assert.AreEqual(num, 10);
            num = engine.TryEvaluate("GETPARAMETER('半径')", 0);
            Assert.AreEqual(num, 10);
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

            // 'HAS' | 'HASKEY' |'CONTAINS'|'CONTAINSKEY' 指向同一函数
            bool b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('age')", false);
            Assert.AreEqual(b, true);
            b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasKey('age')", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").has('age')", false);
            Assert.AreEqual(b, true);

            // 注意只能获取第一层
            b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('work')", true);
            Assert.AreEqual(b, false);


            // 'HASVALUE' | 'CONTAINSVALUE' 指向同一函数
            b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasValue('toolgood')", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").hasValue('toolgood')", false);
            Assert.AreEqual(b, true);
        }






    }
}
