import { FunctionBase } from '../FunctionBase.js';

class Function_RAND extends FunctionBase {
    constructor() {
        super();
    }

    evaluate(engine, tempParameter) {
        return engine.createOperand(Math.random());
    }

    toString(stringBuilder, addBrackets) {
        stringBuilder.append("Rand()");
    }
}

export { Function_RAND };
