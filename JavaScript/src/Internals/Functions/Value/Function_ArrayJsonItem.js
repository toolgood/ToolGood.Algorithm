import { Function_1 } from '../Function_1.js';
import { OperandKeyValue } from '../../../Operand.js';

class Function_ArrayJsonItem extends Function_1 {
    get Name() {
        return "ArrayJsonItem";
    }

    constructor(key, a) {
        super(a);
        this.key = key;
    }

    evaluate(work, tempParameter) {
        let keyValue = {
            key: this.key,
            value: this.a.evaluate(work, tempParameter)
        };
        return new OperandKeyValue(keyValue);
    }

    toString2(stringBuilder, addBrackets) {
        stringBuilder.append(this.key);
        stringBuilder.append(':');
        this.a.toString2(stringBuilder, false);
    }

}

export { Function_ArrayJsonItem };
