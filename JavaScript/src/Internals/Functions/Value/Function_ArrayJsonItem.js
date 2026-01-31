import { Function_1 } from '../Function_1.js';
import { OperandKeyValue } from '../../../Operand.js';

class Function_ArrayJsonItem extends Function_1 {
    constructor(key, func1) {
        super(func1);
        this.key = key;
    }

    Evaluate(engine, tempParameter) {
        let keyValue = {
            key: this.key,
            value: this.func1.Evaluate(engine, tempParameter)
        };
        return new OperandKeyValue(keyValue);
    }

}

export { Function_ArrayJsonItem };
