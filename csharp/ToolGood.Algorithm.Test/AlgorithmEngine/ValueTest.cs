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
            Assert.AreEqual("ToolGood.Algorithm 6.3", t25);
			string t26 = engine.TryEvaluate("Algorithmversion", "");
            Assert.AreEqual("ToolGood.Algorithm 6.3", t26);
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
        public void Json_large_number_error_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // Bug A 回归: 超出 decimal 范围的超大数解析应报错(修复前静默返回 0)
            var t = engine.TryEvaluate("json('{\"a\":99999999999999999999999999999999999}')['a']", 0.0);
            Assert.IsTrue(engine.LastError != null);
        }

        [Test]
        public void Json_string_indexer_no_exception_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // Bug B 回归: 非对象 JSON 值被字符串索引不应抛 NullReferenceException
            var t = engine.TryEvaluate("json('123')['a']", "");
            Assert.IsTrue(engine.LastError != null);

            // 不存在的键返回错误而非异常
            t = engine.TryEvaluate("json('{\"a\":1}')['b']", "");
            Assert.IsTrue(engine.LastError != null);
        }

        [Test]
        public void Json_backslash_escape_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // Bug C 回归: 序列化字符串应转义反斜杠(修复前漏转义, 输出为 C:\temp)
            var t = engine.Parse(@"{'a':'C:\\temp'}");
            var c = engine.Evaluate(t).ToString();
            Assert.AreEqual("{\"a\":\"C:\\\\temp\"}", c);

            // 反斜杠与引号同时转义
            t = engine.Parse("{'a':'a\\\"b\\\\c'}");
            c = engine.Evaluate(t).ToString();
            Assert.AreEqual("{\"a\":\"a\\\"b\\\\c\"}", c);
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
        public void Distance_M_Test()
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
        }

        [Test]
        public void Distance_DM_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.DistanceUnit = Algorithm.Enums.DistanceUnitType.DM;
            bool b = engine.TryEvaluate("1=0.1m", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1dm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10cm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=100mm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.0001km", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Distance_CM_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.DistanceUnit = Algorithm.Enums.DistanceUnitType.CM;
            bool b = engine.TryEvaluate("1=0.01m", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.1dm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1cm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10mm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.00001km", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Distance_MM_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.DistanceUnit = Algorithm.Enums.DistanceUnitType.MM;
            bool b = engine.TryEvaluate("1=0.001m", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.01dm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.1cm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1mm", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.000001km", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Distance_KM_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.DistanceUnit = Algorithm.Enums.DistanceUnitType.KM;

            bool b = engine.TryEvaluate("1=1m*1000", false);
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
        public void Area_M2_Test()
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
        }

        [Test]
        public void Area_DM2_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.AreaUnit = Algorithm.Enums.AreaUnitType.DM2;

            bool b = engine.TryEvaluate("1=1m2/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=100dm2/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10000cm2/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000mm2/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.000001km2/100", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Area_CM2_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.AreaUnit = Algorithm.Enums.AreaUnitType.CM2;

            bool b = engine.TryEvaluate("1=1m2/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=100dm2/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10000cm2/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000mm2/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.000001km2/100/100", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Area_MM2_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.AreaUnit = Algorithm.Enums.AreaUnitType.MM2;

            bool b = engine.TryEvaluate("1=1m2/100/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=100dm2/100/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=10000cm2/100/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000mm2/100/100/100", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.000001km2/100/100/100", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Area_KM2_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.AreaUnit = Algorithm.Enums.AreaUnitType.KM2;

            bool b = engine.TryEvaluate("1=1m2*1000*1000", false);
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
        public void Volume_M3_Test()
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
        }

        [Test]
        public void Volume_DM3_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.VolumeUnit = Algorithm.Enums.VolumeUnitType.DM3;
            bool b = engine.TryEvaluate("1=1m3/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000dm3/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000cm3/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000000mm3/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Volume_CM3_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.VolumeUnit = Algorithm.Enums.VolumeUnitType.CM3;
            bool b = engine.TryEvaluate("1=1m3/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000dm3/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000cm3/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000000mm3/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000/1000", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Volume_MM3_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.VolumeUnit = Algorithm.Enums.VolumeUnitType.MM3;
            bool b = engine.TryEvaluate("1=1m3/1000/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000dm3/1000/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000cm3/1000/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000000000mm3/1000/1000/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000/1000/1000", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Volume_KM3_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.VolumeUnit = Algorithm.Enums.VolumeUnitType.KM3;
            bool b = engine.TryEvaluate("1=1m3*1000*1000*1000", false);
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
        public void Mass_KG_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            bool b = engine.TryEvaluate("1=1kg", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000g", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.001t", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Mass_G_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.MassUnit = Algorithm.Enums.MassUnitType.G;

            bool b = engine.TryEvaluate("1=1kg/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=1000g/1000", false);
            Assert.AreEqual(b, true);

            b = engine.TryEvaluate("1=0.001t/1000", false);
            Assert.AreEqual(b, true);
        }

        [Test]
        public void Mass_T_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.MassUnit = Algorithm.Enums.MassUnitType.T;

            bool b = engine.TryEvaluate("1=1kg*1000", false);
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

        /// <summary>
        /// 往返一致性校验：Parse(exp) -> ToString 还原 -> 重新 Parse 求值，
        /// 要求还原前后语义完全一致(结果值 + 是否错误)。
        /// </summary>
        private static void AssertRoundTrip(string exp)
        {
            var engine1 = new AlgorithmEngine();
            var function = engine1.Parse(exp);
            var text = function.ToString();
            var result1 = engine1.Evaluate(function);

            var engine2 = new AlgorithmEngine();
            Operand result2;
            try {
                result2 = engine2.Evaluate(engine2.Parse(text));
            } catch (Exception ex) {
                throw new Exception($"往返测试失败: [{exp}] 还原为 [{text}] 后重新求值抛出 {ex.GetType().Name}: {ex.Message}");
            }

            if(result1.IsError != result2.IsError || result1.ToString() != result2.ToString()) {
                throw new Exception($"往返测试失败: [{exp}] 还原为 [{text}]，原结果=[{result1}](IsError={result1.IsError})，往返结果=[{result2}](IsError={result2.IsError})");
            }
        }

        [Test]
        public void P0_1_number_tostring_invariant_culture_test()
        {
            // P0-1 回归: ToString 必须使用 InvariantCulture，
            // 否则在 de-DE/fr-FR 等区域下小数点会输出为 ','，导致结果无法被本引擎重新解析
            var oldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");

                var engine = new AlgorithmEngine();
                Assert.AreEqual("1.5", engine.Parse("1.5").ToString());
                Assert.AreEqual("1.5M", engine.Parse("1.5M").ToString());
                Assert.AreEqual("1234.5 + 0", engine.Parse("1234.5 + 0").ToString());

                var e2 = new AlgorithmEngine();
                var r = e2.TryEvaluate(engine.Parse("1.5").ToString(), 0.0);
                Assert.AreEqual(1.5, r, 10);

                AssertRoundTrip("1.5");
                AssertRoundTrip("1.5 + 1");
                AssertRoundTrip("1.5M");
                AssertRoundTrip("1.5M + 1M");
                AssertRoundTrip("1234.5 + 0");
            } finally {
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCulture;
            }
        }

        [Test]
        public void P0_2_arrayjson_error_propagation_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // P0-2 回归: JSON 项内的错误必须向上传播，
            // 修复前错误操作数被静默包裹成 KeyValue，导致 IsError 为 false
            var r = engine.Evaluate(engine.Parse("{\"a\":1, \"b\":1/0}"));
            Assert.IsTrue(r.IsError);
            Assert.AreEqual("Function '/' Div 0 error!", r.ErrorMsg);

            r = engine.Evaluate(engine.Parse("{\"a\":1/0}"));
            Assert.IsTrue(r.IsError);

            // 正常 JSON 不受影响
            r = engine.Evaluate(engine.Parse("{\"a\":1,\"b\":2}"));
            Assert.IsFalse(r.IsError);
            Assert.AreEqual("{\"a\":1,\"b\":2}", r.ToString());

            AssertRoundTrip("{\"a\":1,\"b\":2}");
            AssertRoundTrip("{\"a\":1, \"b\":1/0}");
        }

        [Test]
        public void P0_3_number_out_of_range_parse_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // P0-3 回归: 超出 decimal 范围的纯数字必须在 Parse 阶段抛出明确异常，
            // 而不是在 Evaluate 时抛 KeyNotFoundException(修复前末位数字被误当作单位)
            try {
                engine.Parse("99999999999999999999999999999");
                throw new Exception("P0-3 回归失败: 29 位纯数字应在 Parse 阶段抛出 ArgumentException");
            } catch (ArgumentException ex) {
                Assert.IsTrue(ex.Message.Contains("is invalid or out of range"));
            }

            // ParseWithoutError 不抛异常，仅返回 null
            Assert.IsNull(engine.ParseWithoutError("99999999999999999999999999999"));

            // 边界内数值仍可正常解析求值
            var r = engine.Evaluate(engine.Parse("9999999999999999999999999999"));
            Assert.IsFalse(r.IsError);
            Assert.AreEqual(9999999999999999999999999999m, r.NumberValue);
        }

        [Test]
        public void P0_4_number_unit_overflow_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // P0-4 回归: 单位换算放大溢出应返回 #NUM! 错误操作数，而不是抛出 OverflowException
            var r = engine.Evaluate(engine.Parse("9999999999999999999999999999KM3"));
            Assert.IsTrue(r.IsError);
            Assert.AreEqual("Function 'Num' Num error!", r.ErrorMsg);

            // 溢出后 TryEvaluate 返回默认值，并填充 LastError
            var d = engine.TryEvaluate("9999999999999999999999999999KM3", -1.0);
            Assert.AreEqual(-1.0, d, 10);
            Assert.IsNotNull(engine.LastError);

            // 未溢出的单位换算不受影响
            r = engine.Evaluate(engine.Parse("1KM"));
            Assert.IsFalse(r.IsError);
            Assert.AreEqual(1000m, r.NumberValue);

            // 单位数值同样支持往返
            AssertRoundTrip("1KM");
            AssertRoundTrip("1.5M");
        }

        [Test]
        public void P1_1_arrayjson_key_escape_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // P1-1 回归: 含引号/空格的 key 必须按字符串字面量转义输出。
            // 修复前 ToString 输出 [{a\"b:1}]，再次 Parse 会抛 InvalidCastException
            var f = engine.Parse("{\"a\\\"b\":1}");
            Assert.AreEqual("{\"a\\\"b\":1}", f.ToString());
            AssertRoundTrip("{\"a\\\"b\":1}");

            f = engine.Parse("{\"a b\":1}");
            Assert.AreEqual("{\"a b\":1}", f.ToString());
            AssertRoundTrip("{\"a b\":1}");

            // 含换行转义的 key
            AssertRoundTrip("{\"a\\nb\":1}");

            // 单引号字面量同样解转义，输出统一规范化为双引号
            Assert.AreEqual("{\"a'b\":1}", engine.Parse("{\'a\\\'b\':1}").ToString());
        }

        [Test]
        public void P1_2_diyfunction_error_propagation_test()
        {
            // P1-2 回归: 自定义函数的参数为错误操作数时必须在引擎内被拦截。
            // 修复前错误操作数被直接传入 ExecuteDiyFunction，访问 NumberValue 会抛 NotImplementedException
            Cylinder engine = new Cylinder(10, 15);
            var r = engine.Evaluate(engine.Parse("求面积(1/0)"));
            Assert.IsTrue(r.IsError);
            Assert.AreEqual("Function '/' Div 0 error!", r.ErrorMsg);

            // 溢出后 TryEvaluate 返回默认值，并填充 LastError
            var d = engine.TryEvaluate("求面积(1/0)", -1.0);
            Assert.AreEqual(-1.0, d, 10);
            Assert.IsNotNull(engine.LastError);

            // 正常参数不受影响
            r = engine.Evaluate(engine.Parse("求面积(2)"));
            Assert.IsFalse(r.IsError);
            Assert.IsTrue(Math.Abs((double)r.NumberValue - 4 * Math.PI) < 1e-9);
        }

        [Test]
        public void P1_3_string_unicode_escape_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // P1-3 回归: \uXXXX 十六进制转义必须被解析。
            // 修复前 "\u000b" 被解析成 "u000b"(丢弃 \u 标记与后 4 位)
            var r = engine.Evaluate(engine.Parse("\"a\\u0041b\""));
            Assert.AreEqual("aAb", r.TextValue);

            // 控制字符往返: 0x0B(\v) / 0x07(\a) 由 Function_ValueText 转义输出，重新解析必须还原
            AssertRoundTrip("\"a\\u000bb\"");
            AssertRoundTrip("\"a\\u0007b\"");

            // 不完整转义按原样保留，不吞字符
            r = engine.Evaluate(engine.Parse("\"a\\ub\""));
            Assert.AreEqual("aub", r.TextValue);
        }

        [Test]
        public void Visitors_P1_1_num_unit_split_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // Visitors P1-1 回归: 超出 decimal 范围的指数记法必须与 "1E+30" 一致地在 Parse 阶段失败。
            // 修复前单位切分只判断后缀首字母是否为字母，把 'E' 当作单位首字母切出
            // 数值 1 + 单位 "E29"，报出误导性的"Number unit 'E29' is invalid!"。
            foreach(var exp in new[] { "1E29", "12E29", "1e29", "1E+30", "1E300" }) {
                try {
                    engine.Parse(exp);
                    throw new Exception($"Visitors P1-1 回归失败: [{exp}] 应在 Parse 阶段抛出 ArgumentException");
                } catch(ArgumentException ex) {
                    if(ex.Message.Contains("unit") == true) {
                        throw new Exception($"Visitors P1-1 回归失败: [{exp}] 报出单位错误而非数值越界: {ex.Message}");
                    }
                }
                Assert.IsNull(engine.ParseWithoutError(exp));
            }

            // 范围内的指数记法走数值分支，不受单位切分影响
            var r = engine.Evaluate(engine.Parse("1E2"));
            Assert.IsFalse(r.IsError);
            Assert.AreEqual(100m, r.NumberValue);
            r = engine.Evaluate(engine.Parse("1.5e2"));
            Assert.IsFalse(r.IsError);
            Assert.AreEqual(150m, r.NumberValue);

            // 长度 1/2/3 的单位后缀都必须按完整后缀切分(不能只按末 2/3 位)
            Assert.AreEqual("1KM2", engine.Parse("1KM2").ToString());
            Assert.AreEqual("1M2", engine.Parse("1M2").ToString());
            Assert.AreEqual("1ML", engine.Parse("1ML").ToString());
            Assert.AreEqual("1KG", engine.Parse("1KG").ToString());
            Assert.AreEqual("1T", engine.Parse("1T").ToString());
            Assert.AreEqual("1L", engine.Parse("1L").ToString());

            // 单位数值语义不变，大小写单位等价(token 文本保留原样，需按不区分大小写匹配)
            Assert.AreEqual(1000m, engine.Evaluate(engine.Parse("1KM")).NumberValue);
            Assert.AreEqual(1000m, engine.Evaluate(engine.Parse("1km")).NumberValue);
            Assert.AreEqual(1234m, engine.Evaluate(engine.Parse("1234M")).NumberValue);
            Assert.AreEqual(12m, engine.Evaluate(engine.Parse("12M")).NumberValue);
            Assert.AreEqual(1m, engine.Evaluate(engine.Parse("1m")).NumberValue);

            AssertRoundTrip("1KM2");
            AssertRoundTrip("1ML");
            AssertRoundTrip("12M");
            AssertRoundTrip("1E2");
        }

        [Test]
        public void Visitors_P1_2_function_branch_split_test()
        {
            // Visitors P1-2 回归: VisitFunction_fun 的 PARAMETER(自定义函数) 分支拆分后行为必须不变，
            // 未实现的内建函数 token 不再与 PARAMETER 共用分支而静默降级为自定义函数。
            Cylinder engine = new Cylinder(10, 15);
            var r = engine.Evaluate(engine.Parse("求面积(2)"));
            Assert.IsFalse(r.IsError);
            Assert.IsTrue(Math.Abs((double)r.NumberValue - 4 * Math.PI) < 1e-9);

            // 自定义函数与内建函数混用时各走各的分支
            r = engine.Evaluate(engine.Parse("求面积(2) + ABS(-1)"));
            Assert.IsFalse(r.IsError);
            Assert.IsTrue(Math.Abs((double)r.NumberValue - (4 * Math.PI + 1)) < 1e-9);

            // 未注册的自定义函数在调用时给出明确错误，而不是静默返回结果
            var engine2 = new AlgorithmEngine();
            r = engine2.Evaluate(engine2.Parse("求面积(2)"));
            Assert.IsTrue(r.IsError);
            Assert.AreEqual("DiyFunction [求面积] is missing.", r.ErrorMsg);

            // 自定义函数名参与往返
            AssertRoundTrip("求面积(2)");
        }
    }
}
