using System;
using System.Collections.Generic;
using System.Text;
using PetaTest;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm2.Test.Tests
{
    [TestFixture]
    public class IssuesTest
    {
        [Test]
        public void issues_12()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Year(44406)=2021", false);
            Assert.AreEqual(dt, true);
            dt = engine.TryEvaluate("MONTH(44406)=7", false);
            Assert.AreEqual(dt, true);
            dt = engine.TryEvaluate("DAY(44406)=29", false);
            Assert.AreEqual(dt, true);

            int num = engine.TryEvaluate("date(2011,2,2)", 0);
            Assert.AreEqual(num, 40576);

        }


        [Test]
        public void issues_13()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("days360(date(2020,5,31),date(2023,12,15))", 0);
            Assert.AreEqual(dt, 1275);
        }


        [Test]
        public void issues_27()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("DATE(2024, 8, 1) + TIME(8, 0, 0)", DateTime.Now);
            Assert.AreEqual(dt, DateTime.Parse("2024-08-01 8:0"));
        }

    }
}
