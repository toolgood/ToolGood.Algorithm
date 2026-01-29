import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_DEGREES extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function \'{0}\' parameter is error!', 'Degrees');
            if (args1.isError) {
                return args1;
            }
        }
        const z = args1.doubleValue;
        const r = (z / Math.PI * 180);
        return Operand.create(r);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Degrees');
    }
}

export { Function_DEGREES };
