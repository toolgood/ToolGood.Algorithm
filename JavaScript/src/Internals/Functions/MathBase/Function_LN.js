import { Function_1 } from '../Function_1.js';

class Function_LN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Ln");
            if (args1.IsError) { return args1; }
        }
        let z = args1.DoubleValue;
        if (z <= 0) {
            return engine.createErrorOperand("Function '{0}' parameter is error!", "Ln");
        }
        return engine.createOperand(Math.log(z));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Ln");
    }
}

export { Function_LN };
