import { Function_1 } from '../Function_1';

class Function_SQRT extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Sqrt");
            if (args1.IsError) { return args1; }
        }
        if (args1.NumberValue < 0) {
            return engine.createErrorOperand("Function '{0}' parameter is error!", "Sqrt");
        }
        return engine.createOperand(Math.sqrt(args1.DoubleValue));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Sqrt");
    }
}

export { Function_SQRT };
