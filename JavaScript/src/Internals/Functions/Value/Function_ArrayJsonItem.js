import { Function_1 } from '../Function_1.js';
import { OperandKeyValue } from '../../../Operand.js';

class Function_ArrayJsonItem extends Function_1 {
    constructor(key, a) {
        super(a);
        this.key = key;
    }

    Evaluate(engine, tempParameter) {
        let keyValue = {
            key: this.key,
            value: this.a.Evaluate(engine, tempParameter)
        };
        return new OperandKeyValue(keyValue);
    }

}

export { Function_ArrayJsonItem };
