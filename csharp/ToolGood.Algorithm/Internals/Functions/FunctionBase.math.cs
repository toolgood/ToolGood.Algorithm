using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions
{
    #region math

    #region base

    internal class Function_ABS : Function_1
    {
        public Function_ABS(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Abs"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Abs(args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Abs");
        }
    }

    internal class Function_QUOTIENT : Function_2
    {
        public Function_QUOTIENT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Quotient", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Quotient", 2); if (args2.IsError) { return args2; } }

            if (args2.NumberValue == 0) {
                return Operand.Error("Function '{0}' div 0 error!", "Quotient");
            }
            return Operand.Create((int)(args1.NumberValue / args2.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Quotient");
        }
    }

    internal class Function_SIGN : Function_1
    {
        public Function_SIGN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Sign"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Sign(args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Sign");
        }
    }

    internal class Function_SQRT : Function_1
    {
        public Function_SQRT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Sqrt"); if (args1.IsError) { return args1; } }
            if (args1.NumberValue < 0) {
                return Operand.Error("Function '{0}' parameter is error!", "Sqrt");
            }
            return Operand.Create(Math.Sqrt((double)args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Sqrt");
        }
    }

    internal class Function_TRUNC : Function_1
    {
        public Function_TRUNC(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Trunc"); if (args1.IsError) { return args1; } }
            return Operand.Create((int)(args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Trunc");
        }
    }

    internal class Function_GCD : Function_N
    {
        public Function_GCD(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { var aa = funcs[i].Calculate(work); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Gcd"); }

            return Operand.Create(FunctionUtil.F_base_gcd(list));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Gcd");
        }
    }

    internal class Function_LCM : Function_N
    {
        public Function_LCM(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { var aa = funcs[i].Calculate(work); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Lcm"); }

            return Operand.Create(FunctionUtil.F_base_lgm(list));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Lcm");
        }
    }

    internal class Function_COMBIN : Function_2
    {
        public Function_COMBIN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Combin", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Combin", 2); if (args2.IsError) { return args2; } }

            var total = args1.IntValue;
            var count = args2.IntValue;
            if (total < 0 || count < 0 || total < count) {
                return Operand.Error("Function '{0}' parameter is error!", "Combin");
            }
            decimal sum = 1;
            decimal sum2 = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
                sum2 *= (i + 1);
            }
            return Operand.Create(sum / sum2);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Combin");
        }
    }

    internal class Function_PERMUT : Function_2
    {
        public Function_PERMUT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Permut", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Permut", 2); if (args2.IsError) { return args2; } }

            var total = args1.IntValue;
            var count = args2.IntValue;

            double sum = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
            }
            return Operand.Create(sum);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Permut");
        }
    }

    internal class Function_Percentage : Function_1
    {
        public Function_Percentage(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Percentage"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.NumberValue / 100.0m);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            func1.ToString(stringBuilder, false);
            stringBuilder.Append('%');
        }
    }

    #endregion base

    #region trigonometric functions

    internal class Function_DEGREES : Function_1
    {
        public Function_DEGREES(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Degrees"); if (args1.IsError) { return args1; } }
            var z = (double)args1.NumberValue;
            var r = (z / Math.PI * 180);
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Degrees");
        }
    }

    internal class Function_RADIANS : Function_1
    {
        public Function_RADIANS(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Radians"); if (args1.IsError) { return args1; } }
            var r = (double)args1.NumberValue / 180 * Math.PI;
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Radians");
        }
    }

    internal class Function_COS : Function_1
    {
        public Function_COS(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Cos"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Cos((double)args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Cos");
        }
    }

    internal class Function_SIN : Function_1
    {
        public Function_SIN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Sin"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Sin((double)args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Sin");
        }
    }

    internal class Function_TAN : Function_1
    {
        public Function_TAN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Tan"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Tan((double)args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Tan");
        }
    }

    internal class Function_ACOS : Function_1
    {
        public Function_ACOS(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Acos"); if (args1.IsError) { return args1; } }
            var x = args1.NumberValue;
            if (x < -1 || x > 1) {
                return Operand.Error("Function '{0}' parameter is error!", "Acos");
            }
            return Operand.Create(Math.Acos((double)x));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Acos");
        }
    }

    internal class Function_ASIN : Function_1
    {
        public Function_ASIN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Asin"); if (args1.IsError) { return args1; } }
            var x = args1.NumberValue;
            if (x < -1 || x > 1) {
                return Operand.Error("Function '{0}' parameter is error!", "Asin");
            }
            return Operand.Create(Math.Asin((double)x));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Asin");
        }
    }

    internal class Function_ATAN : Function_1
    {
        public Function_ATAN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Atan"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Atan((double)args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Atan");
        }
    }

    internal class Function_ATAN2 : Function_2
    {
        public Function_ATAN2(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Atan2", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Atan2", 2); if (args2.IsError) { return args2; } }
            return Operand.Create(Math.Atan2((double)args2.NumberValue, (double)args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Atan2");
        }
    }

    internal class Function_COT : Function_1
    {
        public Function_COT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Cot"); if (args1.IsError) { return args1; } }
            var d = Math.Tan((double)args1.NumberValue);
            if (d == 0) {
                return Operand.Error("Function '{0}' div 0 error!", "Cot");
            }
            return Operand.Create(1.0 / d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Cot");
        }
    }

    internal class Function_SEC : Function_1
    {
        public Function_SEC(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Sec"); if (args1.IsError) { return args1; } }
            var d = Math.Cos((double)args1.NumberValue);
            if (d == 0) {
                return Operand.Error("Function '{0}' div 0 error!", "Sec");
            }
            return Operand.Create(1.0 / d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Sec");
        }
    }

    internal class Function_CSC : Function_1
    {
        public Function_CSC(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Csc"); if (args1.IsError) { return args1; } }
            var d = Math.Sin((double)args1.NumberValue);
            if (d == 0) {
                return Operand.Error("Function '{0}' div 0 error!", "Csc");
            }
            return Operand.Create(1.0 / d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Csc");
        }
    }

    internal class Function_COSH : Function_1
    {
        public Function_COSH(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Cosh"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Cosh((double)args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Cosh");
        }
    }

    internal class Function_SINH : Function_1
    {
        public Function_SINH(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Sinh"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Sinh((double)args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Sinh");
        }
    }

    internal class Function_TANH : Function_1
    {
        public Function_TANH(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Tanh"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Tanh((double)args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Tanh");
        }
    }

    internal class Function_ACOSH : Function_1
    {
        public Function_ACOSH(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Acosh"); if (args1.IsError) { return args1; } }
            var z = args1.NumberValue;
            if (z < 1) {
                return Operand.Error("Function '{0}' parameter is error!", "Acosh");
            }
            return Operand.Create(Math.Acosh((double)z));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Acosh");
        }
    }

    internal class Function_ASINH : Function_1
    {
        public Function_ASINH(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Asinh"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Asinh((double)args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Asinh");
        }
    }

    internal class Function_ATANH : Function_1
    {
        public Function_ATANH(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Atanh"); if (args1.IsError) { return args1; } }
            var x = args1.NumberValue;
            if (x >= 1 || x <= -1) {
                return Operand.Error("Function '{0}' parameter is error!", "Atanh");
            }
            return Operand.Create(Math.Atanh((double)x));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Atanh");
        }
    }

    internal class Function_FIXED : Function_3
    {
        public Function_FIXED(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var num = 2;
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Fixed", 2); if (args2.IsError) { return args2; } }
                num = args2.IntValue;
            }
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Fixed", 1); if (args1.IsError) { return args1; } }

            var s = Math.Round(args1.NumberValue, num, MidpointRounding.AwayFromZero);
            var no = false;
            if (func3 != null) {
                var args3 = func3.Calculate(work); if (args3.IsNotBoolean) { args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "Fixed", 3); if (args3.IsError) { return args3; } }
                no = args3.BooleanValue;
            }
            if (no == false) {
                return Operand.Create(s.ToString('N' + num.ToString(), CultureInfo.InvariantCulture));
            }
            return Operand.Create(s.ToString(CultureInfo.InvariantCulture));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Fixed");
        }
    }

    #endregion trigonometric functions

    #region transformation

    internal class Function_BIN2OCT : Function_2
    {
        public Function_BIN2OCT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "BIN2OCT", 1); if (args1.IsError) { return args1; } }

            if (Regex.IsMatch(args1.TextValue, "^[01]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "BIN2OCT", 1); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 2), 8);
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "BIN2OCT", 2); if (args2.IsError) { return args2; } }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "BIN2OCT", 2);
            }
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "BIN2OCT");
        }
    }

    internal class Function_BIN2DEC : Function_1
    {
        public Function_BIN2DEC(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work).ToText("Function '{0}' parameter is error!", "BIN2DEC");
            if (args1.IsError) { return args1; }

            if (Regex.IsMatch(args1.TextValue, "^[01]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function '{0}' parameter is error!", "BIN2DEC"); }
            var num = Convert.ToInt32(args1.TextValue, 2);
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "BIN2DEC");
        }
    }

    internal class Function_BIN2HEX : Function_2
    {
        public Function_BIN2HEX(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "BIN2HEX", 1); if (args1.IsError) { return args1; } }

            if (Regex.IsMatch(args1.TextValue, "^[01]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "BIN2HEX", 1); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 2), 16).ToUpper();
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "BIN2HEX", 2); if (args2.IsError) { return args2; } }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "BIN2HEX", 2);
            }
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "BIN2HEX");
        }
    }

    internal class Function_OCT2BIN : Function_2
    {
        public Function_OCT2BIN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "OCT2BIN", 1); if (args1.IsError) { return args1; } }
            if (Regex.IsMatch(args1.TextValue, "^[0-7]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "OCT2BIN", 1); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 8), 2);
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "OCT2BIN", 2); if (args2.IsError) { return args2; } }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "OCT2BIN", 2);
            }
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "OCT2BIN");
        }
    }

    internal class Function_OCT2DEC : Function_1
    {
        public Function_OCT2DEC(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work).ToText("Function '{0}' parameter is error!", "OCT2DEC");
            if (args1.IsError) { return args1; }
            if (Regex.IsMatch(args1.TextValue, "^[0-7]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function '{0}' parameter is error!", "OCT2DEC"); }
            var num = Convert.ToInt32(args1.TextValue, 8);
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "OCT2DEC");
        }
    }

    internal class Function_OCT2HEX : Function_2
    {
        public Function_OCT2HEX(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "OCT2HEX", 1); if (args1.IsError) { return args1; } }

            if (Regex.IsMatch(args1.TextValue, "^[0-7]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "OCT2HEX", 1); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 8), 16).ToUpper();
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "OCT2HEX", 2); if (args2.IsError) { return args2; } }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "OCT2HEX", 2);
            }
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "OCT2HEX");
        }
    }

    internal class Function_HEX2BIN : Function_2
    {
        public Function_HEX2BIN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "HEX2BIN", 1); if (args1.IsError) { return args1; } }

            if (Regex.IsMatch(args1.TextValue, "^[0-9A-Fa-f]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "HEX2BIN", 1); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 16), 2);
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "HEX2BIN", 2); if (args2.IsError) { return args2; } }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "HEX2BIN", 2);
            }
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HEX2BIN");
        }
    }

    internal class Function_HEX2DEC : Function_1
    {
        public Function_HEX2DEC(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work).ToText("Function '{0}' parameter is error!", "HEX2DEC");
            if (args1.IsError) { return args1; }
            if (Regex.IsMatch(args1.TextValue, "^[0-9A-Fa-f]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function '{0}' parameter is error!", "HEX2DEC"); }
            var num = Convert.ToInt32(args1.TextValue, 16);
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HEX2DEC");
        }
    }

    internal class Function_HEX2OCT : Function_2
    {
        public Function_HEX2OCT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "HEX2OCT", 1); if (args1.IsError) { return args1; } }
            if (Regex.IsMatch(args1.TextValue, "^[0-9A-Fa-f]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "HEX2OCT", 1); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 16), 8);
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "HEX2OCT", 2); if (args2.IsError) { return args2; } }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "HEX2OCT", 2);
            }
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HEX2OCT");
        }
    }

    internal class Function_DEC2BIN : Function_2
    {
        public Function_DEC2BIN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "DEC2BIN", 1); if (args1.IsError) { return args1; } }
            var num = Convert.ToString(args1.IntValue, 2);
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "DEC2BIN", 2); if (args2.IsError) { return args2; } }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "DEC2BIN", 2);
            }
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "DEC2BIN");
        }
    }

    internal class Function_DEC2OCT : Function_2
    {
        public Function_DEC2OCT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "DEC2OCT", 1); if (args1.IsError) { return args1; } }
            var num = Convert.ToString(args1.IntValue, 8);
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "DEC2OCT", 2); if (args2.IsError) { return args2; } }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "DEC2OCT", 2);
            }
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "DEC2OCT");
        }
    }

    internal class Function_DEC2HEX : Function_2
    {
        public Function_DEC2HEX(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "DEC2HEX", 1); if (args1.IsError) { return args1; } }
            var num = Convert.ToString(args1.IntValue, 16).ToUpper();
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "DEC2HEX", 2); if (args2.IsError) { return args2; } }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "DEC2HEX", 2);
            }
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "DEC2HEX");
        }
    }

    #endregion transformation

    #region rounding

    internal class Function_ROUNDUP : Function_2
    {
        public Function_ROUNDUP(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "RoundUp", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "RoundUp", 2); if (args2.IsError) { return args2; } }
            if (args1.NumberValue == 0.0m) { return args1; }
            var a = Math.Pow(10, args2.IntValue);
            var b = args1.NumberValue;

            var t = (Math.Ceiling(Math.Abs((double)b) * a)) / a;
            if (b > 0) return Operand.Create(t);
            return Operand.Create(-t);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "RoundUp");
        }
    }

    internal class Function_ROUNDDOWN : Function_2
    {
        public Function_ROUNDDOWN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "RoundDown", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "RoundDown", 2); if (args2.IsError) { return args2; } }
            if (args1.NumberValue == 0.0m) {
                return args1;
            }
            var a = (decimal)Math.Pow(10, args2.IntValue);
            var b = args1.NumberValue;

            b = ((int)(b * a)) / a;
            return Operand.Create(b);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "RoundDown");
        }
    }

    internal class Function_MROUND : Function_2
    {
        public Function_MROUND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "MRound", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "MRound", 2); if (args2.IsError) { return args2; } }
            var a = args2.NumberValue;
            if (a <= 0) { return Operand.Error("Function '{0}' parameter {1} is error!", "MRound", 2); }

            var b = args1.NumberValue;
            var r = Math.Round(b / a, 0, MidpointRounding.AwayFromZero) * a;
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "MRound");
        }
    }

    internal class Function_ROUND : Function_2
    {
        public Function_ROUND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Round", 1); if (args1.IsError) { return args1; } }

            if (func2 == null) {
                return Operand.Create(Math.Round((decimal)args1.NumberValue, 0, MidpointRounding.AwayFromZero));
            }
            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Round", 2); if (args2.IsError) { return args2; } }
            return Operand.Create(Math.Round((decimal)args1.NumberValue, args2.IntValue, MidpointRounding.AwayFromZero));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Round");
        }
    }

    internal class Function_CEILING : Function_2
    {
        public Function_CEILING(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Ceiling", 1); if (args1.IsError) { return args1; } }

            if (func2 == null)
                return Operand.Create(Math.Ceiling(args1.NumberValue));

            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Ceiling", 2); if (args2.IsError) { return args2; } }
            var b = args2.NumberValue;
            if (b == 0) { return Operand.Create(0); }
            if (b < 0) { return Operand.Error("Function '{0}' parameter {1} is error!", "Ceiling", 2); }

            var a = args1.NumberValue;
            var d = Math.Ceiling(a / b) * b;
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Ceiling");
        }
    }

    internal class Function_FLOOR : Function_2
    {
        public Function_FLOOR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Floor", 1); if (args1.IsError) { return args1; } }
            if (func2 == null) return Operand.Create(Math.Floor(args1.NumberValue));

            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Floor", 2); if (args2.IsError) { return args2; } }
            var b = args2.NumberValue;
            if (b >= 1) { return Operand.Create(args1.IntValue); }
            if (b <= 0) { return Operand.Error("Function '{0}' parameter {1} is error!", "Floor", 2); }

            var a = args1.NumberValue;
            var d = Math.Floor(a / b) * b;
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Floor");
        }
    }

    internal class Function_EVEN : Function_1
    {
        public Function_EVEN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Even"); if (args1.IsError) { return args1; } }
            var z = args1.NumberValue;
            if (z % 2 == 0) { return args1; }
            z = Math.Ceiling(z);
            if (z % 2 == 0) { return Operand.Create(z); }
            z++;
            return Operand.Create(z);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Even");
        }
    }

    internal class Function_ODD : Function_1
    {
        public Function_ODD(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Odd"); if (args1.IsError) { return args1; } }
            var z = args1.NumberValue;
            if (z % 2 == 1) { return args1; }
            z = Math.Ceiling(z);
            if (z % 2 == 1) { return Operand.Create(z); }
            z++;
            return Operand.Create(z);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Odd");
        }
    }

    #endregion rounding

    #region RAND

    internal class Function_RAND : FunctionBase
    {
        public Function_RAND()
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
#if NETSTANDARD2_1
            var tick = DateTime.Now.Ticks;
            Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
#else
            Random rand = Random.Shared;
#endif
            return Operand.Create(rand.NextDouble());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("Rand()");
        }
    }

    internal class Function_RANDBETWEEN : Function_2
    {
        public Function_RANDBETWEEN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "RandBetween", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "RandBetween", 2); if (args2.IsError) { return args2; } }
