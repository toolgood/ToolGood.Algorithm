import { Function_2 } from '../Function_2.js';

class Function_FLOOR extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Floor", 1);
            if (args1.IsError) { return args1; }
        }
        if (this.func2 === null) {
            return engine.createOperand(Math.floor(args1.NumberValue));
        }

        let args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Floor", 2);
            if (args2.IsError) { return args2; }
        }
        const b = args2.NumberValue;
        if (b >= 1) {
            return engine.createOperand(args1.IntValue);
        }
        if (b <= 0) {
            return engine.createErrorOperand("Function '{0}' parameter {1} is error!", "Floor", 2);
        }

        const a = args1.NumberValue;
        const d = Math.floor(a / b) * b;
        return engine.createOperand(d);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Floor");
    }
}

export { Function_FLOOR };
