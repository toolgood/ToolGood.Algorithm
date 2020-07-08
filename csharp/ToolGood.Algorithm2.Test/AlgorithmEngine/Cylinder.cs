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

        protected override Operand GetParameter(string parameter)
        {
            if (parameter == "半径")
            {
                return Operand.Create(_radius);
            }
            if (parameter == "直径")
            {
                return Operand.Create(_radius * 2);
            }
            if (parameter == "高")
            {
                return Operand.Create(_height);
            }
            return base.GetParameter(parameter);
        }

        protected override Operand ExecuteDiyFunction(string funcName, List<Operand> operands)
        {
            if (funcName == "求面积")
            {
                if (operands.Count == 1)
                {
                    var r = operands[0].ToNumber().NumberValue;
                    return r * r * Math.PI;
                }
            }
            return base.ExecuteDiyFunction(funcName, operands);
        }

    }
}
