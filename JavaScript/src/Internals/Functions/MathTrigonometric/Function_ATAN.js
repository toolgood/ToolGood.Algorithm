import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ATAN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(work, tempParameter) {
        const args1 = this.func1.evaluate(work, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function \'{0}\' parameter is error!', 'Atan');
            if (args1.isError) {
                return args1;
            }
        }
        return Operand.create(Math.atan(args1.doubleValue));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Atan');
    }
}

export { Function_ATAN };
