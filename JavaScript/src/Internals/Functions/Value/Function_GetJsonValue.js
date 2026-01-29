import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_GetJsonValue extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        const obj = this.func1.Evaluate(engine, tempParameter);
        if (obj.isError) {
            return obj;
        }
        const op = this.func2.Evaluate(engine, tempParameter);
        if (op.isError) {
            return op;
        }

        if (obj.isArray) {
            op = op.ToNumber('ARRARY index is error!');
            if (op.isError) {
                return op;
            }
            const index = op.intValue - engine.excelIndex;
            if (index < obj.arrayValue.length) {
                return obj.arrayValue[index];
            }
            return Operand.error('ARRARY index {0} greater than maximum length!', index);
        }
        if (obj.isArrayJson) {
                if (op.isNumber) {
                    const operand = obj.tryGetValue(op.numberValue.toString());
                    if (operand !== undefined) {
                        return operand;
                    }
                    return Operand.error('Parameter name {0} is missing!', op.textValue);
                } else if (op.isText) {
                    const operand = obj.tryGetValue(op.textValue);
                    if (operand !== undefined) {
                        return operand;
                    }
                    return Operand.error('Parameter name {0} is missing!', op.textValue);
                }
                return Operand.error('Parameter name is missing!');
            }

        if (obj.isJson) {
            const json = obj.jsonValue;
            if (Array.isArray(json)) {
                op = op.ToNumber('JSON parameter index is error!');
                if (op.isError) {
                    return op;
                }
                const index = op.intValue - engine.excelIndex;
                if (index < json.length) {
                    const v = json[index];
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
                if (op.isError) {
                    return op;
                }
                const v = json[op.textValue];
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
