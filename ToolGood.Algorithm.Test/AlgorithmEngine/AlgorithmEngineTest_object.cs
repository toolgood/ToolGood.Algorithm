using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;

namespace ToolGood.Algorithm
{
    [TestFixture]
    class AlgorithmEngineTest_object
    {
        [Test]
        public void Object_test()
        {
            Cylinder c = new Cylinder(3, 10);
            var d = c.TryEvaluate("[半径]*[半径]*pi()", 0.0);
            Assert.AreEqual(d, Math.PI * 3 * 3);
            var s = c.TryEvaluate("[直径]*pi()", 0.0);
            Assert.AreEqual(s, Math.PI * 6);
            var v = c.TryEvaluate("[半径]*[半径]*pi()*[高]", 0.0);
            Assert.AreEqual(v, Math.PI * 9 * 10);

        }

    }


    public class Cylinder : AlgorithmEngine
    {
        private int _radius;
        private int _height;
        public Cylinder(int radius, int height)
        {
            _radius = radius;
            _height = height;
        }

        protected override Operand GetParameter(Operand curOpd)
        {
            if (curOpd.Parameter == "[半径]") {
                return new Operand(OperandType.NUMBER, _radius);
            }
            if (curOpd.Parameter == "[直径]") {
                return new Operand(OperandType.NUMBER, _radius * 2);
            }
            if (curOpd.Parameter == "[高]") {
                return new Operand(OperandType.NUMBER, _height);
            }
            return base.GetParameter(curOpd);
        }
    }
}
