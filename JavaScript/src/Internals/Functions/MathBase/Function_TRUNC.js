import { Function_1 } from '../Function_1.js';

class Function_TRUNC extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Trunc");
            if (args1.IsError) { return args1; }
        }
        return engine.createOperand(Math.trunc(args1.NumberValue));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Trunc");
    }
}

export { Function_TRUNC };
