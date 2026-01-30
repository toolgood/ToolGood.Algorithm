import { FunctionBase } from '../FunctionBase.js';

class Function_RAND extends FunctionBase {
    constructor() {
        super();
    }

    Evaluate(engine, tempParameter) {
        return Operand.Create(Math.random());
    }

    toString(stringBuilder, addBrackets) {
        stringBuilder.append("Rand()");
    }
}

export { Function_RAND };
