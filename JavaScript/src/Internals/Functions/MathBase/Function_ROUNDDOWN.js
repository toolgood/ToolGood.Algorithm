import { Function_2 } from '../Function_2.js';

class Function_ROUNDDOWN extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "RoundDown", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "RoundDown", 2);
            if (args2.IsError) { return args2; }
        }
        if (args1.NumberValue == 0.0) {
            return args1;
        }
        let a = Math.pow(10, args2.IntValue);
        let b = args1.NumberValue;

        let result = Math.floor(b * a) / a;
        return engine.createOperand(result);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "RoundDown");
    }
}

export { Function_ROUNDDOWN };
