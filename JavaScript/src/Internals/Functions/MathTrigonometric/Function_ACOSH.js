import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ACOSH extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function {0} parameter is error!", 'Acosh');
            if (args1.IsError) {
                return args1;
            }
        }
        let z = args1.DoubleValue;
        if (z < 1) {
            return Operand.Error("Function {0} parameter is error!", 'Acosh');
        }
        return Operand.Create(Math.acosh(z));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Acosh');
    }
}

export { Function_ACOSH };
