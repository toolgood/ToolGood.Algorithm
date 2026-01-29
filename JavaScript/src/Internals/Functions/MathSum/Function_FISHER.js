import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_FISHER extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function {0} parameter is error!', 'Fisher');
            if (args1.IsError) {
                return args1;
            }
        }
        const x = args1.DoubleValue;
        if (x >= 1 || x <= -1) {
            return Operand.error('Function {0} parameter is error!', 'Fisher');
        }
        const n = 0.5 * Math.log((1 + x) / (1 - x));
        return Operand.Create(n);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Fisher');
    }
}

export { Function_FISHER };
