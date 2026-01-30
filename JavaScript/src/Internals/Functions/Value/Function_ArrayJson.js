import { Function_N } from '../Function_N.js';
import { OperandKeyValueList } from '../../../Operand.js';

class Function_ArrayJson extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let result = new OperandKeyValueList();
        for (let item of this.funcs) {
            let o = item.Evaluate(engine, tempParameter);
            result.AddValue(o._value);
        }
        return result;
    }

    toString(stringBuilder, addBrackets) {
        stringBuilder.append('{');
        for (let i = 0; i < this.funcs.length; i++) {
            if (i > 0) {
                stringBuilder.append(', ');
            }
            this.funcs[i].toString(stringBuilder, false);
        }
        stringBuilder.append('}');
    }
}

export { Function_ArrayJson };
