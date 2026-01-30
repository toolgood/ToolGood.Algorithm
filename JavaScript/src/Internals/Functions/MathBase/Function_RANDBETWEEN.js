import { Function_2 } from '../Function_2.js';

class Function_RANDBETWEEN extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "RandBetween", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "RandBetween", 2);
            if (args2.IsError) { return args2; }
        }
        return Operand.Create(Math.random() * (args2.NumberValue - args1.NumberValue) + args1.NumberValue);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "RandBetween");
    }
}

export { Function_RANDBETWEEN };
