using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm
{
    class FunctionException : Exception
    {
        public Operand Operand { get; private set; }
        public FunctionException(Operand operand):base(operand.StringValue)
        {
            Operand = operand;
        }
    }
}
