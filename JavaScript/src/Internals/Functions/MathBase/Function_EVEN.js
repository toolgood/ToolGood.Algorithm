import { Function_1 } from '../Function_1.js';

class Function_EVEN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Even");
            if (args1.IsError) { return args1; }
        }
        let z = args1.NumberValue;
        if (z % 2 == 0) { return args1; }
        z = Math.ceil(z);
        if (z % 2 == 0) { return engine.createOperand(z); }
        z++;
        return engine.createOperand(z);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Even");
    }
}

export { Function_EVEN };
