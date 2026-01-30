import { Function_1 } from '../Function_1.js';

class Function_FACTDOUBLE extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "FactDouble");
            if (args1.IsError) { return args1; }
        }
        let z = args1.IntValue;
        if (z < 0) {
            return engine.createErrorOperand("Function '{0}' parameter is error!", "FactDouble");
        }

        let d = 1;
        for (let i = z; i > 0; i -= 2) {
            d *= i;
        }
        return Operand.Create(d);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "FactDouble");
    }
}

export { Function_FACTDOUBLE };
