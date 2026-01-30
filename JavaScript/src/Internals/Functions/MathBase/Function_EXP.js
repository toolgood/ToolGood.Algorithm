import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_EXP extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Exp");
            if (args1.IsError) { return args1; }
        }
        return Operand.Create(Math.exp(args1.DoubleValue));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Exp");
    }
}

export { Function_EXP };
