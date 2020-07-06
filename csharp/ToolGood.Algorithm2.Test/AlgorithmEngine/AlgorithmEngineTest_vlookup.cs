using System;
using System.Collections.Generic;
using System.Text;
using PetaTest;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm
{
    [TestFixture]
    public class AlgorithmEngineTest_vlookup
    {
        [Test]
        public void vlookup_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var num = engine.TryEvaluate("vlookup({{1,2,3},{2,3,4},{3,4,5},{4,5,6}},3,2)", 0);
            Assert.AreEqual(num, 4);

            num = engine.TryEvaluate("vlookup({{1,'2',3},{2,'3',4},{3,'4',5},{4,'5',6}},3,2)", 0);
            Assert.AreEqual(num, 4);

            num = engine.TryEvaluate("vlookup({{1,'2',3},{2,'3',4},{3,'4',5},{4,'5',6}},3.1,2)", 0);
            Assert.AreEqual(num, 4);

            num = engine.TryEvaluate("vlookup({{1,'2',3},{2,'3',4},{'3a','4',5},{4,'5',6}},'3a',2)", 0);
            Assert.AreEqual(num, 4);
        }
        [Test]
        public void lookup_test()
        {
            var json = @"[
    {'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'},
    {'灰色':'XL','canBookCount':929,'saleCount':70,'specId':'893746f5330dc3273d24aa1ac1a9a8b5'},
    {'灰色':'XXL','canBookCount':942,'saleCount':57,'specId':'42d994cba0210528142a743d4069700f'},
    {'白色':'L','canBookCount':862,'saleCount':136,'specId':'82114cbd2c10b5e97b01af1510807e2d'},
    {'白色':'XL','canBookCount':881,'saleCount':118,'specId':'c45d8408137e34adf8e695250c42a2e9'},
    {'白色':'XXL','canBookCount':917,'saleCount':82,'specId':'df78564262818d6eb0c428a37ab4a251'},
    {'蓝色':'L','canBookCount':962,'saleCount':35,'specId':'e959b6ab7c355e403a3312c75bd3d5b4','key':null},
    {'蓝色':'XL','canBookCount':973,'saleCount':26,'specId':'27402e07efd89afa50733afa94cd6976'},
    {'蓝色':'XXL','canBookCount':985,'saleCount':14,'specId':'358b6c3b52bf711ac8ecfe7513a4f3ad'}
]";
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.AddParameter("jsonArray", json);
            // 第二种方法
            //engine.AddParameter("jsonArray",Operand.CreateJson( json));

            var num = engine.TryEvaluate("lookup([jsonArray].Json(),'[saleCount]=91','specId')", null);
            Assert.AreEqual(num, "43b0e72e98731aed69e1f0cc7d64bf4d");

            num = engine.TryEvaluate("lookup([jsonArray].Json(),'[saleCount]=35','specId')", null);
            Assert.AreEqual(num, "e959b6ab7c355e403a3312c75bd3d5b4");

            num = engine.TryEvaluate("lookup([jsonArray].Json(),'[蓝色]=\"L\"','specId')", null);
            Assert.AreEqual(num, "e959b6ab7c355e403a3312c75bd3d5b4");

            num = engine.TryEvaluate("lookup([jsonArray].Json(),'[蓝色]=\"L\"','key')", "1");
            Assert.AreEqual(null, num);
        }

    }
}
