import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_GetJsonValue extends Function_2 {
    get Name() {
        return "GetJsonValue";
    }

    constructor(z, op) {
        super(z, op);
    }

    evaluate(work, tempParameter) {
        let obj = this.a.evaluate(work, tempParameter);
        // 与 C# 一致:错误或空值直接传播
        if (obj.IsError || obj.IsNull) {
            return obj;
        }
        let op = this.b.evaluate(work, tempParameter);
        if (op.IsError || op.IsNull) {
            return op;
        }

        if (obj.IsArray) {
            op = op.ToNumber('Function \'{0}\'' + ' ARRAY index is error!', 'GetJsonValue');
            if (op.IsError) {
                return op;
            }
            let index = op.IntValue - work.ExcelIndex;
            // 与 C# 一致:负索引视为越界,返回错误而非 undefined
            if (index < obj.ArrayValue.length && index >= 0) {
                return obj.ArrayValue[index];
            }
            return Operand.Error('Function \'{0}\'' + ' ARRAY index {1} out of range!', 'GetJsonValue', index);
        }
        if (obj.IsArrayJson) {
            if (op.IsNumber) {
                let operand = obj.TryGetValue(op.NumberValue.toString());
                if (operand !== null) {
                    return operand;
                }
                return Operand.Error('Function \'{0}\'' + ' Parameter name \'{1}\'' + ' is missing!', 'GetJsonValue', op.NumberValue);
            } else if (op.IsText) {
                let operand = obj.TryGetValue(op.TextValue);
                if (operand !== null) {
                    return operand;
                }
                return Operand.Error('Function \'{0}\'' + ' Parameter name \'{1}\'' + ' is missing!', 'GetJsonValue', op.TextValue);
            }
            return Operand.Error('Function \'{0}\'' + ' Parameter name is missing!', 'GetJsonValue');
        }

        if (obj.IsJson) {
            let json = obj.JsonValue;
            if (json.IsArray) {
                op = op.ToNumber('Function \'{0}\'' + ' JSON parameter index is error!', 'GetJsonValue');
                if (op.IsError) {
                    return op;
                }
                let index = op.IntValue - work.ExcelIndex;
                if (index < json.inst_array.length) {
                    let v = json.inst_array[index];
                    if (v.IsString) {
                        return Operand.Create(v.StringValue);
                    }
                    if (v.IsBoolean) {
                        return Operand.Create(v.BooleanValue);
                    }
                    if (v.IsDouble) {
                        return Operand.Create(v.NumberValue);
                    }
                    if (v.IsObject) {
                        return Operand.Create(v);
                    }
                    if (v.IsArray) {
                        return Operand.Create(v);
                    }
                    if (v.IsNull) {
                        return Operand.CreateNull();
                    }
                    return Operand.Create(v);
                }
                return Operand.Error('Function \'{0}\' JSON index {1} out of range!', 'GetJsonValue', index);
            } else if (json.IsObject) {
                op = op.ToText('Function \'{0}\' JSON parameter name is error!', 'GetJsonValue');
                if (op.IsError) {
                    return op;
                }
                let v = json.inst_object[op.TextValue];
                if (v) {
                    if (v.IsString) {
                        return Operand.Create(v.StringValue);
                    }
                    if (v.IsBoolean) {
                        return Operand.Create(v.BooleanValue);
                    }
                    if (v.IsDouble) {
                        return Operand.Create(v.NumberValue);
                    }
                    if (v.IsObject) {
                        return Operand.Create(v);
                    }
                    if (v.IsArray) {
                        return Operand.Create(v);
                    }
                    if (v.IsNull) {
                        return Operand.CreateNull();
                    }
                    return Operand.Create(v);
                }
            }
        }
        return Operand.Error('Function \'{0}\' Operator is error!', 'GetJsonValue');
    }

    toString2(stringBuilder, addBrackets) {
        this.a.toString2(stringBuilder, false);
        stringBuilder.append('[');
        this.b.toString2(stringBuilder, false);
        stringBuilder.append(']');
    }

}

export { Function_GetJsonValue };
