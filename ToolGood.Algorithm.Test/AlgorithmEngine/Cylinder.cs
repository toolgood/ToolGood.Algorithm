using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm
{
    //定义圆柱信息
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
