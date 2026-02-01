import { Function_N } from '../Function_N.js';
import { OperandKeyValueList } from '../../../Operand.js';

class Function_ArrayJson extends Function_N {
    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let result = new OperandKeyValueList();
        for (let item of this.z) {
            let o = item.Evaluate(engine, tempParameter);
            result.AddValue(o._value);
        }
        return result;
    }


}

export { Function_ArrayJson };
