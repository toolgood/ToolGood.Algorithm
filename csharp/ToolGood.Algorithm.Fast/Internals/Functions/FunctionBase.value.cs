using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions
{
    internal class Function_Value : FunctionBase
    {
        private Operand _value;

        public Function_Value(Operand value)
        {
            _value = value;
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            return _value;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            if (_value.Type == OperandType.TEXT) {
                stringBuilder.Append('"');
                var stringValue = _value.TextValue;
                stringValue = stringValue.Replace("\\", "\\\\");
                stringValue = stringValue.Replace("\r", "\\r");
                stringValue = stringValue.Replace("\n", "\\n");
                stringValue = stringValue.Replace("\t", "\\t");
                stringValue = stringValue.Replace("\0", "\\0");
                stringValue = stringValue.Replace("\v", "\\v");
                stringValue = stringValue.Replace("\a", "\\a");
                stringValue = stringValue.Replace("\b", "\\b");
                stringValue = stringValue.Replace("\f", "\\f");
                stringValue = stringValue.Replace("\"", "\\\"");
                stringBuilder.Append(stringValue);
                stringBuilder.Append('"');
            } else if (_value.Type == OperandType.DATE) {
                stringBuilder.Append('"');
                stringBuilder.Append(_value.DateValue.ToString());
                stringBuilder.Append('"');
            } else if (_value.Type == OperandType.BOOLEAN) {
                stringBuilder.Append(_value.BooleanValue? "true" : "false");
            } else {
                stringBuilder.Append(_value.ToString());
            }
        }

    }

    internal class Function_ERROR : Function_1
    {
        public Function_ERROR(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var t = func1.Calculate(work).TextValue;
            return Operand.Error(t);
        }

        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ERROR(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(")");
        }
    }

    internal class Function_Array : Function_N
    {
        public Function_Array(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Calculate(work); if (aa.IsError) { return aa; } args.Add(aa); }
            return Operand.Create(args);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("Array(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_NUM : FunctionBase
    {
        private decimal d;
        private string unit;

        public Function_NUM(decimal func1, string func2)
        {
            d = func1;
            unit = func2;
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var dict = NumberUnitTypeHelper.GetUnitTypedict();
            var d2 = NumberUnitTypeHelper.TransformationUnit(d, dict[unit], work.DistanceUnit, work.AreaUnit, work.VolumeUnit, work.MassUnit);
            return Operand.Create(d2);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append(d);
            stringBuilder.Append(unit);
        }
    }

    internal class Function_PARAMETER : FunctionBase
    {
        private string name;
        private FunctionBase func1;

        public Function_PARAMETER(string name)
        {
            this.name = name;
        }

        public Function_PARAMETER(FunctionBase func1)
        {
            this.func1 = func1;
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            if (string.IsNullOrEmpty(name)) {
                return work.GetParameter(func1.Calculate(work).TextValue);
            }
            return work.GetParameter(name);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("[");
            if (string.IsNullOrEmpty(name)) {
                func1.ToString(stringBuilder, false);
            } else {
                stringBuilder.Append(name);
            }
            stringBuilder.Append("]");
        }
    }

    internal class Function_GetJsonValue : Function_2
    {
        public Function_GetJsonValue(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var obj = func1.Calculate(work); if (obj.IsError) { return obj; }
            var op = func2.Calculate(work); if (op.IsError) { return op; }

            if (obj.Type == OperandType.ARRARY) {
                op = op.ToNumber("ARRARY index is error!");
                if (op.IsError) { return op; }
                var index = op.IntValue - work.ExcelIndex;
                if (index < obj.ArrayValue.Count)
                    return obj.ArrayValue[index];
                return Operand.Error($"ARRARY index {index} greater than maximum length!");
            }
            if (obj.Type == OperandType.ARRARYJSON) {
                if (op.Type == OperandType.NUMBER) {
                    if (((OperandKeyValueList)obj).TryGetValue(op.NumberValue.ToString(), out Operand operand)) {
                        return operand;
                    }
                    return Operand.Error($"Parameter name `{op.TextValue}` is missing!");
                } else if (op.Type == OperandType.TEXT) {
                    if (((OperandKeyValueList)obj).TryGetValue(op.TextValue, out Operand operand)) {
                        return operand;
                    }
                    return Operand.Error($"Parameter name `{op.TextValue}` is missing!");
                }
                return Operand.Error("Parameter name is missing!");
            }

            if (obj.Type == OperandType.JSON) {
                var json = obj.JsonValue;
                if (json.IsArray) {
                    op = op.ToNumber("JSON parameter index is error!");
                    if (op.IsError) { return op; }
                    var index = op.IntValue - work.ExcelIndex;
                    if (index < json.Count) {
                        var v = json[index];
                        if (v.IsString) return Operand.Create(v.StringValue);
                        if (v.IsBoolean) return Operand.Create(v.BooleanValue);
                        if (v.IsDouble) return Operand.Create(v.NumberValue);
                        if (v.IsObject) return Operand.Create(v);
                        if (v.IsArray) return Operand.Create(v);
                        if (v.IsNull) return Operand.CreateNull();
                        return Operand.Create(v);
                    }
                    return Operand.Error($"JSON index {index} greater than maximum length!");
                } else {
                    op = op.ToText("JSON parameter name is error!");
                    if (op.IsError) { return op; }
                    var v = json[op.TextValue];
                    if (v != null) {
                        if (v.IsString) return Operand.Create(v.StringValue);
                        if (v.IsBoolean) return Operand.Create(v.BooleanValue);
                        if (v.IsDouble) return Operand.Create(v.NumberValue);
                        if (v.IsObject) return Operand.Create(v);
                        if (v.IsArray) return Operand.Create(v);
                        if (v.IsNull) return Operand.CreateNull();
                        return Operand.Create(v);
                    }
                }
            }
            return Operand.Error(" Operator is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            func1.ToString(stringBuilder, false);
            stringBuilder.Append("[");
            func2.ToString(stringBuilder, false);
            stringBuilder.Append("]");
        }
    }


    internal class Function_DiyFunction : Function_N
    {
        private string funName;

        public Function_DiyFunction(string name, FunctionBase[] funcs) : base(funcs)
        {
            this.funName = name;
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Calculate(work); args.Add(aa); }
            return work.ExecuteDiyFunction(funName, args);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append(funName);
            stringBuilder.Append("(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_PARAM : Function_2
    {
        public Function_PARAM(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText(); if (args1.IsError) return args1; }
            var result = work.GetParameter(args1.TextValue);
            if (result.IsError) {
                if (func2 != null) {
                    return func2.Calculate(work);
                }
            }
            return result;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("PARAM(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_ArrayJson : Function_N
    {
        public Function_ArrayJson(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            OperandKeyValueList result = new OperandKeyValueList(null);
            foreach (var item in funcs) {
                var o = item.Calculate(work);
                result.AddValue((KeyValue)((OperandKeyValue)o).Value);
            }
            return result;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("{");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append('}');
        }
    }

    internal class Function_ArrayJsonItem : Function_1
    {
        private string key;

        public Function_ArrayJsonItem(string key, FunctionBase func1) : base(func1)
        {
            this.key = key;
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            KeyValue keyValue = new KeyValue();
            keyValue.Key = key;
            keyValue.Value = func1.Calculate(work);
            return new OperandKeyValue(keyValue);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append(key);
            stringBuilder.Append(":");
            func1.ToString(stringBuilder, false);
        }

    }

}
