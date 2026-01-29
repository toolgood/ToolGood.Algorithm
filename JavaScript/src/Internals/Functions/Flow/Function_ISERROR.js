import { Function_2 } from '../Function_2.js';

class Function_ISERROR extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (this.func2 !== null) {
            if (args1.IsError) {
                return this.func2.evaluate(engine, tempParameter);
            }
            return args1;
        }
        if (args1.IsError) {
            return engine.createBooleanOperand(true);
        }
        return engine.createBooleanOperand(false);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsError");
    }
}

export { Function_ISERROR };
