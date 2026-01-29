import { Function_2 } from '../Function_2';

class Function_ROUND extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Round", 1);
            if (args1.IsError) { return args1; }
        }

        if (this._arg2 === null) {
            return engine.createOperand(Math.round(args1.NumberValue));
        }
        let args2 = this._arg2.evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Round", 2);
            if (args2.IsError) { return args2; }
        }
        const decimalPlaces = args2.IntValue;
        const factor = Math.pow(10, decimalPlaces);
        return engine.createOperand(Math.round(args1.NumberValue * factor) / factor);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Round");
    }
}

export { Function_ROUND };
