import { Function_1 } from '../Function_1';

class Function_ABS extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Abs");
            if (args1.IsError) { return args1; }
        }
        return engine.createOperand(Math.abs(args1.NumberValue));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Abs");
    }
}

export { Function_ABS };
