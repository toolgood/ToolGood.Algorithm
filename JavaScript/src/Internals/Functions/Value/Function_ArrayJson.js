import { Function_N } from '../Function_N.js';
import { OperandKeyValueList } from '../../../Operand.js';

class Function_ArrayJson extends Function_N {
    get Name() {
        return "ArrayJson";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(work, tempParameter) {
        let result = new OperandKeyValueList();
        for (let item of this.z) {
            let o = item.Evaluate(work, tempParameter);
            result.AddValue(o._value);
        }
        return result;
    }

    ToString(stringBuilder, addBrackets) {
        stringBuilder.append('{');
        for (let i = 0; i < this.z.length; i++) {
            if (i > 0) {
                stringBuilder.append(", ");
            }
            this.z[i].ToString(stringBuilder, false);
        }
        stringBuilder.append('}');
    }

}

export { Function_ArrayJson };
