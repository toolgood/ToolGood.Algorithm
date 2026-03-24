import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_IFS extends Function_N {
    get Name() {
        return "Ifs";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        for (let i = 0; i < this.z.length - 1; i += 2) {
            let condition = this.getBoolean(engine, tempParameter, i);
            if (condition.IsError) { return condition; }
            if (condition.BooleanValue) {
                return this.z[i + 1].evaluate(engine, tempParameter);
            }
        }
        return this.functionError();
    }
}

export { Function_IFS };