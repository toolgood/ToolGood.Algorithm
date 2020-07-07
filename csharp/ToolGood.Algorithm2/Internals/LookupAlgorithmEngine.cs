using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals
{
    class LookupAlgorithmEngine : AlgorithmEngine
    {
        public Operand Json;

        protected override Operand GetParameter(string parameter)
        {
            var v = Json.JsonValue[parameter];
            if (v!=null)
            {
                if (v.IsString) return Operand.Create(v.StringValue);
                if (v.IsBoolean) return Operand.Create(v.BooleanValue);
                if (v.IsDouble) return Operand.Create(v.NumberValue);
                if (v.IsObject) return Operand.Create(v);
                if (v.IsArray) return Operand.Create(v);
                if (v.IsNull) return Operand.CreateNull();
                return Operand.Create(v);
            }
            return base.GetParameter(parameter);
        }

    }
}
