import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_GetJsonValue extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let obj = this.func1.Evaluate(engine, tempParameter);
        if (obj.IsError) {
            return obj;
        }
        let op = this.func2.Evaluate(engine, tempParameter);
        if (op.IsError) {
            return op;
        }

        if (obj.IsArray) {
            op = op.ToNumber('ARRARY index is error!');
            if (op.IsError) {
                return op;
            }
            let index = op.IntValue - engine.ExcelIndex;
            if (index < obj.ArrayValue.length) {
                return Operand.Create(obj.ArrayValue[index]);
            }
            return Operand.Error('ARRARY index {0} greater than maximum length!', index);
        }
        // 首先尝试作为 ArrayJson 处理
        if (obj.IsArrayJson) {
            if (op.IsNumber) {
                let operand = obj.TryGetValue(op.NumberValue.toString());
                if (operand !== null) {
                    return operand;
                }
                return Operand.Error('Parameter name {0} is missing!', op.TextValue);
            } else if (op.IsText) {
                let operand = obj.TryGetValue(op.TextValue);
                if (operand !== null) {
                    return operand;
                }
                return Operand.Error('Parameter name {0} is missing!', op.TextValue);
            }
            return Operand.Error('Parameter name is missing!');
        }

        // 然后尝试作为 Json 处理
        if (obj.IsJson) {
            let json = obj.JsonValue;
            if (json.IsArray) {
                op = op.ToNumber('JSON parameter index is error!');
                if (op.IsError) {
                    return op;
                }
                let index = op.IntValue - engine.ExcelIndex;
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
                    if (v.IsNull) {
                        return Operand.CreateNull();
                    }
                    return Operand.Create(v);
                }
                return Operand.Error('JSON index {0} greater than maximum length!', index);
            } else if (json.IsObject) {
                op = op.ToText('JSON parameter name is error!');
                if (op.IsError) {
                    return op;
                }
                
                // 尝试直接访问属性
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
                    if (v.IsNull) {
                        return Operand.CreateNull();
                    }
                    return Operand.Create(v);
                }
                
                // 尝试访问带引号的属性
                let quotedKey = '"' + op.TextValue + '"';
                v = json.inst_object[quotedKey];
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
                    if (v.IsNull) {
                        return Operand.CreateNull();
                    }
                    return Operand.Create(v);
                }
            }
        }
        
        // 最后尝试作为普通对象处理
        if (typeof obj._value === 'object' && obj._value !== null) {
            op = op.ToText('Parameter name is error!');
            if (op.IsError) {
                return op;
            }
            
            let v = obj._value[op.TextValue];
            if (v !== undefined) {
                return Operand.Create(v);
            }
        }
        return Operand.Error('Operator is error!');
    }

    toString(stringBuilder, addBrackets) {
        this.func1.toString(stringBuilder, false);
        stringBuilder.append('[');
        this.func2.toString(stringBuilder, false);
        stringBuilder.append(']');
    }
}

export { Function_GetJsonValue };
