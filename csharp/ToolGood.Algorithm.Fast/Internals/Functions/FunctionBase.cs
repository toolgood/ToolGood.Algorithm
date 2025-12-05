using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Fast.Internals.Functions
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
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function GCD parameter is error!"); }

            return Operand.Create(F_base_gcd(list));
        }
        private int F_base_gcd(List<decimal> list)
        {
            list = list.OrderBy(q => q).ToList();
            var g = F_base_gcd((int)list[1], (int)list[0]);
            for (int i = 2; i < list.Count; i++) {
                g = F_base_gcd((int)list[i], g);
            }
            return g;
        }
        private int F_base_gcd(int a, int b)
        {
            if (b == 1) { return 1; }
            if (b == 0) { return a; }
            return F_base_gcd(b, a % b);
        }
        private static bool F_base_GetList(List<Operand> args, List<decimal> list)
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
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function LCM parameter is error!"); }

            return Operand.Create(F_base_lgm(list));
        }
        private int F_base_lgm(List<decimal> list)
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

        private int F_base_gcd(int a, int b)
        {
            if (b == 1) { return 1; }
            if (b == 0) { return a; }
            return F_base_gcd(b, a % b);
        }
        private static bool F_base_GetList(List<Operand> args, List<decimal> list)
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
    public class Function_COSH: Function_1
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

    #endregion


}
