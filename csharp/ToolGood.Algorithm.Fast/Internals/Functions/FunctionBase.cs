using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.math;

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
            var o1 = F_base_GetList(args1, list1);
            var o2 = F_base_GetList(args2, list2);
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
        private static bool F_base_GetList(Operand args, List<decimal> list)
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
            var o1 = F_base_GetList(args1, list1);
            var o2 = F_base_GetList(args2, list2);
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
        private static bool F_base_GetList(Operand args, List<decimal> list)
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
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MULTINOMIAL parameter is error!"); }

            int sum = 0;
            int n = 1;
            for (int i = 0; i < list.Count; i++) {
                var a = (int)list[i];
                n *= F_base_Factorial(a);
                sum += a;
            }

            var r = F_base_Factorial(sum) / n;
            return Operand.Create(r);
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
        private static int F_base_Factorial(int a)
        {
            if (a == 0) { return 1; }
            int r = 1;
            for (int i = a; i > 0; i--) {
                r *= i;
            }
            return r;
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
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function PRODUCT parameter is error!"); }

            decimal d = 1;
            for (int i = 0; i < list.Count; i++) {
                var a = list[i];
                d *= a;
            }
            return Operand.Create(d);
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
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function SUMSQ parameter is error!"); }

            decimal d = 0;
            for (int i = 0; i < list.Count; i++) {
                var a = list[i];
                d += a * a;
            }
            return Operand.Create(d);
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


    public class Function_CONCAT : Function_N
    {
        public Function_CONCAT(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in funcs) {
                var arg = item.Accept(work);
                if (arg.IsError) { return arg; }
                var s = arg.ToText(null);
                if (s.IsError) { return s; }
                sb.Append(s.TextValue);
            }
            return Operand.Create(sb.ToString());
        }
    }
    public class Function_TEXTJOIN : Function_N
    {
        public Function_TEXTJOIN(FunctionBase[] funcs) : base(funcs)
        {
        }
        public override Operand Accept(Work work)
        {
            if (funcs.Length < 2) {
                return Operand.Error("Function TEXTJOIN parameter is error!");
            }
            var argSep = funcs[0].Accept(work);
            if (argSep.IsError) { return argSep; }
            var sep = argSep.ToText("Function TEXTJOIN parameter 1 is error!");
            if (sep.IsError) { return sep; }
            var argIgnoreEmpty = funcs[1].Accept(work);
            if (argIgnoreEmpty.IsError) { return argIgnoreEmpty; }
            var ignoreEmpty = argIgnoreEmpty.ToBool("Function TEXTJOIN parameter 2 is error!");
            if (ignoreEmpty.IsError) { return ignoreEmpty; }
            List<string> list = new List<string>();
            for (int i = 2; i < funcs.Length; i++) {
                var arg = funcs[i].Accept(work);
                if (arg.IsError) { return arg; }
                var s = arg.ToText(null);
                if (s.IsError) { return s; }
                if (ignoreEmpty.BoolValue && string.IsNullOrEmpty(s.TextValue)) {
                    continue;
                }
                list.Add(s.TextValue);
            }
            return Operand.Create(string.Join(sep.TextValue, list));
        }
    }
    public class Function_UNICHAR : Function_1
    {
        public Function_UNICHAR(FunctionBase func1) : base(func1)
        {
        }
        public override Operand Accept(Work work)
        {
            var args1 = func1.Accept(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function UNICHAR parameter is error!"); if (args1.IsError) { return args1; } }
            char c = (char)(int)args1.NumberValue;
            return Operand.Create(c.ToString());
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
            var n = args1.IntValue;
            if (n < 0) { return Operand.Error("Function FACTORIAL parameter is error!"); }
            if (n > 170) { return Operand.Error("Function FACTORIAL parameter is too large!"); }
            double result = 1;
            for (int i = 2; i <= n; i++) {
                result *= i;
            }
            return Operand.Create((decimal)result);
        }
    }
}
