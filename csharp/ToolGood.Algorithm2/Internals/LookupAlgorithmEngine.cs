using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals
{
    class LookupAlgorithmEngine : AlgorithmEngine
    {
        private static readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");

        public Operand Json;

        protected override Operand GetParameter(string parameter)
        {
            var v = Json.JsonValue[parameter];
            if (v!=null)
            {
                if (v.IsString) return Operand.Create(v.ToString());
                if (v.IsBoolean) return Operand.Create(bool.Parse(v.ToString()));
                if (v.IsDouble) return Operand.Create(double.Parse(v.ToString(), NumberStyles.Any, cultureInfo));
                if (v.IsInt) return Operand.Create(double.Parse(v.ToString(), NumberStyles.Any, cultureInfo));
                if (v.IsLong) return Operand.Create(double.Parse(v.ToString(), NumberStyles.Any, cultureInfo));
                if (v.IsObject) return Operand.Create(v);
                if (v.IsArray) return Operand.Create(v);
                if (v.IsNull) return Operand.CreateNull();
                return Operand.Create(v);
            }
            return base.GetParameter(parameter);
        }

    }
}