#if NETSTANDARD2_1
            var tick = DateTime.Now.Ticks;
            Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
#else
            Random rand = Random.Shared;
#endif
            return Operand.Create((decimal)rand.NextDouble() * (args2.NumberValue - args1.NumberValue) + args1.NumberValue);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "RandBetween");
        }
    }

    #endregion RAND

    #region power logarithm factorial

    internal class Function_COVARIANCES : Function_2
    {
        public Function_COVARIANCES(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { if (args2.IsError) { return args2; } }

            List<decimal> list1 = new List<decimal>();
            List<decimal> list2 = new List<decimal>();

            var o1 = FunctionUtil.F_base_GetList(args1, list1);
            var o2 = FunctionUtil.F_base_GetList(args2, list2);
            if (o1 == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "CovarIanceS", 1); }
            if (o2 == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "CovarIanceS", 2); }
            if (list1.Count != list2.Count) { return Operand.Error("Function '{0}' parameter's count error!", "CovarIanceS"); }
            if (list1.Count == 1) { return Operand.Error("Function '{0}' parameter's count error!", "CovarIanceS"); }

            var avg1 = list1.Average();
            var avg2 = list2.Average();
            decimal sum = 0;
            for (int i = 0; i < list1.Count; i++) {
                sum += (list1[i] - avg1) * (list2[i] - avg2);
            }
            var val = sum / (list1.Count - 1);
            return Operand.Create(val);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "CovarIanceS");
        }
    }

    internal class Function_COVAR : Function_2
    {
        public Function_COVAR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { if (args2.IsError) { return args2; } }
            List<decimal> list1 = new List<decimal>();
            List<decimal> list2 = new List<decimal>();
            var o1 = FunctionUtil.F_base_GetList(args1, list1);
            var o2 = FunctionUtil.F_base_GetList(args2, list2);
            if (o1 == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "Covar", 1); }
            if (o2 == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "Covar", 2); }
            if (list1.Count != list2.Count) { return Operand.Error("Function '{0}' parameter's count error!", "Covar"); }
            if (list1.Count == 0) { return Operand.Error("Function '{0}' parameter's count error!", "Covar"); }

            var avg1 = list1.Average();
            var avg2 = list2.Average();
            decimal sum = 0;
            for (int i = 0; i < list1.Count; i++) {
                sum += (list1[i] - avg1) * (list2[i] - avg2);
            }
            var val = sum / list1.Count;
            return Operand.Create(val);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Covar");
        }
    }

    internal class Function_FACT : Function_1
    {
        public Function_FACT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Fact"); if (args1.IsError) { return args1; } }
            if (args1.IsError) { return args1; }

            var z = args1.IntValue;
            if (z < 0) {
                return Operand.Error("Function '{0}' parameter is error!", "Fact");
            }
            double d = 1;
            for (int i = 1; i <= z; i++) {
                d *= i;
            }
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Fact");
        }
    }

    internal class Function_FACTDOUBLE : Function_1
    {
        public Function_FACTDOUBLE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "FactDouble"); if (args1.IsError) { return args1; } }
            var z = args1.IntValue;
            if (z < 0) { return Operand.Error("Function '{0}' parameter is error!", "FactDouble"); }

            double d = 1;
            for (int i = z; i > 0; i -= 2) {
                d *= i;
            }
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "FactDouble");
        }
    }

    internal class Function_POWER : Function_2
    {
        public Function_POWER(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Power", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Power", 2); if (args2.IsError) { return args2; } }
            return Operand.Create(Math.Pow((double)args1.NumberValue, (double)args2.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Power");
        }
    }

    internal class Function_EXP : Function_1
    {
        public Function_EXP(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Exp"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Exp((double)args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Exp");
        }
    }

    internal class Function_LN : Function_1
    {
        public Function_LN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Ln"); if (args1.IsError) { return args1; } }
            var z = args1.NumberValue;
            if (z <= 0) {
                return Operand.Error("Function '{0}' parameter is error!", "Ln");
            }
            return Operand.Create(Math.Log((double)z));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Ln");
        }
    }

    internal class Function_LOG : Function_2
    {
        public Function_LOG(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Log", 1); if (args1.IsError) { return args1; } }
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Log", 2); if (args2.IsError) { return args2; } }
                return Operand.Create(Math.Log((double)args1.NumberValue, (double)args2.NumberValue));
            }
            return Operand.Create(Math.Log((double)args1.NumberValue, 10));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Log");
        }
    }

    internal class Function_MULTINOMIAL : Function_N
    {
        public Function_MULTINOMIAL(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Calculate(work); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Multinomial"); }

            int sum = 0;
            int n = 1;
            for (int i = 0; i < list.Count; i++) {
                var a = (int)list[i]; // 小于等于0 时，按0处理
                n *= FunctionUtil.F_base_Factorial(a);
                sum += a;
            }

            var r = FunctionUtil.F_base_Factorial(sum) / n;
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Multinomial");
        }
    }

    internal class Function_PRODUCT : Function_N
    {
        public Function_PRODUCT(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { var aa = funcs[i].Calculate(work); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Product"); }

            decimal d = 1;
            for (int i = 0; i < list.Count; i++) {
                var a = list[i];
                d *= a;
            }
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Product");
        }
    }

    internal class Function_SQRTPI : Function_1
    {
        public Function_SQRTPI(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "SqrtPI"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Sqrt((double)args1.NumberValue * Math.PI));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "SqrtPI");
        }
    }

    internal class Function_SUMSQ : Function_N
    {
        public Function_SUMSQ(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { var aa = funcs[i].Calculate(work); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "SumSQ"); }

            decimal d = 0;
            for (int i = 0; i < list.Count; i++) {
                var a = list[i];
                d += a * a;
            }
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "SumSQ");
        }
    }

    #endregion power logarithm factorial

    #endregion math
}
