import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_FISHER extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function {0} parameter is error!', 'Fisher');
            if (args1.isError) {
                return args1;
            }
        }
        const x = args1.doubleValue;
        if (x >= 1 || x <= -1) {
            return Operand.error('Function {0} parameter is error!', 'Fisher');
        }
        const n = 0.5 * Math.log((1 + x) / (1 - x));
        return Operand.create(n);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Fisher');
    }
}

export { Function_FISHER };
