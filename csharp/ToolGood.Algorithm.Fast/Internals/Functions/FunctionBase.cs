using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.math;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions
{
    public class Work
    {
        public event Func<mathParser.ProgContext, string, Operand> GetParameter;
        public event Func<mathParser.ProgContext, string, List<Operand>, Operand> DiyFunction;
        public int excelIndex;
        public bool useLocalTime;
        public DistanceUnitType DistanceUnit = DistanceUnitType.M;
        public AreaUnitType AreaUnit = AreaUnitType.M2;
        public VolumeUnitType VolumeUnit = VolumeUnitType.M3;
        public MassUnitType MassUnit = MassUnitType.KG;

    }

    public abstract class FunctionBase
    {
        public abstract Operand Accept(Work work);
    }

    public class Function_Value : FunctionBase
    {
        private Operand _value;

        public Function_Value(Operand value)
        {
            _value = value;
        }

        public override Operand Accept(Work work)
        {
            return _value;
        }
    }

    public abstract class Function_1 : FunctionBase
    {
        protected FunctionBase func1;

        protected Function_1(FunctionBase func1)
        {
            this.func1 = func1;
        }
    }

    public abstract class Function_2 : FunctionBase
    {
        protected FunctionBase func1;
        protected FunctionBase func2;
        public Function_2(FunctionBase func1, FunctionBase func2)
        {
            this.func1 = func1;
            this.func2 = func2;
        }
    }

    public abstract class Function_3 : FunctionBase
    {
        protected FunctionBase func1;
        protected FunctionBase func2;
        protected FunctionBase func3;

        protected Function_3(FunctionBase func1, FunctionBase func2, FunctionBase func3)
        {
            this.func1 = func1;
            this.func2 = func2;
            this.func3 = func3;
        }
    }
    public abstract class Function_4 : FunctionBase
    {
        protected FunctionBase func1;
        protected FunctionBase func2;
        protected FunctionBase func3;
        protected FunctionBase func4;

        protected Function_4(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4)
        {
            this.func1 = func1;
            this.func2 = func2;
            this.func3 = func3;
            this.func4 = func4;
        }
    }


    public abstract class Function_N : FunctionBase
    {
        protected FunctionBase[] funcs;

        protected Function_N(FunctionBase[] funcs)
        {
            this.funcs = funcs;
        }
    }


    #region * / % + - &
    public class Function_Mul : Function_2
    {
        public Function_Mul(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }

            if (args1.Type == OperandType.TEXT) {
                if (decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args1 = Operand.Create(d);
                } else if (bool.TryParse(args1.TextValue, out bool b)) {
                    args1 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args1 = Operand.Create(ts);
                } else if (DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args1 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Two types cannot be multiplied ");
                }
            }
            if (args2.Type == OperandType.TEXT) {
                if (decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args2 = Operand.Create(d);
                } else if (bool.TryParse(args2.TextValue, out bool b)) {
                    args2 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args2.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args2 = Operand.Create(ts);
                } else if (DateTime.TryParse(args2.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args2 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Two types cannot be multiplied");
                }
            }
            if (args1.Type == OperandType.DATE) {
                if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '*' parameter 2 is error!"); if (args2.IsError) { return args2; } }
                if (args2.NumberValue == 1) { return args1; }
                return Operand.Create((MyDate)(args1.DateValue * args2.NumberValue));
            } else if (args2.Type == OperandType.DATE) {
                if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '*' parameter 1 is error!"); if (args1.IsError) { return args1; } }
                if (args1.NumberValue == 1) { return args2; }
                return Operand.Create((MyDate)(args2.DateValue * args1.NumberValue));
            }

            if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '*' parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '*' parameter 2 is error!"); if (args2.IsError) { return args2; } }
            if (args1.NumberValue == 1) { return args2; }
            if (args2.NumberValue == 1) { return args1; }

            return Operand.Create(args1.NumberValue * args2.NumberValue);
        }
    }

    public class Function_Div : Function_2
    {
        public Function_Div(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }

            if (args1.Type == OperandType.TEXT) {
                if (decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args1 = Operand.Create(d);
                } else if (bool.TryParse(args1.TextValue, out bool b)) {
                    args1 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args1 = Operand.Create(ts);
                } else if (DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args1 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Two types cannot be divided ");
                }
            }
            if (args2.Type == OperandType.TEXT) {
                if (decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args2 = Operand.Create(d);
                } else if (bool.TryParse(args2.TextValue, out bool b)) {
                    args2 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args2.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args2 = Operand.Create(ts);
                } else if (DateTime.TryParse(args2.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args2 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Two types cannot be divided");
                }
            }
            if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '/' parameter 2 is error!"); if (args2.IsError) { return args2; } }
            if (args2.NumberValue == 0) { return Operand.Error("Div 0 is error!"); }
            if (args2.NumberValue == 1) { return args1; }

            if (args1.Type == OperandType.DATE) { return Operand.Create(args1.DateValue / args2.NumberValue); }
            if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '/' parameter 1 is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(args1.NumberValue / args2.NumberValue);
        }
    }

    public class Function_Mod : Function_2
    {
        public Function_Mod(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }

            if (args1.Type == OperandType.TEXT) {
                if (decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args1 = Operand.Create(d);
                } else if (bool.TryParse(args1.TextValue, out bool b)) {
                    args1 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args1 = Operand.Create(ts);
                } else if (DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args1 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Two types cannot be divided ");
                }
            }
            if (args2.Type == OperandType.TEXT) {
                if (decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args2 = Operand.Create(d);
                } else if (bool.TryParse(args2.TextValue, out bool b)) {
                    args2 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args2.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args2 = Operand.Create(ts);
                } else if (DateTime.TryParse(args2.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args2 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Two types cannot be divided");
                }
            }
            if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function % parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function % parameter 2 is error!"); if (args2.IsError) { return args2; } }
            if (args2.NumberValue == 0) { return Operand.Error("Div 0 is error!"); }

            return Operand.Create(args1.NumberValue % args2.NumberValue);
        }
    }

    public class Function_Add : Function_2
    {
        public Function_Add(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }

            if (args1.Type == OperandType.TEXT) {
                if (decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args1 = Operand.Create(d);
                } else if (bool.TryParse(args1.TextValue, out bool b)) {
                    args1 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args1 = Operand.Create(ts);
                } else if (DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args1 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Function '+'   is error");
                }
            }
            if (args2.Type == OperandType.TEXT) {
                if (decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args2 = Operand.Create(d);
                } else if (bool.TryParse(args2.TextValue, out bool b)) {
                    args2 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args2.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args2 = Operand.Create(ts);
                } else if (DateTime.TryParse(args2.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args2 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Function '+' is error");
                }
            }
            if (args1.Type == OperandType.DATE) {
                if (args2.Type == OperandType.DATE) return Operand.Create(args1.DateValue + args2.DateValue);
                if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '+' parameter 2 is error!"); if (args2.IsError) { return args2; } }
                if (args2.NumberValue == 0) { return args1; }
                return Operand.Create(args1.DateValue + args2.NumberValue);
            } else if (args2.Type == OperandType.DATE) {
                if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '+' parameter 1 is error!"); if (args1.IsError) { return args1; } }
                if (args1.NumberValue == 0) { return args2; }
                return Operand.Create(args2.DateValue + args1.NumberValue);
            }
            if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '+' parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '+' parameter 2 is error!"); if (args2.IsError) { return args2; } }
            if (args1.NumberValue == 0) { return args2; }
            if (args2.NumberValue == 0) { return args1; }

            return Operand.Create(args1.NumberValue + args2.NumberValue);
        }
    }
    public class Function_Sub : Function_2
    {
        public Function_Sub(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }

            if (args1.Type == OperandType.TEXT) {
                if (decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args1 = Operand.Create(d);
                } else if (bool.TryParse(args1.TextValue, out bool b)) {
                    args1 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args1 = Operand.Create(ts);
                } else if (DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args1 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Function '+'   is error");
                }
            }
            if (args2.Type == OperandType.TEXT) {
                if (decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args2 = Operand.Create(d);
                } else if (bool.TryParse(args2.TextValue, out bool b)) {
                    args2 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args2.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args2 = Operand.Create(ts);
                } else if (DateTime.TryParse(args2.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args2 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Function '+' is error");
                }
            }
            if (args1.Type == OperandType.DATE) {
                if (args2.Type == OperandType.DATE) return Operand.Create(args1.DateValue - args2.DateValue);
                if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '-' parameter 2 is error!"); if (args2.IsError) { return args2; } }
                if (args2.NumberValue == 0) { return args1; }
                return Operand.Create(args1.DateValue - args2.NumberValue);
            } else if (args2.Type == OperandType.DATE) {
                if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '-' parameter 1 is error!"); if (args1.IsError) { return args1; } }
                return Operand.Create(args1.NumberValue - args2.DateValue);
            }
            if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '-' parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '-' parameter 2 is error!"); if (args2.IsError) { return args2; } }
            if (args2.NumberValue == 0) { return args1; }

            return Operand.Create(args1.NumberValue - args2.NumberValue);
        }
    }

    public class Function_Connect : Function_2
    {
        public Function_Connect(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }

            if (args1.IsNull) {
                if (args2.IsNull) return args1;
                return args2.ToText("Function '&' parameter 2 is error!");
            } else if (args2.IsNull) {
                return args1.ToText("Function '&' parameter 1 is error!");
            }
            if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function '&' parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function '&' parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(args1.TextValue + args2.TextValue);
        }
    }

    #endregion

    #region == != >= <= > <
    public class Function_EQ : Function_2
    {
        public Function_EQ(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }

            if (args1.Type == args2.Type) {
                if (args1.Type == OperandType.NUMBER) {
                    return Operand.Create(args1.NumberValue == args2.NumberValue);
                } else if (args1.Type == OperandType.TEXT) {
                    return Operand.Create(args1.TextValue == args2.TextValue);
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.BOOLEAN) {
                    args1 = args1.ToNumber();
                    args2 = args2.ToNumber();
                    return Operand.Create(args1.NumberValue == args2.NumberValue);
                } else if (args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    args2 = args2.ToText();
                    return Operand.Create(args1.TextValue == args2.TextValue);
                } else if (args1.Type == OperandType.NULL) {
                    return Operand.True;
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args2.Type == OperandType.TEXT) {
                if (args1.Type == OperandType.BOOLEAN) {
                    var a = args2.ToBoolean();
                    if (a.IsError == false) {
                        return a.BooleanValue == args1.BooleanValue ? Operand.True : Operand.False;
                    }
                    args1 = args1.ToText();
                    return Operand.Create(args1.TextValue == args2.TextValue);
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.NUMBER || args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    return Operand.Create(args1.TextValue == args2.TextValue);
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args1.Type == OperandType.JSON || args2.Type == OperandType.JSON
                  || args1.Type == OperandType.ARRARY || args2.Type == OperandType.ARRARY
                  || args1.Type == OperandType.ARRARYJSON || args2.Type == OperandType.ARRARYJSON
                  ) {
                return Operand.Error("Function compare is error.");
            }
            if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '==' parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '==' parameter 2 is error!"); if (args2.IsError) { return args2; } }

            return Operand.Create(args1.NumberValue == args2.NumberValue);
        }
    }

    public class Function_NE : Function_2
    {
        public Function_NE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }

            if (args1.Type == args2.Type) {
                if (args1.Type == OperandType.NUMBER) {
                    return Operand.Create(args1.NumberValue != args2.NumberValue);
                } else if (args1.Type == OperandType.TEXT) {
                    return Operand.Create(args1.TextValue != args2.TextValue);
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.BOOLEAN) {
                    args1 = args1.ToNumber();
                    args2 = args2.ToNumber();
                    return Operand.Create(args1.NumberValue != args2.NumberValue);
                } else if (args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    args2 = args2.ToText();
                    return Operand.Create(args1.TextValue != args2.TextValue);
                } else if (args1.Type == OperandType.NULL) {
                    return Operand.False;
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args2.Type == OperandType.TEXT) {
                if (args1.Type == OperandType.BOOLEAN) {
                    var a = args2.ToBoolean();
                    if (a.IsError == false) {
                        return a.BooleanValue != args1.BooleanValue ? Operand.True : Operand.False;
                    }
                    args1 = args1.ToText();
                    return Operand.Create(args1.TextValue != args2.TextValue);
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.NUMBER || args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    return Operand.Create(args1.TextValue != args2.TextValue);
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args1.Type == OperandType.JSON || args2.Type == OperandType.JSON
                  || args1.Type == OperandType.ARRARY || args2.Type == OperandType.ARRARY
                  || args1.Type == OperandType.ARRARYJSON || args2.Type == OperandType.ARRARYJSON
                  ) {
                return Operand.Error("Function compare is error.");
            }
            if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '!=' parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '!=' parameter 2 is error!"); if (args2.IsError) { return args2; } }

            return Operand.Create(args1.NumberValue != args2.NumberValue);
        }
    }

    public class Function_GE : Function_2
    {
        public Function_GE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }

            if (args1.Type == args2.Type) {
                if (args1.Type == OperandType.NUMBER) {
                    return Operand.Create(args1.NumberValue >= args2.NumberValue);
                } else if (args1.Type == OperandType.TEXT) {
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r >= 0 ? Operand.True : Operand.False;
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.BOOLEAN) {
                    args1 = args1.ToNumber();
                    args2 = args2.ToNumber();
                    return Operand.Create(args1.NumberValue >= args2.NumberValue);
                } else if (args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    args2 = args2.ToText();
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r >= 0 ? Operand.True : Operand.False;
                } else if (args1.Type == OperandType.NULL) {
                    return Operand.True;
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args2.Type == OperandType.TEXT) {
                if (args1.Type == OperandType.BOOLEAN) {
                    var a = args2.ToBoolean();
                    if (a.IsError == false) {
                        return a.BooleanValue != args1.BooleanValue ? Operand.True : Operand.False;
                    }
                    args1 = args1.ToText();
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r >= 0 ? Operand.True : Operand.False;
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.NUMBER || args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r >= 0 ? Operand.True : Operand.False;
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args1.Type == OperandType.JSON || args2.Type == OperandType.JSON
                  || args1.Type == OperandType.ARRARY || args2.Type == OperandType.ARRARY
                  || args1.Type == OperandType.ARRARYJSON || args2.Type == OperandType.ARRARYJSON
                  ) {
                return Operand.Error("Function compare is error.");
            }
            if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '!=' parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '!=' parameter 2 is error!"); if (args2.IsError) { return args2; } }

            return Operand.Create(args1.NumberValue >= args2.NumberValue);
        }
    }

    public class Function_LE : Function_2
    {
        public Function_LE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }

            if (args1.Type == args2.Type) {
                if (args1.Type == OperandType.NUMBER) {
                    return Operand.Create(args1.NumberValue <= args2.NumberValue);
                } else if (args1.Type == OperandType.TEXT) {
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r <= 0 ? Operand.True : Operand.False;
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.BOOLEAN) {
                    args1 = args1.ToNumber();
                    args2 = args2.ToNumber();
                    return Operand.Create(args1.NumberValue <= args2.NumberValue);
                } else if (args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    args2 = args2.ToText();
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r <= 0 ? Operand.True : Operand.False;
                } else if (args1.Type == OperandType.NULL) {
                    return Operand.True;
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args2.Type == OperandType.TEXT) {
                if (args1.Type == OperandType.BOOLEAN) {
                    var a = args2.ToBoolean();
                    if (a.IsError == false) {
                        return a.BooleanValue != args1.BooleanValue ? Operand.True : Operand.False;
                    }
                    args1 = args1.ToText();
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r <= 0 ? Operand.True : Operand.False;
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.NUMBER || args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r <= 0 ? Operand.True : Operand.False;
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args1.Type == OperandType.JSON || args2.Type == OperandType.JSON
                  || args1.Type == OperandType.ARRARY || args2.Type == OperandType.ARRARY
                  || args1.Type == OperandType.ARRARYJSON || args2.Type == OperandType.ARRARYJSON
                  ) {
                return Operand.Error("Function compare is error.");
            }
            if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '!=' parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '!=' parameter 2 is error!"); if (args2.IsError) { return args2; } }

            return Operand.Create(args1.NumberValue <= args2.NumberValue);
        }
    }

    public class Function_GT : Function_2
    {
        public Function_GT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }

            if (args1.Type == args2.Type) {
                if (args1.Type == OperandType.NUMBER) {
                    return Operand.Create(args1.NumberValue > args2.NumberValue);
                } else if (args1.Type == OperandType.TEXT) {
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r > 0 ? Operand.True : Operand.False;
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.BOOLEAN) {
                    args1 = args1.ToNumber();
                    args2 = args2.ToNumber();
                    return Operand.Create(args1.NumberValue > args2.NumberValue);
                } else if (args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    args2 = args2.ToText();
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r > 0 ? Operand.True : Operand.False;
                } else if (args1.Type == OperandType.NULL) {
                    return Operand.True;
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args2.Type == OperandType.TEXT) {
                if (args1.Type == OperandType.BOOLEAN) {
                    var a = args2.ToBoolean();
                    if (a.IsError == false) {
                        return a.BooleanValue != args1.BooleanValue ? Operand.True : Operand.False;
                    }
                    args1 = args1.ToText();
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r > 0 ? Operand.True : Operand.False;
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.NUMBER || args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r > 0 ? Operand.True : Operand.False;
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args1.Type == OperandType.JSON || args2.Type == OperandType.JSON
                  || args1.Type == OperandType.ARRARY || args2.Type == OperandType.ARRARY
                  || args1.Type == OperandType.ARRARYJSON || args2.Type == OperandType.ARRARYJSON
                  ) {
                return Operand.Error("Function compare is error.");
            }
            if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '!=' parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '!=' parameter 2 is error!"); if (args2.IsError) { return args2; } }

            return Operand.Create(args1.NumberValue > args2.NumberValue);
        }
    }

    public class Function_LT : Function_2
    {
        public Function_LT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }

            if (args1.Type == args2.Type) {
                if (args1.Type == OperandType.NUMBER) {
                    return Operand.Create(args1.NumberValue < args2.NumberValue);
                } else if (args1.Type == OperandType.TEXT) {
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r < 0 ? Operand.True : Operand.False;
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.BOOLEAN) {
                    args1 = args1.ToNumber();
                    args2 = args2.ToNumber();
                    return Operand.Create(args1.NumberValue < args2.NumberValue);
                } else if (args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    args2 = args2.ToText();
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r < 0 ? Operand.True : Operand.False;
                } else if (args1.Type == OperandType.NULL) {
                    return Operand.True;
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args2.Type == OperandType.TEXT) {
                if (args1.Type == OperandType.BOOLEAN) {
                    var a = args2.ToBoolean();
                    if (a.IsError == false) {
                        return a.BooleanValue != args1.BooleanValue ? Operand.True : Operand.False;
                    }
                    args1 = args1.ToText();
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r < 0 ? Operand.True : Operand.False;
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.NUMBER || args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                    return r < 0 ? Operand.True : Operand.False;
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args1.Type == OperandType.JSON || args2.Type == OperandType.JSON
                  || args1.Type == OperandType.ARRARY || args2.Type == OperandType.ARRARY
                  || args1.Type == OperandType.ARRARYJSON || args2.Type == OperandType.ARRARYJSON
                  ) {
                return Operand.Error("Function compare is error.");
            }
            if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '!=' parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '!=' parameter 2 is error!"); if (args2.IsError) { return args2; } }

            return Operand.Create(args1.NumberValue < args2.NumberValue);
        }
    }

    #endregion

    #region AND OR
    public class Function_AND : Function_2
    {
        public Function_AND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            // 程序 && and || or 与 excel的  AND(x,y) OR(x,y) 有区别
            // 在excel内 AND(x,y) OR(x,y) 先报错，
            // 在程序中，&& and  有true 直接返回true 就不会检测下一个会不会报错
            // 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
            var args1 = func1.Accept(work); if (args1.Type != OperandType.BOOLEAN) { args1 = args1.ToBoolean(); if (args1.IsError) { return args1; } }
            if (args1.BooleanValue == false) return Operand.False;
            return func2.Accept(work).ToBoolean();
        }
    }
    public class Function_OR : Function_2
    {
        public Function_OR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            // 程序 && and || or 与 excel的  AND(x,y) OR(x,y) 有区别
            // 在excel内 AND(x,y) OR(x,y) 先报错，
            // 在程序中，&& and  有true 直接返回true 就不会检测下一个会不会报错
            // 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
            var args1 = func1.Accept(work); if (args1.Type != OperandType.BOOLEAN) { args1 = args1.ToBoolean(); if (args1.IsError) { return args1; } }
            if (args1.BooleanValue) return Operand.True;
            return func2.Accept(work).ToBoolean();
        }
    }

    public class Function_AND_N : Function_N
    {
        public Function_AND_N(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var index = 1;
            bool b = true;
            foreach (var item in funcs) {
                var a = item.Accept(work).ToBoolean($"Function AND parameter {index++} is error!");
                if (a.IsError) { return a; }
                if (a.BooleanValue == false) b = false;
            }
            return b ? Operand.True : Operand.False;
        }
    }
    public class Function_OR_N : Function_N
    {
        public Function_OR_N(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var index = 1;
            bool b = false;
            foreach (var item in funcs) {
                var a = item.Accept(work).ToBoolean($"Function AND parameter {index++} is error!");
                if (a.IsError) { return a; }
                if (a.BooleanValue) b = true;
            }
            return b ? Operand.True : Operand.False;
        }
    }

    #endregion

    #region flow
    public class Function_IF : Function_3
    {
        public Function_IF(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.BOOLEAN) { args1 = args1.ToBoolean("Function IF first parameter is error!"); if (args1.IsError) { return args1; } }
            if (args1.BooleanValue) return func2.Accept(work);
            return func3.Accept(work);
        }
    }
    public class Function_IFERROR : Function_3
    {
        public Function_IFERROR(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.IsError) { return func2.Accept(work); }
            return func3.Accept(work);
        }
    }
    public class Function_ISNUMBER : Function_1
    {
        public Function_ISNUMBER(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type == OperandType.NUMBER) { return Operand.True; }
            return Operand.False;
        }
    }
    public class Function_ISTEXT : Function_1
    {
        public Function_ISTEXT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type == OperandType.TEXT) { return Operand.True; }
            return Operand.False;
        }
    }
    public class Function_ISERROR : Function_2
    {
        public Function_ISERROR(FunctionBase func1) : base(func1, null)
        {
        }
        public Function_ISERROR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (func2 != null) {
                if (args1.IsError) { return func2.Accept(work); }
                return args1;
            }
            if (args1.IsError) { return Operand.True; }
            return Operand.False;
        }
    }
    public class Function_ISNULL : Function_2
    {
        public Function_ISNULL(FunctionBase func1) : base(func1, null)
        {
        }
        public Function_ISNULL(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (func2 != null) {
                if (args1.IsNull) { return func2.Accept(work); }
                return args1;
            }
            if (args1.IsNull) { return Operand.True; }
            return Operand.False;
        }
    }
    public class Function_ISNULLORERROR : Function_2
    {
        public Function_ISNULLORERROR(FunctionBase func1) : base(func1, null)
        {
        }
        public Function_ISNULLORERROR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (func2 != null) {
                if (args1.IsNull || args1.IsError) { return func2.Accept(work); }
                return args1;
            }
            if (args1.IsNull || args1.IsError) { return Operand.True; }
            return Operand.False;
        }
    }
    public class Function_ISEVEN : Function_1
    {
        public Function_ISEVEN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type == OperandType.NUMBER) {
                if (args1.IntValue % 2 == 0) { return Operand.True; }
            }
            return Operand.False;
        }
    }
    public class Function_ISODD : Function_1
    {
        public Function_ISODD(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type == OperandType.NUMBER) {
                if (args1.IntValue % 2 == 1) { return Operand.True; }
            }
            return Operand.False;
        }
    }
    public class Function_ISLOGICAL : Function_1
    {
        public Function_ISLOGICAL(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type == OperandType.BOOLEAN) { return Operand.True; }
            return Operand.False;
        }
    }
    public class Function_ISNONTEXT : Function_1
    {
        public Function_ISNONTEXT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type != OperandType.TEXT) { return Operand.True; }
            return Operand.False;
        }
    }
    public class Function_NOT : Function_1
    {
        public Function_NOT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.IsError) { return args1; }
            return args1.BooleanValue ? Operand.False : Operand.True;
        }
    }
    #endregion

    #region math

    #region base
    public class Function_ABS : Function_1
    {
        public Function_ABS(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ABS parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Abs(args1.NumberValue));
        }
    }

    public class Function_QUOTIENT : Function_2
    {
        public Function_QUOTIENT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function QUOTIENT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function QUOTIENT parameter 2 is error!"); if (args2.IsError) { return args2; } }

            if (args2.NumberValue == 0) {
                return Operand.Error("Function QUOTIENT div 0 error!");
            }
            return Operand.Create((int)(args1.NumberValue / args2.NumberValue));
        }
    }

    public class Function_SIGN : Function_1
    {
        public Function_SIGN(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SIGN parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Sign(args1.NumberValue));
        }
    }

    public class Function_SQRT : Function_1
    {
        public Function_SQRT(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SIGN parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Sqrt((double)args1.NumberValue));
        }
    }

    public class Function_TRUNC : Function_1
    {
        public Function_TRUNC(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SIGN parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create((int)(args1.NumberValue));
        }
    }

    public class Function_GCD : Function_N
    {
        public Function_GCD(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { var aa = funcs[i].Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function GCD parameter is error!"); }

            return Operand.Create(FunctionUtil.F_base_gcd(list));
        }
    }

    public class Function_LCM : Function_N
    {
        public Function_LCM(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { var aa = funcs[i].Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }


            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function LCM parameter is error!"); }

            return Operand.Create(FunctionUtil.F_base_lgm(list));
        }
    }

    public class Function_COMBIN : Function_2
    {
        public Function_COMBIN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function COMBIN parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function COMBIN parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var total = args1.IntValue;
            var count = args2.IntValue;
            decimal sum = 1;
            decimal sum2 = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
                sum2 *= (i + 1);
            }
            return Operand.Create(sum / sum2);
        }
    }

    public class Function_PERMUT : Function_2
    {
        public Function_PERMUT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function PERMUT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function PERMUT parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var total = args1.IntValue;
            var count = args2.IntValue;

            double sum = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
            }
            return Operand.Create(sum);
        }
    }

    public class Function_Percentage : Function_1
    {
        public Function_Percentage(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function Percentage parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.NumberValue / 100.0m);
        }
    }

    #endregion

    #region trigonometric functions

    public class Function_DEGREES : Function_1
    {
        public Function_DEGREES(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function DEGREES parameter is error!"); if (args1.IsError) { return args1; } }
            var z = (double)args1.NumberValue;
            var r = (z / Math.PI * 180);
            return Operand.Create(r);
        }
    }
    public class Function_RADIANS : Function_1
    {
        public Function_RADIANS(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function RADIANS parameter is error!"); if (args1.IsError) { return args1; } }
            var r = (double)args1.NumberValue / 180 * Math.PI;
            return Operand.Create(r);
        }
    }
    public class Function_COS : Function_1
    {
        public Function_COS(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function COS parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Cos((double)args1.NumberValue));
        }
    }
    public class Function_SIN : Function_1
    {
        public Function_SIN(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SIN parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Sin((double)args1.NumberValue));
        }
    }
    public class Function_TAN : Function_1
    {
        public Function_TAN(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TAN parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Tan((double)args1.NumberValue));
        }
    }
    public class Function_ACOS : Function_1
    {
        public Function_ACOS(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ACOS parameter is error!"); if (args1.IsError) { return args1; } }
            var x = args1.NumberValue;
            if (x < -1 || x > 1) {
                return Operand.Error("Function ACOS parameter is error!");
            }
            return Operand.Create(Math.Acos((double)x));
        }
    }
    public class Function_ASIN : Function_1
    {
        public Function_ASIN(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ASIN parameter is error!"); if (args1.IsError) { return args1; } }
            var x = args1.NumberValue;
            if (x < -1 || x > 1) {
                return Operand.Error("Function ASIN parameter is error!");
            }
            return Operand.Create(Math.Asin((double)x));
        }
    }
    public class Function_ATAN : Function_1
    {
        public Function_ATAN(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ATAN parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Atan((double)args1.NumberValue));
        }
    }
    public class Function_ATAN2 : Function_2
    {
        public Function_ATAN2(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ATAN2 parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ATAN2 parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(Math.Atan2((double)args1.NumberValue, (double)args2.NumberValue));
        }
    }
    public class Function_COT : Function_1
    {
        public Function_COT(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function COT parameter is error!"); if (args1.IsError) { return args1; } }
            var d = Math.Tan((double)args1.NumberValue);
            if (d == 0) {
                return Operand.Error("Function COT div 0 error!");
            }
            return Operand.Create(1.0 / d);
        }
    }
    public class Function_SEC : Function_1
    {
        public Function_SEC(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SEC parameter is error!"); if (args1.IsError) { return args1; } }
            var d = Math.Cos((double)args1.NumberValue);
            if (d == 0) {
                return Operand.Error("Function SEC div 0 error!");
            }
            return Operand.Create(1.0 / d);
        }
    }
    public class Function_CSC : Function_1
    {
        public Function_CSC(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function CSC parameter is error!"); if (args1.IsError) { return args1; } }
            var d = Math.Sin((double)args1.NumberValue);
            if (d == 0) {
                return Operand.Error("Function CSC div 0 error!");
            }
            return Operand.Create(1.0 / d);
        }
    }
    public class Function_COSH : Function_1
    {
        public Function_COSH(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function COSH parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Cosh((double)args1.NumberValue));
        }
    }
    public class Function_SINH : Function_1
    {
        public Function_SINH(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SINH parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Sinh((double)args1.NumberValue));
        }
    }
    public class Function_TANH : Function_1
    {
        public Function_TANH(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TANH parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Tanh((double)args1.NumberValue));
        }
    }
    public class Function_ACOSH : Function_1
    {
        public Function_ACOSH(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ACOSH parameter is error!"); if (args1.IsError) { return args1; } }
            var z = args1.NumberValue;
            if (z < 1) {
                return Operand.Error("Function ACOSH parameter is error!");
            }
            return Operand.Create(Math.Acosh((double)z));
        }
    }
    public class Function_ASINH : Function_1
    {
        public Function_ASINH(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ASINH parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Asinh((double)args1.NumberValue));
        }
    }
    public class Function_ATANH : Function_1
    {
        public Function_ATANH(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ATANH parameter is error!"); if (args1.IsError) { return args1; } }
            var x = args1.NumberValue;
            if (x >= 1 || x <= -1) {
                return Operand.Error("Function ATANH parameter is error!");
            }
            return Operand.Create(Math.Atanh((double)x));
        }
    }
    public class Function_FIXED : Function_N
    {
        public Function_FIXED(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var num = 2;
            if (funcs.Length > 1) {
                var args2 = funcs[1].Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function FIXED parameter 2 is error!"); if (args2.IsError) { return args2; } }
                num = args2.IntValue;
            }
            var args1 = funcs[0].Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FIXED parameter 1 is error!"); if (args1.IsError) { return args1; } }

            var s = Math.Round(args1.NumberValue, num, MidpointRounding.AwayFromZero);
            var no = false;
            if (funcs.Length == 3) {
                var args3 = funcs[2].Accept(work); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function FIXED parameter 3 is error!"); if (args3.IsError) { return args3; } }
                no = args3.BooleanValue;
            }
            if (no == false) {
                return Operand.Create(s.ToString('N' + num.ToString(), CultureInfo.InvariantCulture));
            }
            return Operand.Create(s.ToString(CultureInfo.InvariantCulture));
        }
    }

    #endregion

    #region transformation
    public class Function_BIN2OCT : Function_N
    {
        public Function_BIN2OCT(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToText("Function BIN2OCT parameter 1 is error!");
            if (args1.IsError) { return args1; }

            if (Regex.IsMatch(args1.TextValue, "^[01]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function BIN2OCT parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 2), 8);
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function BIN2OCT parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function BIN2OCT parameter 2 is error!");
            }
            return Operand.Create(num);
        }
    }

    public class Function_BIN2DEC : Function_1
    {
        public Function_BIN2DEC(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work).ToText("Function BIN2DEC parameter is error!");
            if (args1.IsError) { return args1; }

            if (Regex.IsMatch(args1.TextValue, "^[01]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function BIN2DEC parameter is error!"); }
            var num = Convert.ToInt32(args1.TextValue, 2);
            return Operand.Create(num);
        }
    }

    public class Function_BIN2HEX : Function_N
    {
        public Function_BIN2HEX(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToText("Function BIN2HEX parameter 1 is error!");
            if (args1.IsError) { return args1; }

            if (Regex.IsMatch(args1.TextValue, "^[01]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function BIN2HEX parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 2), 16).ToUpper();
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function BIN2HEX parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function BIN2HEX parameter 2 is error!");
            }
            return Operand.Create(num);

        }
    }
    public class Function_OCT2BIN : Function_N
    {
        public Function_OCT2BIN(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }
            var args1 = args[0].ToText("Function OCT2BIN parameter 1 is error!");
            if (args1.IsError) { return args1; }
            if (Regex.IsMatch(args1.TextValue, "^[0-7]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function OCT2BIN parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 8), 2);
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function OCT2BIN parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function OCT2BIN parameter 2 is error!");
            }
            return Operand.Create(num);
        }
    }
    public class Function_OCT2DEC : Function_1
    {
        public Function_OCT2DEC(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work).ToText("Function OCT2DEC parameter is error!");
            if (args1.IsError) { return args1; }
            if (Regex.IsMatch(args1.TextValue, "^[0-7]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function OCT2DEC parameter is error!"); }
            var num = Convert.ToInt32(args1.TextValue, 8);
            return Operand.Create(num);
        }
    }
    public class Function_OCT2HEX : Function_N
    {
        public Function_OCT2HEX(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }
            var args1 = args[0].ToText("Function OCT2HEX parameter 1 is error!");
            if (args1.IsError) { return args1; }
            if (Regex.IsMatch(args1.TextValue, "^[0-7]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function OCT2HEX parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 8), 16).ToUpper();
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function OCT2HEX parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function OCT2HEX parameter 2 is error!");
            }
            return Operand.Create(num);
        }
    }
    public class Function_HEX2BIN : Function_N
    {
        public Function_HEX2BIN(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }
            var args1 = args[0].ToText("Function HEX2BIN parameter 1 is error!");
            if (args1.IsError) { return args1; }
            if (Regex.IsMatch(args1.TextValue, "^[0-9A-Fa-f]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function HEX2BIN parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 16), 2);
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function HEX2BIN parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function HEX2BIN parameter 2 is error!");
            }
            return Operand.Create(num);
        }
    }
    public class Function_HEX2DEC : Function_1
    {
        public Function_HEX2DEC(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work).ToText("Function HEX2DEC parameter is error!");
            if (args1.IsError) { return args1; }
            if (Regex.IsMatch(args1.TextValue, "^[0-9A-Fa-f]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function HEX2DEC parameter is error!"); }
            var num = Convert.ToInt32(args1.TextValue, 16);
            return Operand.Create(num);
        }
    }
    public class Function_HEX2OCT : Function_N
    {
        public Function_HEX2OCT(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }
            var args1 = args[0].ToText("Function HEX2OCT parameter 1 is error!");
            if (args1.IsError) { return args1; }
            if (Regex.IsMatch(args1.TextValue, "^[0-9A-Fa-f]+$", RegexOptions.Compiled) == false) { return Operand.Error("Function HEX2OCT parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 16), 8);
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function HEX2OCT parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function HEX2OCT parameter 2 is error!");
            }
            return Operand.Create(num);
        }
    }
    public class Function_DEC2BIN : Function_N
    {
        public Function_DEC2BIN(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }
            var args1 = args[0].ToNumber("Function DEC2BIN parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var num = Convert.ToString(args1.IntValue, 2);
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function DEC2BIN parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function DEC2BIN parameter 2 is error!");
            }
            return Operand.Create(num);
        }
    }
    public class Function_DEC2OCT : Function_N
    {
        public Function_DEC2OCT(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }
            var args1 = args[0].ToNumber("Function DEC2OCT parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var num = Convert.ToString(args1.IntValue, 8);
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function DEC2OCT parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function DEC2OCT parameter 2 is error!");
            }
            return Operand.Create(num);
        }
    }
    public class Function_DEC2HEX : Function_N
    {
        public Function_DEC2HEX(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }
            var args1 = args[0].ToNumber("Function DEC2HEX parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var num = Convert.ToString(args1.IntValue, 16).ToUpper();
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function DEC2HEX parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function DEC2HEX parameter 2 is error!");
            }
            return Operand.Create(num);
        }
    }

    #endregion

    #region rounding
    public class Function_ROUNDUP : Function_2
    {
        public Function_ROUNDUP(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ROUNDUP parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ROUNDUP parameter 2 is error!"); if (args2.IsError) { return args2; } }
            if (args1.NumberValue == 0.0m) { return args1; }
            var a = Math.Pow(10, args2.IntValue);
            var b = args1.NumberValue;

            var t = (Math.Ceiling(Math.Abs((double)b) * a)) / a;
            if (b > 0) return Operand.Create(t);
            return Operand.Create(-t);
        }
    }
    public class Function_ROUNDDOWN : Function_2
    {
        public Function_ROUNDDOWN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ROUNDDOWN parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ROUNDDOWN parameter 2 is error!"); if (args2.IsError) { return args2; } }
            if (args1.NumberValue == 0.0m) {
                return args1;
            }
            var a = (decimal)Math.Pow(10, args2.IntValue);
            var b = args1.NumberValue;

            b = ((int)(b * a)) / a;
            return Operand.Create(b);
        }
    }
    public class Function_MROUND : Function_2
    {
        public Function_MROUND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function MROUND parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function MROUND parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var a = args2.NumberValue;
            if (a <= 0) { return Operand.Error("Function MROUND parameter 2 is error!"); }

            var b = args1.NumberValue;
            var r = Math.Round(b / a, 0, MidpointRounding.AwayFromZero) * a;
            return Operand.Create(r);
        }
    }
    public class Function_ROUND : Function_2
    {
        public Function_ROUND(FunctionBase func1) : base(func1, null)
        {
        }
        public Function_ROUND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ROUND parameter 1 is error!"); if (args1.IsError) { return args1; } }

            if (func2 == null) {
                return Operand.Create(Math.Round((decimal)args1.NumberValue, 0, MidpointRounding.AwayFromZero));
            }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ROUND parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(Math.Round((decimal)args1.NumberValue, args2.IntValue, MidpointRounding.AwayFromZero));
        }
    }

    public class Function_CEILING : Function_2
    {
        public Function_CEILING(FunctionBase func1) : base(func1, null)
        {
        }
        public Function_CEILING(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function CEILING parameter 1 is error!"); if (args1.IsError) { return args1; } }

            if (func2 == null)
                return Operand.Create(Math.Ceiling(args1.NumberValue));

            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function CEILING parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var b = args2.NumberValue;
            if (b == 0) { return Operand.Create(0); }
            if (b < 0) { return Operand.Error("Function CEILING parameter 2 is error!"); }

            var a = args1.NumberValue;
            var d = Math.Ceiling(a / b) * b;
            return Operand.Create(d);
        }
    }
    public class Function_FLOOR : Function_2
    {
        public Function_FLOOR(FunctionBase func1) : base(func1, null)
        {
        }
        public Function_FLOOR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FLOOR parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (func2 == null) return Operand.Create(Math.Floor(args1.NumberValue));

            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function FLOOR parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var b = args2.NumberValue;
            if (b >= 1) { return Operand.Create(args1.IntValue); }
            if (b <= 0) { return Operand.Error("Function FLOOR parameter 2 is error!"); }

            var a = args1.NumberValue;
            var d = Math.Floor(a / b) * b;
            return Operand.Create(d);
        }
    }
    public class Function_EVEN : Function_1
    {
        public Function_EVEN(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function EVEN parameter is error!"); if (args1.IsError) { return args1; } }
            var z = args1.NumberValue;
            if (z % 2 == 0) { return args1; }
            z = Math.Ceiling(z);
            if (z % 2 == 0) { return Operand.Create(z); }
            z++;
            return Operand.Create(z);
        }
    }
    public class Function_ODD : Function_1
    {
        public Function_ODD(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ODD parameter is error!"); if (args1.IsError) { return args1; } }
            var z = args1.NumberValue;
            if (z % 2 != 0) { return args1; }
            z = Math.Ceiling(z);
            if (z % 2 != 0) { return Operand.Create(z); }
            z++;
            return Operand.Create(z);
        }
    }

    #endregion

    #region RAND
    public class Function_RAND : FunctionBase
    {
        public Function_RAND()
        {
        }
        public override Operand Accept(Work work)
        {
#if NETSTANDARD2_1
            var tick = DateTime.Now.Ticks;
            Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
#else
            Random rand = Random.Shared;
#endif
            return Operand.Create(rand.NextDouble());
        }
    }
    public class Function_RANDBETWEEN : Function_2
    {
        public Function_RANDBETWEEN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function RANDBETWEEN parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function RANDBETWEEN parameter 2 is error!"); if (args2.IsError) { return args2; } }
#if NETSTANDARD2_1
            var tick = DateTime.Now.Ticks;
            Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
#else
            Random rand = Random.Shared;
#endif
            return Operand.Create((decimal)rand.NextDouble() * (args2.NumberValue - args1.NumberValue) + args1.NumberValue);
        }
    }
    #endregion

    #region power logarithm factorial
    public class Function_COVARIANCES : Function_2
    {
        public Function_COVARIANCES(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function COVARIANCES parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function COVARIANCES parameter 2 is error!"); if (args2.IsError) { return args2; } }

            List<decimal> list1 = new List<decimal>();
            List<decimal> list2 = new List<decimal>();

            var o1 = FunctionUtil.F_base_GetList(args1, list1);
            var o2 = FunctionUtil.F_base_GetList(args2, list2);
            if (o1 == false) { return Operand.Error("Function COVARIANCE.S parameter 1 error!"); }
            if (o2 == false) { return Operand.Error("Function COVARIANCE.S parameter 2 error!"); }
            if (list1.Count != list2.Count) { return Operand.Error("Function COVARIANCE.S parameter's count error!"); }

            var avg1 = list1.Average();
            var avg2 = list2.Average();
            decimal sum = 0;
            for (int i = 0; i < list1.Count; i++) {
                sum += (list1[i] - avg1) * (list2[i] - avg2);
            }
            var val = sum / (list1.Count - 1);
            return Operand.Create(val);
        }

    }
    public class Function_COVAR : Function_2
    {
        public Function_COVAR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function COVAR parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function COVAR parameter 2 is error!"); if (args2.IsError) { return args2; } }
            List<decimal> list1 = new List<decimal>();
            List<decimal> list2 = new List<decimal>();
            var o1 = FunctionUtil.F_base_GetList(args1, list1);
            var o2 = FunctionUtil.F_base_GetList(args2, list2);
            if (o1 == false) { return Operand.Error("Function COVAR parameter 1 error!"); }
            if (o2 == false) { return Operand.Error("Function COVAR parameter 2 error!"); }
            if (list1.Count != list2.Count) { return Operand.Error("Function COVAR parameter's count error!"); }

            var avg1 = list1.Average();
            var avg2 = list2.Average();
            decimal sum = 0;
            for (int i = 0; i < list1.Count; i++) {
                sum += (list1[i] - avg1) * (list2[i] - avg2);
            }
            var val = sum / list1.Count;
            return Operand.Create(val);
        }

    }
    public class Function_FACT : Function_1
    {
        public Function_FACT(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FACT parameter is error!"); if (args1.IsError) { return args1; } }
            if (args1.IsError) { return args1; }

            var z = args1.IntValue;
            if (z < 0) {
                return Operand.Error("Function FACT parameter is error!");
            }
            double d = 1;
            for (int i = 1; i <= z; i++) {
                d *= i;
            }
            return Operand.Create(d);
        }
    }
    public class Function_FACTDOUBLE : Function_1
    {
        public Function_FACTDOUBLE(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FACTDOUBLE parameter is error!"); if (args1.IsError) { return args1; } }
            var z = args1.IntValue;
            if (z < 0) { return Operand.Error("Function FACTDOUBLE parameter is error!"); }

            double d = 1;
            for (int i = z; i > 0; i -= 2) {
                d *= i;
            }
            return Operand.Create(d);
        }
    }
    public class Function_POWER : Function_2
    {
        public Function_POWER(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function POWER parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function POWER parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(Math.Pow((double)args1.NumberValue, (double)args2.NumberValue));
        }
    }
    public class Function_EXP : Function_1
    {
        public Function_EXP(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function EXP parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Exp((double)args1.NumberValue));
        }
    }
    public class Function_LN : Function_1
    {
        public Function_LN(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LN parameter is error!"); if (args1.IsError) { return args1; } }
            var z = args1.NumberValue;
            if (z <= 0) {
                return Operand.Error("Function LN parameter is error!");
            }
            return Operand.Create(Math.Log((double)z));
        }
    }
    public class Function_LOG : Function_2
    {
        public Function_LOG(FunctionBase func1) : base(func1, null)
        {
        }
        public Function_LOG(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LOG parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (func2 != null) {
                var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function POWER parameter 2 is error!"); if (args2.IsError) { return args2; } }
                return Operand.Create(Math.Log((double)args1.NumberValue, (double)args2.NumberValue));
            }
            return Operand.Create(Math.Log((double)args1.NumberValue, 10));
        }
    }
    public class Function_MULTINOMIAL : Function_N
    {
        public Function_MULTINOMIAL(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MULTINOMIAL parameter is error!"); }

            int sum = 0;
            int n = 1;
            for (int i = 0; i < list.Count; i++) {
                var a = (int)list[i];
                n *= FunctionUtil.F_base_Factorial(a);
                sum += a;
            }

            var r = FunctionUtil.F_base_Factorial(sum) / n;
            return Operand.Create(r);
        }
    }

    public class Function_PRODUCT : Function_N
    {
        public Function_PRODUCT(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { var aa = funcs[i].Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function PRODUCT parameter is error!"); }

            decimal d = 1;
            for (int i = 0; i < list.Count; i++) {
                var a = list[i];
                d *= a;
            }
            return Operand.Create(d);
        }
    }
    public class Function_SQRTPI : Function_1
    {
        public Function_SQRTPI(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SQRTPI parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Sqrt((double)args1.NumberValue * Math.PI));
        }
    }
    public class Function_SUMSQ : Function_N
    {
        public Function_SUMSQ(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { var aa = funcs[i].Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function SUMSQ parameter is error!"); }

            decimal d = 0;
            for (int i = 0; i < list.Count; i++) {
                var a = list[i];
                d += a * a;
            }
            return Operand.Create(d);
        }
    }

    #endregion
    #endregion

    #region string
    public class Function_ASC : Function_1
    {
        public Function_ASC(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ASC parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(F_base_ToDBC(args1.TextValue));
        }
        private static String F_base_ToDBC(String input)
        {
            StringBuilder sb = new StringBuilder(input);
            for (int i = 0; i < input.Length; i++) {
                var c = input[i];
                if (c == 12288) {
                    sb[i] = (char)32;
                    continue;
                } else if (c > 65280 && c < 65375) {
                    sb[i] = (char)(c - 65248);
                }
            }
            return sb.ToString();
        }
    }

    public class Function_JIS : Function_1
    {
        public Function_JIS(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function JIS parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(F_base_ToSBC(args1.TextValue));
        }
        private static String F_base_ToSBC(String input)
        {
            StringBuilder sb = new StringBuilder(input);
            for (int i = 0; i < input.Length; i++) {
                var c = input[i];
                if (c == ' ') {
                    sb[i] = (char)12288;
                } else if (c < 127) {
                    sb[i] = (char)(c + 65248);
                }
            }
            return sb.ToString();
        }
    }
    public class Function_CHAR : Function_1
    {
        public Function_CHAR(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function CHAR parameter is error!"); if (args1.IsError) { return args1; } }
            char c = (char)(int)args1.NumberValue;
            return Operand.Create(c.ToString());
        }
    }
    public class Function_CLEAN : Function_1
    {
        public Function_CLEAN(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function CLEAN parameter is error!"); if (args1.IsError) { return args1; } }
            var t = args1.TextValue;
            StringBuilder sb = new StringBuilder(t.Length);
            for (int i = 0; i < t.Length; i++) {
                var c = t[i];
                if (c != '\f' && c != '\n' && c != '\r' && c != '\t' && c != '\v') {
                    sb.Append(c);
                }
            }
            return Operand.Create(sb.ToString());
        }
    }
    public class Function_CODE : Function_1
    {
        public Function_CODE(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function CODE parameter is error!"); if (args1.IsError) { return args1; } }
            if (string.IsNullOrEmpty(args1.TextValue)) {
                return Operand.Error("Function CODE parameter is error!");
            }
            char c = args1.TextValue[0];
            return Operand.Create((decimal)(int)c);
        }
    }
    public class Function_CONCATENATE : Function_N
    {
        public Function_CONCATENATE(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < funcs.Length; i++) {
                var a = funcs[i].Accept(work); if (a.Type != OperandType.TEXT) { a = a.ToText($"Function CONCATENATE parameter {i + 1} is error!"); if (a.IsError) { return a; } }
                sb.Append(a.TextValue);
            }
            return Operand.Create(sb.ToString());
        }
    }
    public class Function_EXACT : Function_2
    {
        public Function_EXACT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function EXACT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function EXACT parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(args1.TextValue == args2.TextValue);
        }
    }
    public class Function_FIND : Function_3
    {
        public Function_FIND(FunctionBase func1, FunctionBase func2) : base(func1, func2, null)
        {
        }
        public Function_FIND(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function FIND parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function FIND parameter 2 is error!"); if (args2.IsError) { return args2; } }
            if (func3 == null) {
                var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue) + work.excelIndex;
                return Operand.Create(p);
            }
            var count = func3.Accept(work).ToNumber("Function FIND parameter 3 is error!"); if (count.IsError) { return count; }
            var p2 = args2.TextValue.AsSpan(count.IntValue).IndexOf(args1.TextValue) + count.IntValue + work.excelIndex;
            return Operand.Create(p2);
        }
    }
    public class Function_LEFT : Function_2
    {
        public Function_LEFT(FunctionBase func1) : base(func1, null)
        {
        }
        public Function_LEFT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LEFT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (func2 == null) {
                return Operand.Create(args1.TextValue[0].ToString());
            }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LEFT parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(args1.TextValue.AsSpan(0, args2.IntValue).ToString());
        }
    }
    public class Function_LEN : Function_1
    {
        public Function_LEN(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LEN parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create((decimal)args1.TextValue.Length);
        }
    }
    public class Function_LOWER : Function_1
    {
        public Function_LOWER(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LOWER parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.TextValue.ToLower());
        }
    }
    public class Function_MID : Function_3
    {
        public Function_MID(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function MID parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function MID parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = func3.Accept(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function MID parameter 3 is error!"); if (args3.IsError) { return args3; } }
            return Operand.Create(args1.TextValue.AsSpan(args2.IntValue - work.excelIndex, args3.IntValue).ToString());
        }
    }
    public class Function_PROPER : Function_1
    {
        public Function_PROPER(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function PROPER parameter is error!"); if (args1.IsError) { return args1; } }

            var text = args1.TextValue;
            StringBuilder sb = new StringBuilder(text);
            bool isFirst = true;
            for (int i = 0; i < text.Length; i++) {
                var t = text[i];
                if (t == ' ' || t == '\r' || t == '\n' || t == '\t' || t == '.') {
                    isFirst = true;
                } else if (isFirst) {
                    sb[i] = char.ToUpper(t);
                    isFirst = false;
                }
            }
            return Operand.Create(sb.ToString());
        }
    }
    public class Function_REPLACE : Function_4
    {
        public Function_REPLACE(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REPLACE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var oldtext = args1.TextValue;
            if (func4 == null) {
                var args22 = func2.Accept(work); if (args22.Type != OperandType.TEXT) { args22 = args22.ToText("Function REPLACE parameter 2 is error!"); if (args22.IsError) { return args22; } }
                var args32 = func3.Accept(work); if (args32.Type != OperandType.TEXT) { args32 = args32.ToText("Function REPLACE parameter 3 is error!"); if (args32.IsError) { return args32; } }

                var old = args22.TextValue;
                var newstr = args32.TextValue;
                return Operand.Create(oldtext.Replace(old, newstr));
            }

            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function REPLACE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = func3.Accept(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function REPLACE parameter 3 is error!"); if (args3.IsError) { return args3; } }
            var args4 = func4.Accept(work); if (args4.Type != OperandType.TEXT) { args4 = args4.ToText("Function REPLACE parameter 4 is error!"); if (args4.IsError) { return args4; } }

            var start = args2.IntValue - work.excelIndex;
            var length = args3.IntValue;
            var newtext = args4.TextValue;

            StringBuilder sb = new StringBuilder(oldtext.Length + newtext.Length);
            for (int i = 0; i < oldtext.Length; i++) {
                if (i < start) {
                    sb.Append(oldtext[i]);
                } else if (i == start) {
                    sb.Append(newtext);
                } else if (i >= start + length) {
                    sb.Append(oldtext[i]);
                }
            }
            return Operand.Create(sb.ToString());
        }
    }
    public class Function_REPT : Function_2
    {
        public Function_REPT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REPT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function REPT parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var newtext = args1.TextValue;
            var length = args2.IntValue;
            StringBuilder sb = new StringBuilder(newtext.Length * length);
            for (int i = 0; i < length; i++) {
                sb.Append(newtext);
            }
            return Operand.Create(sb.ToString());
        }
    }
    public class Function_RIGHT : Function_2
    {
        public Function_RIGHT(FunctionBase func1) : base(func1, null)
        {
        }
        public Function_RIGHT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function RIGHT parameter 1 is error!"); if (args1.IsError) { return args1; } }

            if (func2 == null) {
                return Operand.Create(args1.TextValue[args1.TextValue.Length - 1].ToString());
            }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function RIGHT parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(args1.TextValue.AsSpan(args1.TextValue.Length - args2.IntValue, args2.IntValue).ToString());
        }
    }
    public class Function_RMB : Function_1
    {
        public Function_RMB(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function RMB parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(F_base_ToChineseRMB(args1.NumberValue));
        }
        private static string F_base_ToChineseRMB(decimal x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A", CultureInfo.InvariantCulture);
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}", RegexOptions.Compiled);
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString(), RegexOptions.Compiled);
        }
    }
    public class Function_SEARCH : Function_3
    {
        public Function_SEARCH(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function SEARCH parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function SEARCH parameter 2 is error!"); if (args2.IsError) { return args2; } }

            if (func3 == null) {
                var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + work.excelIndex;
                return Operand.Create(p);
            }
            var args3 = func3.Accept(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function SEARCH parameter 3 is error!"); if (args3.IsError) { return args3; } }
            var p2 = args2.TextValue.AsSpan(args3.IntValue).IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + args3.IntValue + work.excelIndex;
            return Operand.Create(p2);
        }
    }
    public class Function_SUBSTITUTE : Function_4
    {
        public Function_SUBSTITUTE(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3, null)
        {
        }
        public Function_SUBSTITUTE(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function SUBSTITUTE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function SUBSTITUTE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = func3.Accept(work); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function SUBSTITUTE parameter 3 is error!"); if (args3.IsError) { return args3; } }
            if (func4 == null) {
                return Operand.Create(args1.TextValue.Replace(args2.TextValue, args3.TextValue));
            }
            var args4 = func4.Accept(work); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function SUBSTITUTE parameter 4 is error!"); if (args4.IsError) { return args4; } }
            string text = args1.TextValue;
            string oldtext = args2.TextValue;
            string newtext = args3.TextValue;
            int index = args4.IntValue;

            int index2 = 0;
            StringBuilder sb = new StringBuilder(text.Length + newtext.Length);
            for (int i = 0; i < text.Length; i++) {
                bool b = true;
                for (int j = 0; j < oldtext.Length; j++) {
                    var t = text[i + j];
                    var t2 = oldtext[j];
                    if (t != t2) {
                        b = false;
                        break;
                    }
                }
                if (b) {
                    index2++;
                }
                if (b && index2 == index) {
                    sb.Append(newtext);
                    i += oldtext.Length - 1;
                } else {
                    sb.Append(text[i]);
                }
            }
            return Operand.Create(sb.ToString());
        }
    }

    public class Function_T : Function_1
    {
        public Function_T(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type == OperandType.TEXT) {
                return args1;
            }
            return Operand.Create("");
        }
    }
    public class Function_TEXT : Function_2
    {
        public Function_TEXT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.IsError) { return args1; }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function TEXT parameter 2 is error!"); if (args2.IsError) { return args2; } }

            if (args1.Type == OperandType.TEXT) {
                return args1;
            } else if (args1.Type == OperandType.BOOLEAN) {
                return Operand.Create(args1.BooleanValue ? "TRUE" : "FALSE");
            } else if (args1.Type == OperandType.NUMBER) {
                return Operand.Create(args1.NumberValue.ToString(args2.TextValue, CultureInfo.InvariantCulture));
            } else if (args1.Type == OperandType.DATE) {
                return Operand.Create(args1.DateValue.ToString(args2.TextValue));
            }
            args1 = args1.ToText("Function TEXT parameter 1 is error!"); if (args1.IsError) { return args1; }
            return Operand.Create(args1.TextValue.ToString());
        }
    }
    public class Function_TRIM : Function_1
    {
        public Function_TRIM(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function TRIM parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.TextValue.AsSpan().Trim().ToString());
        }
    }
    public class Function_UPPER : Function_1
    {
        public Function_UPPER(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function UPPER parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.TextValue.ToUpper());
        }
    }
    public class Function_VALUE : Function_1
    {
        public Function_VALUE(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type == OperandType.NUMBER) { return args1; }
            if (args1.Type == OperandType.BOOLEAN) { return args1.BooleanValue ? Operand.One : Operand.Zero; }
            if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function VALUE parameter is error!"); if (args1.IsError) { return args1; } }

            if (decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                return Operand.Create(d);
            }
            return Operand.Error("Function VALUE parameter is error!");
        }
    }
    #endregion

    #region MyDate time
    public class Function_DATEVALUE : Function_N
    {
        public Function_DATEVALUE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }
            if (args[0].Type == OperandType.DATE) { return args[0]; }
            int type = 0;
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function DATEVALUE parameter 2 is error!");
                if (args2.IsError) { return args2; }
                type = args2.IntValue;
            }
            if (type == 0) {
                if (args[0].Type == OperandType.TEXT) {
                    if (DateTime.TryParse(args[0].TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time)) {
                        return Operand.Create(time);
                    }
                }
                var args1 = args[0].ToNumber("Function DATEVALUE parameter 1 is error!");
                if (args1.LongValue <= 2958465L) { // 9999-12-31 日时间在excel的数字为 2958465
                    return args1.ToMyDate();
                }
                if (args1.LongValue <= 253402232399L) { // 9999-12-31 12:59:59 日时间 转 时间截 为 253402232399L，
                    var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(args1.LongValue);
                    if (work.useLocalTime) { return Operand.Create(time.ToLocalTime()); }
                    return Operand.Create(time);
                }
                // 注：时间截 253402232399 ms 转时间 为 1978-01-12 05:30:32
                var time2 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(args1.LongValue);
                if (work.useLocalTime) { return Operand.Create(time2.ToLocalTime()); }
                return Operand.Create(time2);
            } else if (type == 1) {
                var args1 = args[0].ToText("Function DATEVALUE parameter 1 is error!");
                if (args1.IsError) { return args1; }
                if (DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    return Operand.Create(dt);
                }
            } else if (type == 2) {
                return args[0].ToNumber("Function DATEVALUE parameter is error!").ToMyDate();
            } else if (type == 3) {
                var args1 = args[0].ToNumber("Function DATEVALUE parameter 1 is error!");
                var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(args1.LongValue);
                if (work.useLocalTime) { return Operand.Create(time.ToLocalTime()); }
                return Operand.Create(time);
            } else if (type == 4) {
                var args1 = args[0].ToNumber("Function DATEVALUE parameter 1 is error!");
                var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(args1.LongValue);
                if (work.useLocalTime) { return Operand.Create(time.ToLocalTime()); }
                return Operand.Create(time);
            }
            return Operand.Error("Function DATEVALUE parameter is error!");
        }
    }
    public class Function_TIMESTAMP : Function_N
    {
        public Function_TIMESTAMP(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }
            int type = 0; // 毫秒
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function TIMESTAMP parameter 2 is error!");
                if (args2.IsError) { return args2; }
                type = args2.IntValue;
            }
            DateTime args1;
            if (work.useLocalTime) {
                args1 = args[0].ToMyDate("Function TIMESTAMP parameter 1 is error!").DateValue.ToDateTime(DateTimeKind.Local).ToUniversalTime();
            } else {
                args1 = args[0].ToMyDate("Function TIMESTAMP parameter 1 is error!").DateValue.ToDateTime(DateTimeKind.Utc);
            }
            if (type == 0) {
                var ms = (args1 - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                return Operand.Create(ms);
            } else if (type == 1) {
                var s = (args1 - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                return Operand.Create(s);
            }
            return Operand.Error("Function TIMESTAMP parameter is error!");
        }
    }

    public class Function_TIMEVALUE : Function_1
    {
        public Function_TIMEVALUE(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function TIMEVALUE parameter is error!"); if (args1.IsError) { return args1; } }

            if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan dt)) {
                return Operand.Create(dt);
            }
            return Operand.Error("Function TIMEVALUE parameter is error!");
        }
    }
    public class Function_DATE : Function_N
    {
        public Function_DATE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = funcs[0].Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function DATE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function DATE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = funcs[2].Accept(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function DATE parameter 3 is error!"); if (args3.IsError) { return args3; } }

            MyDate d;
            if (funcs.Length == 3) {
                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, 0, 0, 0);
            } else if (funcs.Length == 4) {
                var args4 = funcs[3].Accept(work); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function DATE parameter 4 is error!"); if (args4.IsError) { return args4; } }

                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, 0, 0);
            } else if (funcs.Length == 5) {
                var args4 = funcs[3].Accept(work); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function DATE parameter 4 is error!"); if (args4.IsError) { return args4; } }
                var args5 = funcs[4].Accept(work); if (args5.Type != OperandType.NUMBER) { args5 = args5.ToNumber("Function DATE parameter 5 is error!"); if (args5.IsError) { return args5; } }
                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, 0);
            } else {
                var args4 = funcs[3].Accept(work); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function DATE parameter 4 is error!"); if (args4.IsError) { return args4; } }
                var args5 = funcs[4].Accept(work); if (args5.Type != OperandType.NUMBER) { args5 = args5.ToNumber("Function DATE parameter 5 is error!"); if (args5.IsError) { return args5; } }
                var args6 = funcs[5].Accept(work); if (args6.Type != OperandType.NUMBER) { args6 = args6.ToNumber("Function DATE parameter 6 is error!"); if (args6.IsError) { return args6; } }
                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, args6.IntValue);
            }
            return Operand.Create(d);
        }
    }
    public class Function_DATEDIF : Function_3
    {
        public Function_DATEDIF(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function DATEDIF parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.DATE) { args2 = args2.ToMyDate("Function DATEDIF parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = func3.Accept(work); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function DATEDIF parameter 3 is error!"); if (args3.IsError) { return args3; } }
            var startMyDate = (DateTime)args1.DateValue;
            var endMyDate = (DateTime)args2.DateValue;
            var t = args3.TextValue.ToLower();

            if (CharUtil.Equals(t, 'Y')) {

                #region y

                bool b = false;
                if (startMyDate.Month < endMyDate.Month) {
                    b = true;
                } else if (startMyDate.Month == endMyDate.Month) {
                    if (startMyDate.Day <= endMyDate.Day) b = true;
                }
                if (b) {
                    return Operand.Create((endMyDate.Year - startMyDate.Year));
                } else {
                    return Operand.Create((endMyDate.Year - startMyDate.Year - 1));
                }

                #endregion y
            } else if (CharUtil.Equals(t, 'M')) {

                #region m

                bool b = false;
                if (startMyDate.Day <= endMyDate.Day) b = true;
                if (b) {
                    return Operand.Create((endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month));
                } else {
                    return Operand.Create((endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month - 1));
                }

                #endregion m
            } else if (CharUtil.Equals(t, 'D')) {
                return Operand.Create((endMyDate - startMyDate).Days);
            } else if (CharUtil.Equals(t, "YD")) {

                #region yd

                var day = endMyDate.DayOfYear - startMyDate.DayOfYear;
                if (endMyDate.Year > startMyDate.Year && day < 0) {
                    var days = new DateTime(startMyDate.Year, 12, 31).DayOfYear;
                    day = days + day;
                }
                return Operand.Create((day));

                #endregion yd
            } else if (CharUtil.Equals(t, "MD")) {

                #region md

                var mo = endMyDate.Day - startMyDate.Day;
                if (mo < 0) {
                    int days;
                    if (startMyDate.Month == 12) {
                        days = new DateTime(startMyDate.Year + 1, 1, 1).AddDays(-1).Day;
                    } else {
                        days = new DateTime(startMyDate.Year, startMyDate.Month + 1, 1).AddDays(-1).Day;
                    }
                    mo += days;
                }
                return Operand.Create((mo));

                #endregion md
            } else if (CharUtil.Equals(t, "YM")) {

                #region ym

                var mo = endMyDate.Month - startMyDate.Month;
                if (endMyDate.Day < startMyDate.Day) mo--;
                if (mo < 0) mo += 12;
                return Operand.Create((mo));

                #endregion ym
            }
            return Operand.Error("Function DATEDIF parameter 3 is error!");
        }
    }

    public class Function_TIME : Function_N
    {
        public Function_TIME(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = funcs[0].Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TIME parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function TIME parameter 2 is error!"); if (args2.IsError) { return args2; } }

            MyDate d;
            if (funcs.Length == 3) {
                var args3 = funcs[2].Accept(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function TIME parameter 3 is error!"); if (args3.IsError) { return args3; } }
                d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, args3.IntValue);
            } else {
                d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, 0);
            }
            return Operand.Create(d);
        }
    }

    public class Function_NOW : FunctionBase
    {
        public override Operand Accept(Work work)
        {
            if (work.useLocalTime) {
                return Operand.Create(DateTime.Now);
            } else {
                return Operand.Create(DateTime.UtcNow);
            }
        }
    }
    public class Function_TODAY : FunctionBase
    {
        public override Operand Accept(Work work)
        {
            DateTime now;
            if (work.useLocalTime) {
                now = DateTime.Now;
            } else {
                now = DateTime.UtcNow;
            }
            return Operand.Create(new MyDate(now.Year, now.Month, now.Day, 0, 0, 0));
        }
    }

    public class Function_SECOND : Function_1
    {
        public Function_SECOND(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function SECOND parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.DateValue.Second);
        }
    }

    public class Function_MINUTE : Function_1
    {
        public Function_MINUTE(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function MINUTE parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.DateValue.Minute);
        }
    }
    public class Function_HOUR : Function_1
    {
        public Function_HOUR(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function HOUR parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.DateValue.Hour);
        }
    }

    public class Function_MONTH : Function_1
    {
        public Function_MONTH(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function MONTH parameter is error!"); if (args1.IsError) { return args1; } }
            if (args1.DateValue.Month == null) {
                return Operand.Error("Function MONTH is error!");
            }
            return Operand.Create((int)args1.DateValue.Month.Value);
        }
    }
    public class Function_YEAR : Function_1
    {
        public Function_YEAR(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function YEAR parameter is error!"); if (args1.IsError) { return args1; } }
            if (args1.DateValue.Year == null) {
                return Operand.Error("Function YEAR is error!");
            }
            return Operand.Create(args1.DateValue.Year.Value);
        }
    }
    public class Function_DAY : Function_1
    {
        public Function_DAY(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work);
            if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function DAY parameter is error!"); if (args1.IsError) { return args1; } }
            if (args1.DateValue.Day == null) {
                return Operand.Error("Function DAY is error!");
            }
            return Operand.Create(args1.DateValue.Day.Value);
        }
    }
    public class Function_WEEKDAY : Function_N
    {
        public Function_WEEKDAY(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var exprs = funcs;
            var args1 = exprs[0].Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function WEEKDAY parameter 1 is error!"); if (args1.IsError) { return args1; } }

            var type = 1;
            if (exprs.Length == 2) {
                var args2 = exprs[1].Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function WEEKDAY parameter 2 is error!"); if (args2.IsError) { return args2; } }
                type = args2.IntValue;
            }

            var t = ((DateTime)args1.DateValue).DayOfWeek;
            if (type == 1) {
                return Operand.Create((double)(t + 1));
            } else if (type == 2) {
                if (t == 0) return Operand.Create(7d);
                return Operand.Create((double)t);
            }
            if (t == 0) {
                return Operand.Create(6d);
            }
            return Operand.Create((double)(t - 1));
        }
    }

    public class Function_DAYSINMONTH : Function_2
    {
        public Function_DAYSINMONTH(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function DAYSINMONTH parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function DAYSINMONTH parameter 2 is error!"); if (args2.IsError) { return args2; } }
            int year = args1.IntValue;
            int month = args2.IntValue;
            if (month < 1 || month > 12) { return Operand.Error("Function DAYSINMONTH parameter 2 is error!"); }
            int days = DateTime.DaysInMonth(year, month);
            return Operand.Create((decimal)days);
        }
    }

    public class Function_DAYS360 : Function_3
    {
        public Function_DAYS360(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function DAYS360 parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.DATE) { args2 = args2.ToMyDate("Function DAYS360 parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var startMyDate = (DateTime)args1.DateValue;
            var endMyDate = (DateTime)args2.DateValue;

            var method = false;
            if (func3 != null) {
                var args3 = func3.Accept(work); if (args3.Type != OperandType.DATE) { args3 = args3.ToMyDate("Function DAYS360 parameter 3 is error!"); if (args3.IsError) { return args3; } }
                if (args3.IsError) { return args3; }
                method = args3.BooleanValue;
            }
            var days = endMyDate.Year * 360 + (endMyDate.Month - 1) * 30
                        - startMyDate.Year * 360 - (startMyDate.Month - 1) * 30;
            if (method) {
                if (endMyDate.Day == 31) days += 30;
                if (startMyDate.Day == 31) days -= 30;
            } else {
                if (startMyDate.Month == 12) {
                    if (startMyDate.Day == new DateTime(startMyDate.Year + 1, 1, 1).AddDays(-1).Day) {
                        days -= 30;
                    } else {
                        days -= startMyDate.Day;
                    }
                } else {
                    if (startMyDate.Day == new DateTime(startMyDate.Year, startMyDate.Month + 1, 1).AddDays(-1).Day) {
                        days -= 30;
                    } else {
                        days -= startMyDate.Day;
                    }
                }
                if (endMyDate.Month == 12) {
                    if (endMyDate.Day == new DateTime(endMyDate.Year + 1, 1, 1).AddDays(-1).Day) {
                        if (startMyDate.Day < 30) {
                            days += 31;
                        } else {
                            days += 30;
                        }
                    } else {
                        days += endMyDate.Day;
                    }
                } else {
                    if (endMyDate.Day == new DateTime(endMyDate.Year, endMyDate.Month + 1, 1).AddDays(-1).Day) {
                        if (startMyDate.Day < 30) {
                            days += 31;
                        } else {
                            days += 30;
                        }
                    } else {
                        days += endMyDate.Day;
                    }
                }
            }
            return Operand.Create(days);
        }
    }

    public class Function_EDATE : Function_2
    {
        public Function_EDATE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function EDATE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function EDATE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddMonths(args2.IntValue)));
        }
    }

    public class Function_EOMONTH : Function_2
    {
        public Function_EOMONTH(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function EOMONTH parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function EOMONTH parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var dt = ((DateTime)args1.DateValue).AddMonths(args2.IntValue + 1);
            dt = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
            return Operand.Create(dt);
        }
    }
    public class Function_NETWORKDAYS : Function_N
    {
        public Function_NETWORKDAYS(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = funcs[0].Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function NETWORKDAYS parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Accept(work); if (args2.Type != OperandType.DATE) { args2 = args2.ToMyDate("Function NETWORKDAYS parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var startMyDate = (DateTime)args1.DateValue;
            var endMyDate = (DateTime)args2.DateValue;

            List<DateTime> list = new List<DateTime>();
            for (int i = 2; i < funcs.Length; i++) {
                var ar = funcs[i].Accept(work).ToMyDate($"Function NETWORKDAYS parameter {i + 1} is error!");
                if (ar.IsError) { return ar; }
                list.Add(ar.DateValue);
            }
            var days = 0;
            while (startMyDate <= endMyDate) {
                if (startMyDate.DayOfWeek != DayOfWeek.Sunday && startMyDate.DayOfWeek != DayOfWeek.Saturday) {
                    if (list.Contains(startMyDate) == false) {
                        days++;
                    }
                }
                startMyDate = startMyDate.AddDays(1);
            }
            return Operand.Create(days);
        }
    }

    public class Function_WORKDAY : Function_N
    {
        public Function_WORKDAY(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = funcs[0].Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function WORKDAY parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function WORKDAY parameter 2 is error!"); if (args2.IsError) { return args2; } }


            var startMyDate = (DateTime)args1.DateValue;
            var days = args2.IntValue;
            List<DateTime> list = new List<DateTime>();
            for (int i = 2; i < funcs.Length; i++) {
                var ar = funcs[i].Accept(work).ToMyDate($"Function WORKDAY parameter {i + 1} is error!");
                if (ar.IsError) { return ar; }
                list.Add(ar.DateValue);
            }
            while (days > 0) {
                startMyDate = startMyDate.AddDays(1);
                if (startMyDate.DayOfWeek == DayOfWeek.Saturday) continue;
                if (startMyDate.DayOfWeek == DayOfWeek.Sunday) continue;
                if (list.Contains(startMyDate)) continue;
                days--;
            }
            return Operand.Create(startMyDate);
        }
    }
    public class Function_WEEKNUM : Function_N
    {
        public Function_WEEKNUM(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = funcs[0].Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function WEEKNUM parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var startMyDate = (DateTime)args1.DateValue;

            var days = startMyDate.DayOfYear + (int)(new DateTime(startMyDate.Year, 1, 1).DayOfWeek);
            if (funcs.Length == 2) {
                var args2 = funcs[1].Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function WEEKNUM parameter 2 is error!"); if (args2.IsError) { return args2; } }
                if (args2.IntValue == 2) {
                    days--;
                }
            }

            var week = Math.Ceiling(days / 7.0);
            return Operand.Create(week);
        }
    }
    public class Function_ADDMONTHS : Function_2
    {
        public Function_ADDMONTHS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function ADDMONTHS parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ADDMONTHS parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddMonths(args2.IntValue)));
        }
    }
    public class Function_ADDYEARS : Function_2
    {
        public Function_ADDYEARS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function ADDYEARS parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ADDYEARS parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddYears(args2.IntValue)));
        }
    }
    public class Function_ADDSECONDS : Function_2
    {
        public Function_ADDSECONDS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function ADDSECONDS parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ADDSECONDS parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddSeconds((double)args2.NumberValue)));
        }
    }
    public class Function_ADDMINUTES : Function_2
    {
        public Function_ADDMINUTES(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function ADDMINUTES parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ADDMINUTES parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddMinutes((double)args2.NumberValue)));
        }
    }
    public class Function_ADDHOURS : Function_2
    {
        public Function_ADDHOURS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function ADDHOURS parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ADDHOURS parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddHours((double)args2.NumberValue)));
        }
    }
    public class Function_ADDDAYS : Function_2
    {
        public Function_ADDDAYS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function ADDDAYS parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ADDDAYS parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddDays((double)args2.NumberValue)));
        }
    }

    #endregion
    #region sum
    public class Function_MAX : Function_N
    {
        public Function_MAX(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function MAX parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MAX parameter error!"); }

            return Operand.Create(list.Max());
        }
    }
    public class Function_MIN : Function_N
    {
        public Function_MIN(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function MIN parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MIN parameter error!"); }
            return Operand.Create(list.Min());
        }
    }
    public class Function_SUM : Function_N
    {
        public Function_SUM(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function SUM parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function SUM parameter error!"); }
            return Operand.Create(list.Sum());
        }
    }
    public class Function_SUMIF : Function_N
    {
        public Function_SUMIF(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function SUMIF parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args[0], list);
            if (o == false) { return Operand.Error("Function SUMIF parameter 1 error!"); }

            List<decimal> sumdbs;
            if (args.Count == 3) {
                sumdbs = new List<decimal>();
                var o2 = FunctionUtil.F_base_GetList(args[2], sumdbs);
                if (o2 == false) { return Operand.Error("Function SUMIF parameter 3 error!"); }
            } else {
                sumdbs = list;
            }

            decimal sum;
            if (args[1].Type == OperandType.NUMBER) {
                sum = FunctionUtil.F_base_countif(list, args[1].NumberValue) * args[1].NumberValue;
            } else {
                if (decimal.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    sum = FunctionUtil.F_base_sumif(list, d, sumdbs);
                } else {
                    var sunif = args[1].TextValue.Trim();
                    var m2 = FunctionUtil.sumifMatch(sunif);
                    if (m2 != null) {
                        sum = FunctionUtil.F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return Operand.Error("Function SUMIF parameter 2 error!");
                    }
                }
            }
            return Operand.Create(sum);
        }
    }
    public class Function_AVEDEV : Function_N
    {
        public Function_AVEDEV(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function AVEDEV parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function AVEDEV parameter error!"); }
            if (list.Count == 0) { return Operand.Zero; }
            var avg = list.Average();
            decimal sum = 0;
            foreach (var item in list) {
                sum += Math.Abs(item - avg);
            }
            return Operand.Create(sum / list.Count);
        }
    }
    public class Function_AVERAGE : Function_N
    {
        public Function_AVERAGE(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function AVERAGE parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function AVERAGE parameter error!"); }
            if (list.Count == 0) { return Operand.Zero; }
            return Operand.Create(list.Average());
        }
    }
    public class Function_AVERAGEIF : Function_N
    {
        public Function_AVERAGEIF(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { var aa = funcs[i].Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args[0], list);
            if (o == false) { return Operand.Error("Function AVERAGE parameter 1 error!"); }

            List<decimal> sumdbs;
            if (args.Count == 3) {
                sumdbs = new List<decimal>();
                var o2 = FunctionUtil.F_base_GetList(args[2], sumdbs);
                if (o2 == false) { return Operand.Error("Function AVERAGE parameter 3 error!"); }
            } else {
                sumdbs = list;
            }

            decimal sum;
            int count;
            if (args[1].Type == OperandType.NUMBER) {
                count = FunctionUtil.F_base_countif(list, args[1].NumberValue);
                sum = count * args[1].NumberValue;
            } else {
                if (decimal.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = FunctionUtil.F_base_countif(list, d);
                    sum = FunctionUtil.F_base_sumif(list, d, sumdbs);
                } else {
                    var sunif = args[1].TextValue.Trim();
                    var m2 = FunctionUtil.sumifMatch(sunif);
                    if (m2 != null) {
                        count = FunctionUtil.F_base_countif(list, m2.Item1, m2.Item2);
                        sum = FunctionUtil.F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return Operand.Error("Function AVERAGE parameter 2 error!");
                    }
                }
            }
            return Operand.Create(sum / count);
        }
    }
    public class Function_COUNT : Function_N
    {
        public Function_COUNT(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function COUNT parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function COUNT parameter error!"); }
            return Operand.Create(list.Count);
        }
    }
    public class Function_COUNTIF : Function_2
    {
        public Function_COUNTIF(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToNumber("Function COUNTIF parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.IsError) { return args2; }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function COUNTIF parameter 1 error!"); }
            int count;
            if (args2.Type == OperandType.NUMBER) {
                count = FunctionUtil.F_base_countif(list, args2.NumberValue);
            } else {
                if (decimal.TryParse(args2.TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = FunctionUtil.F_base_countif(list, d);
                } else {
                    var sunif = args2.TextValue.Trim();
                    var m2 = FunctionUtil.sumifMatch(sunif);
                    if (m2 != null) {
                        count = FunctionUtil.F_base_countif(list, m2.Item1, m2.Item2);
                    } else {
                        return Operand.Error("Function COUNTIF parameter 2 error!");
                    }
                }
            }
            return Operand.Create((decimal)count);
        }
    }


    public class Function_COUNTA : Function_N
    {
        public Function_COUNTA(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }
            List<string> list = new List<string>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function COUNTA parameter error!"); }
            return Operand.Create((decimal)list.Count);
        }
    }
    public class Function_MEDIAN : Function_N
    {
        public Function_MEDIAN(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function MEDIAN parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);

            if (o == false) { return Operand.Error("Function MEDIAN parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function MEDIAN parameter error!"); }

            list = list.OrderBy(q => q).ToList();
            return Operand.Create(list[list.Count / 2]);
        }
    }
    public class Function_MODE : Function_N
    {
        public Function_MODE(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function MODE parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MODE parameter error!"); }

            Dictionary<decimal, int> dict = new Dictionary<decimal, int>();
            foreach (var item in list) {
                if (dict.ContainsKey(item)) {
                    dict[item] += 1;
                } else {
                    dict[item] = 1;
                }
            }
            return Operand.Create(dict.OrderByDescending(q => q.Value).First().Key);
        }
    }
    public class Function_LARGE : Function_2
    {
        public Function_LARGE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToNumber("Function LARGE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LARGE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function LARGE parameter 1 error!"); }

            list = list.OrderByDescending(q => q).ToList();
            int k = args2.IntValue;
            if (k < 1 - work.excelIndex || k > list.Count - work.excelIndex) {
                return Operand.Error("Function LARGE parameter 2 is error!");
            }
            return Operand.Create(list[k - work.excelIndex]);
        }
    }
    public class Function_SMALL : Function_2
    {
        public Function_SMALL(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToNumber("Function SMALL parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function SMALL parameter 2 is error!"); if (args2.IsError) { return args2; } }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function SMALL parameter 1 error!"); }
            list = list.OrderBy(q => q).ToList();
            int k = args2.IntValue;
            if (k < 1 - work.excelIndex || k > list.Count - work.excelIndex) {
                return Operand.Error("Function SMALL parameter 2 is error!");
            }
            return Operand.Create(list[k - work.excelIndex]);
        }
    }
    public class Function_PERCENTILE : Function_2
    {
        public Function_PERCENTILE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToNumber("Function PERCENTILE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function PERCENTILE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function PERCENTILE parameter 1 error!"); }
            var k = args2.NumberValue;
            return Operand.Create(ExcelFunctions.Percentile(list.Select(q => (double)q).ToArray(), (double)k));
        }
    }
    public class Function_GEOMEAN : Function_N
    {
        public Function_GEOMEAN(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function GEOMEAN parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function GEOMEAN parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function GEOMEAN parameter error!"); }
            double product = 1.0;
            foreach (var num in list) {
                if (num <= 0) {
                    return Operand.Error("Function GEOMEAN parameter error!");
                }
                product *= (double)num;
            }
            double geoMean = Math.Pow(product, 1.0 / list.Count);
            return Operand.Create((decimal)geoMean);
        }
    }
    public class Function_HARMEAN : Function_N
    {
        public Function_HARMEAN(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function HARMEAN parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            if (args.Count == 1) return args[0];

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function HARMEAN parameter error!"); }

            decimal sum = 0;
            foreach (var db in list) {
                if (db == 0) {
                    return Operand.Error("Function HARMEAN parameter error!");
                }
                sum += 1 / db;
            }
            return Operand.Create(list.Count / sum);
        }
    }


    public class Function_PERCENTRANK : Function_N
    {
        public Function_PERCENTRANK(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Accept(Work work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Accept(work); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToArray("Function PERCENTRANK parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var args2 = args[1].ToNumber("Function PERCENTRANK parameter 2 is error!");
            if (args2.IsError) { return args2; }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function PERCENTRANK parameter error!"); }

            var k = args2.NumberValue;
            var v = ExcelFunctions.PercentRank(list.Select(q => (double)q).ToArray(), (double)k);
            var d = 3;
            if (args.Count == 3) {
                var args3 = args[2].ToNumber("Function PERCENTRANK parameter 3 is error!");
                if (args3.IsError) { return args3; }
                d = args3.IntValue;
            }
            return Operand.Create(Math.Round(v, d, MidpointRounding.AwayFromZero));
        }
    }
    public class Function_QUANTILE : Function_2
    {
        public Function_QUANTILE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToNumber("Function QUANTILE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function QUANTILE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function QUANTILE parameter 1 error!"); }
            var k = args2.NumberValue;
            return Operand.Create(ExcelFunctions.Quantile(list.Select(q => (double)q).ToArray(), (double)k));
        }
    }


    public class Function_RANGE : Function_2
    {
        public Function_RANGE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function RANGE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function RANGE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(args2.NumberValue - args1.NumberValue);
        }
    }
    public class Function_VARIANCE : Function_N
    {
        public Function_VARIANCE(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function VARIANCE parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function VARIANCE parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function VARIANCE parameter error!"); }
            var avg = list.Average();
            var variance = list.Sum(d => (d - avg) * (d - avg)) / list.Count;
            return Operand.Create(variance);
        }
    }
    public class Function_STDEV : Function_N
    {
        public Function_STDEV(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function STDEV parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function STDEV parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function STDEV parameter error!"); }

            var avg = list.Average();
            decimal sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(Math.Sqrt((double)sum / (list.Count - 1)));
        }
    }
    public class Function_STDEVP : Function_N
    {
        public Function_STDEVP(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function STDEVP parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function STDEVP parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function STDEVP parameter error!"); }
            var avg = list.Average();
            decimal sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(Math.Sqrt((double)sum / list.Count));
        }
    }
    public class Function_DEVSQ : Function_N
    {
        public Function_DEVSQ(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function DEVSQ parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function DEVSQ parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function DEVSQ parameter error!"); }
            var avg = list.Average();
            decimal sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(sum);
        }
    }
    public class Function_VAR : Function_N
    {
        public Function_VAR(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Accept(work).ToNumber($"Function VARVARP parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }
            if (args.Count == 1) {
                return Operand.Error("Function VAR parameter only one error!");
            }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function VARVARP parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function VARVARP parameter error!"); }
            decimal sum = 0;
            decimal sum2 = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += list[i] * list[i];
                sum2 += list[i];
            }
            return Operand.Create((list.Count * sum - sum2 * sum2) / list.Count / (list.Count - 1));
        }
    }




    public class Function_QUARTILE : Function_2
    {
        public Function_QUARTILE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToNumber("Function QUARTILE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Accept(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function QUARTILE parameter 2 is error!"); if (args2.IsError) { return args2; } }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function QUARTILE parameter 1 error!"); }

            var quant = args2.IntValue;
            if (quant < 0 || quant > 4) {
                return Operand.Error("Function QUARTILE parameter 2 is error!");
            }
            return Operand.Create(ExcelFunctions.Quartile(list.Select(q => (double)q).ToArray(), quant));
        }
    }


    public class Function_FACTORIAL : Function_1
    {
        public Function_FACTORIAL(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FACTORIAL parameter is error!"); if (args1.IsError) { return args1; } }
            int n = args1.IntValue;
            if (n < 0) { return Operand.Error("Function FACTORIAL parameter is error!"); }
            decimal result = 1;
            for (int i = 1; i <= n; i++) {
                result *= i;
            }
            return Operand.Create(result);
        }
    }

    #endregion



    public class FunctionUtil
    {
        public static bool F_base_GetList(List<Operand> args, List<decimal> list)
        {
            foreach (var item in args) {
                if (item.Type == OperandType.NUMBER) {
                    list.Add(item.NumberValue);
                } else if (item.Type == OperandType.ARRARY) {
                    var o = F_base_GetList(item.ArrayValue, list);
                    if (o == false) { return false; }
                } else if (item.Type == OperandType.JSON) {
                    var i = item.ToArray(null);
                    if (i.IsError) { return false; }
                    var o = F_base_GetList(i.ArrayValue, list);
                    if (o == false) { return false; }
                } else {
                    var o = item.ToNumber(null);
                    if (o.IsError) { return false; }
                    list.Add(o.NumberValue);
                }
            }
            return true;
        }
        public static bool F_base_GetList(Operand args, List<decimal> list)
        {
            if (args.IsError) { return false; }
            if (args.Type == OperandType.NUMBER) {
                list.Add(args.NumberValue);
            } else if (args.Type == OperandType.ARRARY) {
                var o = F_base_GetList(args.ArrayValue, list);
                if (o == false) { return false; }
            } else if (args.Type == OperandType.JSON) {
                var i = args.ToArray(null);
                if (i.IsError) { return false; }
                var o = F_base_GetList(i.ArrayValue, list);
                if (o == false) { return false; }
            } else {
                var o = args.ToNumber(null);
                if (o.IsError) { return false; }
                list.Add(o.NumberValue);
            }
            return true;
        }
        public static bool F_base_GetList(Operand args, List<string> list)
        {
            if (args.IsError) { return false; }
            if (args.Type == OperandType.ARRARY) {
                var o = F_base_GetList(args.ArrayValue, list);
                if (o == false) { return false; }
            } else if (args.Type == OperandType.JSON) {
                var i = args.ToArray(null);
                if (i.IsError) { return false; }
                var o = F_base_GetList(i.ArrayValue, list);
                if (o == false) { return false; }
            } else {
                var o = args.ToText(null);
                if (o.IsError) { return false; }
                list.Add(o.TextValue);
            }
            return true;
        }
        public static bool F_base_GetList(List<Operand> args, List<string> list)
        {
            foreach (var item in args) {
                if (item.Type == OperandType.ARRARY) {
                    var o = F_base_GetList(item.ArrayValue, list);
                    if (o == false) { return false; }
                } else if (item.Type == OperandType.JSON) {
                    var i = item.ToArray(null);
                    if (i.IsError) { return false; }
                    var o = F_base_GetList(i.ArrayValue, list);
                    if (o == false) { return false; }
                } else {
                    var o = item.ToText(null);
                    if (o.IsError) { return false; }
                    list.Add(o.TextValue);
                }
            }
            return true;
        }


        public static int F_base_countif(List<decimal> dbs, decimal d)
        {
            int count = 0;
            for (int i = 0; i < dbs.Count; i++) {
                var item = dbs[i];
                if (item == d) {
                    count++;
                }
            }
            return count;
        }

        public static int F_base_countif(List<decimal> dbs, string s, decimal d)
        {
            int count = 0;
            for (int i = 0; i < dbs.Count; i++) {
                var item = dbs[i];
                if (F_base_compare(item, d, s)) {
                    count++;
                }
            }
            return count;
        }

        public static decimal F_base_sumif(List<decimal> dbs, decimal d, List<decimal> sumdbs)
        {
            decimal sum = 0;
            for (int i = 0; i < dbs.Count; i++) {
                var item = dbs[i];
                if (item == d) {
                    sum += sumdbs[i];
                }
                //if (Math.Round(item, 10, MidpointRounding.AwayFromZero) == d) {
                //	sum += item;
                //}
            }
            return sum;
        }

        public static decimal F_base_sumif(List<decimal> dbs, string s, decimal d, List<decimal> sumdbs)
        {
            decimal sum = 0;
            for (int i = 0; i < dbs.Count; i++) {
                if (F_base_compare(dbs[i], d, s)) {
                    sum += sumdbs[i];
                }
            }
            return sum;
        }
        public static bool F_base_compare(decimal a, decimal b, string ss)
        {
            if (CharUtil.Equals(ss, '<')) {
                return a < b;
                //return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) < 0;
            } else if (CharUtil.Equals(ss, "<=")) {
                return a <= b;
                //return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) <= 0;
            } else if (CharUtil.Equals(ss, '>')) {
                return a > b;
                //return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) > 0;
            } else if (CharUtil.Equals(ss, ">=")) {
                return (a >= b);
                //return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) >= 0;
            } else if (CharUtil.Equals(ss, "=", "==", "===")) {
                return a == b;
                //return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) == 0;
            }
            return a != b;
            //return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) != 0;
        }

        public static int F_base_gcd(List<decimal> list)
        {
            list = list.OrderBy(q => q).ToList();
            var g = F_base_gcd((int)list[1], (int)list[0]);
            for (int i = 2; i < list.Count; i++) {
                g = F_base_gcd((int)list[i], g);
            }
            return g;
        }
        public static int F_base_gcd(int a, int b)
        {
            if (b == 1) { return 1; }
            if (b == 0) { return a; }
            return F_base_gcd(b, a % b);
        }
        public static int F_base_lgm(List<decimal> list)
        {
            list = list.OrderBy(q => q).ToList();
            list.RemoveAll(q => q <= 1);

            int a = (int)list[0];
            for (int i = 1; i < list.Count; i++) {
                int b = (int)list[i];
                int g = b > a ? F_base_gcd(b, a) : F_base_gcd(a, b);
                a = a / g * b;
            }
            return a;
        }
        public static int F_base_Factorial(int a)
        {
            if (a == 0) { return 1; }
            int r = 1;
            for (int i = a; i > 0; i--) {
                r *= i;
            }
            return r;
        }

        public static Tuple<string, decimal> sumifMatch(string s)
        {
            var c = s[0];
            if (c == '>' || c == '＞') {
                if (s.Length > 1 && (s[1] == '=' || s[1] == '＝')) {
                    if (decimal.TryParse(s.AsSpan(2).Trim(), out decimal d)) {
                        return Tuple.Create(">=", d);
                    }
                } else if (decimal.TryParse(s.AsSpan(1).Trim(), out decimal d)) {
                    return Tuple.Create(">", d);
                }
            } else if (c == '<' || c == '＜') {
                if (s.Length > 1 && (s[1] == '=' || s[1] == '＝')) {
                    if (decimal.TryParse(s.AsSpan(2).Trim(), out decimal d)) {
                        return Tuple.Create("<=", d);
                    }
                } else if (s.Length > 1 && (s[1] == '>' || s[1] == '＞')) {
                    if (decimal.TryParse(s.AsSpan(2).Trim(), out decimal d)) {
                        return Tuple.Create("!=", d);
                    }
                } else if (decimal.TryParse(s.AsSpan(1).Trim(), out decimal d)) {
                    return Tuple.Create("<", d);
                }
            } else if (c == '=' || c == '＝') {
                var index = 1;
                if (s.Length > 1 && (s[1] == '=' || s[1] == '＝')) {
                    index = 2;
                    if (s.Length > 2 && (s[2] == '=' || s[2] == '＝')) {
                        index = 3;
                    }
                }
                if (decimal.TryParse(s.AsSpan(index).Trim(), out decimal d)) {
                    return Tuple.Create("=", d);
                }
            } else if (c == '!' || c == '！') {
                var index = 1;
                if (s.Length > 1 && (s[1] == '=' || s[1] == '＝')) {
                    index = 2;
                    if (s.Length > 2 && (s[2] == '=' || s[2] == '＝')) {
                        index = 3;
                    }
                }
                if (decimal.TryParse(s.AsSpan(index).Trim(), out decimal d)) {
                    return Tuple.Create("!=", d);
                }
            }
            return null;
        }

    }
}
