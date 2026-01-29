import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_FISHERINV extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function {0} parameter is error!', 'FisherInv');
            if (args1.IsError) {
                return args1;
            }
        }
        const x = args1.DoubleValue;
        const n = (Math.exp(2 * x) - 1) / (Math.exp(2 * x) + 1);
        return Operand.Create(n);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'FisherInv');
    }
}

export { Function_FISHERINV };
