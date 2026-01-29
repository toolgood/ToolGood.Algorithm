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
            let index = op.IntValue - engine.excelIndex;
            if (index < obj.ArrayValue.length) {
                return obj.ArrayValue[index];
            }
            return Operand.error('ARRARY index {0} greater than maximum length!', index);
        }
        if (obj.IsArrayJson) {
                if (op.IsNumber) {
                    let operand = obj.tryGetValue(op.NumberValue.toString());
                    if (operand !== undefined) {
                        return operand;
                    }
                    return Operand.error('Parameter name {0} is missing!', op.TextValue);
                } else if (op.isText) {
                    let operand = obj.tryGetValue(op.TextValue);
                    if (operand !== undefined) {
                        return operand;
                    }
                    return Operand.error('Parameter name {0} is missing!', op.TextValue);
                }
                return Operand.error('Parameter name is missing!');
            }

        if (obj.IsJson) {
            let json = obj.JsonValue;
            if (Array.IsArray(json)) {
                op = op.ToNumber('JSON parameter index is error!');
                if (op.IsError) {
                    return op;
                }
                let index = op.IntValue - engine.excelIndex;
                if (index < json.length) {
                    let v = json[index];
                    if (typeof v === 'string') {
                        return Operand.Create(v);
                    }
                    if (typeof v === 'boolean') {
                        return Operand.Create(v);
                    }
                    if (typeof v === 'number') {
                        return Operand.Create(v);
                    }
                    if (typeof v === 'object' && v !== null) {
                        return Operand.Create(v);
                    }
                    if (v === null) {
                        return Operand.createNull();
                    }
                    return Operand.Create(v);
                }
                return Operand.error('JSON index {0} greater than maximum length!', index);
            } else {
                op = op.ToText('JSON parameter name is error!');
                if (op.IsError) {
                    return op;
                }
                let v = json[op.TextValue];
                if (v !== undefined) {
                    if (typeof v === 'string') {
                        return Operand.Create(v);
                    }
                    if (typeof v === 'boolean') {
                        return Operand.Create(v);
                    }
                    if (typeof v === 'number') {
                        return Operand.Create(v);
                    }
                    if (typeof v === 'object' && v !== null) {
                        return Operand.Create(v);
                    }
                    if (v === null) {
                        return Operand.createNull();
                    }
                    return Operand.Create(v);
                }
            }
        }
        return Operand.error('Operator is error!');
    }

    toString(stringBuilder, addBrackets) {
        this.func1.toString(stringBuilder, false);
        stringBuilder.append('[');
        this.func2.toString(stringBuilder, false);
        stringBuilder.append(']');
    }
}

export { Function_GetJsonValue };
