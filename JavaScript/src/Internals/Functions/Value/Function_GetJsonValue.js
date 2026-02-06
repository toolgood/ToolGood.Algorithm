import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_GetJsonValue extends Function_2 {
    get Name() {
        return "GetJsonValue";
    }

    constructor(z, op) {
        super(z, op);
    }

    Evaluate(work, tempParameter) {
        let obj = this.a.Evaluate(work, tempParameter);
        if (obj.IsError) {
            return obj;
        }
        let op = this.b.Evaluate(work, tempParameter);
        if (op.IsError) {
            return op;
        }

        if (obj.IsArray) {
            op = op.ToNumber('Function \'{0}\'' + ' ARRARY index is error!', 'GetJsonValue');
            if (op.IsError) {
                return op;
            }
            let index = op.IntValue - work.ExcelIndex;
            if (index < obj.ArrayValue.length) {
                return obj.ArrayValue[index];
            }
            return Operand.Error('Function \'{0}\'' + ' ARRARY index {1} greater than maximum length!', 'GetJsonValue', index);
        }
        if (obj.IsArrayJson) {
            if (op.IsNumber) {
                let operand = obj.TryGetValue(op.NumberValue.toString());
                if (operand !== null) {
                    return operand;
                }
                return Operand.Error('Function \'{0}\'' + ' Parameter name \'{1}\'' + ' is missing!', 'GetJsonValue', op.TextValue);
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
                if (index < json.length) {
                    let v = json[index];
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
                return Operand.Error('Function \'{0}\' JSON index {1} greater than maximum length!', 'GetJsonValue', index);
            } else if (json.IsObject) {
                op = op.ToText('Function \'{0}\' JSON parameter name is error!', 'GetJsonValue');
                if (op.IsError) {
                    return op;
                }
                let v = json[op.TextValue];
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
        return Operand.Error('Function \'{0}\'' + ' Operator is error!', 'GetJsonValue');
    }

    ToString(stringBuilder, addBrackets) {
        this.a.ToString(stringBuilder, false);
        stringBuilder.append('[');
        this.b.ToString(stringBuilder, false);
        stringBuilder.append(']');
    }

}

export { Function_GetJsonValue };
