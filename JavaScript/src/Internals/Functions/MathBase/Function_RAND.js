import { FunctionBase } from '../FunctionBase.js';
import { Operand } from '../../../Operand.js';

class Function_RAND extends FunctionBase {
    constructor() {
        super();
    }

    Evaluate(engine, tempParameter) {
        return Operand.Create(Math.random());
    }

    toString(stringBuilder, addBrackets) {
        stringBuilder.push("Rand()");
    }
}

export { Function_RAND };
