import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_FISHERINV extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function {0} parameter is error!', 'FisherInv');
            if (args1.isError) {
                return args1;
            }
        }
        const x = args1.doubleValue;
        const n = (Math.exp(2 * x) - 1) / (Math.exp(2 * x) + 1);
        return Operand.create(n);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'FisherInv');
    }
}

export { Function_FISHERINV };
