import { Function_0 } from '../Function_0.js';
import { Operand } from '../../../Operand.js';

class Function_NULL extends Function_0 {
    get Name() {
        return "NULL";
    }

    constructor() {
        super();
    }

    evaluate(engine, tempParameter = null) {
        return Operand.CreateNull();
    }
}

export { Function_NULL };