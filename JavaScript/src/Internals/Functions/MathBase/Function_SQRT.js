import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_SQRT extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Sqrt");
            if (args1.IsError) { return args1; }
        }
        if (args1.NumberValue < 0) {
            return Operand.Error("Function '{0}' parameter is error!", "Sqrt");
        }
        return Operand.Create(Math.sqrt(args1.DoubleValue));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Sqrt");
    }
}

export { Function_SQRT };
