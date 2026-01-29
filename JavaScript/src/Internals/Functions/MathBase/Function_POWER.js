import { Function_2 } from '../Function_2.js';

class Function_POWER extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Power", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this._arg2.evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Power", 2);
            if (args2.IsError) { return args2; }
        }
        return engine.createOperand(Math.pow(args1.DoubleValue, args2.DoubleValue));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Power");
    }
}

export { Function_POWER };
