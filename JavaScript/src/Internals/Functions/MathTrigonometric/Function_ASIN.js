import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ASIN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(work, tempParameter) {
        const args1 = this.func1.evaluate(work, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function \'{0}\' parameter is error!', 'Asin');
            if (args1.isError) {
                return args1;
            }
        }
        const x = args1.doubleValue;
        if (x < -1 || x > 1) {
            return Operand.error('Function \'{0}\' parameter is error!', 'Asin');
        }
        return Operand.create(Math.asin(x));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Asin');
    }
}

export { Function_ASIN };
