import { Function_2 } from '../Function_2.js';

class Function_CEILING extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Ceiling", 1);
            if (args1.IsError) { return args1; }
        }

        if (this.func2 === null) {
            return engine.createOperand(Math.ceil(args1.NumberValue));
        }

        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Ceiling", 2);
            if (args2.IsError) { return args2; }
        }
        const b = args2.NumberValue;
        if (b == 0) {
            return engine.createOperand(0);
        }
        if (b < 0) {
            return engine.createErrorOperand("Function '{0}' parameter {1} is error!", "Ceiling", 2);
        }

        const a = args1.NumberValue;
        const d = Math.ceil(a / b) * b;
        return engine.createOperand(d);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Ceiling");
    }
}

export { Function_CEILING };
