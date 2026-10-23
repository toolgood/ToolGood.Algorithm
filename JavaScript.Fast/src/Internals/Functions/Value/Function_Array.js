import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_Array extends Function_N {
    get Name() {
        return "Array";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args = [];
        let error = this.tryEvaluateAll(work, tempParameter, args);
        if (error != null) {
            return error;
        }
        let result = Operand.Create(args);
        return result;
    }

}

export { Function_Array };
