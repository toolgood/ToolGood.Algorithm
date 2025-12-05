using System;
using System.Globalization;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Fast.Internals.Functions
{
    public class Work
    {

    }

    public abstract class FunctionBase
    {
        public abstract Operand Accept(Work work);
    }

    public class Function_Value : FunctionBase
    {
        private Operand _value;
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

    public class Function_AND_N : FunctionBase
    {
        private FunctionBase[] funcs;

        public Function_AND_N(FunctionBase[] funcs)
        {
            this.funcs = funcs;
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
    public class Function_OR_N : FunctionBase
    {
        private FunctionBase[] funcs;

        public Function_OR_N(FunctionBase[] funcs)
        {
            this.funcs = funcs;
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



    #endregion


    #endregion


}
