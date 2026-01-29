import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_RADIANS extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotNumber) {
            args1.ToNumber('Function \'{0}\' parameter is error!', 'Radians');
            if (args1.isError) {
                return args1;
            }
        }
        const r = args1.doubleValue / 180 * Math.PI;
        return Operand.Create(r);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Radians');
    }
}

export { Function_RADIANS };
