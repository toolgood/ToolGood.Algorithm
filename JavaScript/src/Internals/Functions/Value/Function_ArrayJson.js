import { Function_N } from '../Function_N.js';
import { OperandKeyValueList } from '../../../Operand.js';

class Function_ArrayJson extends Function_N {
    get Name() {
        return "ArrayJson";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let result = new OperandKeyValueList();
        for (let item of this.z) {
            let o = item.evaluate(work, tempParameter);
            result.AddValue(o._value);
        }
        return result;
    }

    toString2(stringBuilder, addBrackets) {
        stringBuilder.append('{');
        for (let i = 0; i < this.z.length; i++) {
            if (i > 0) {
                stringBuilder.append(", ");
            }
            this.z[i].toString2(stringBuilder, false);
        }
        stringBuilder.append('}');
    }

}

export { Function_ArrayJson };
