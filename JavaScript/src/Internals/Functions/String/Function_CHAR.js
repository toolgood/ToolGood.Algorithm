import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_CHAR extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(work, tempParameter) {
        const args1 = this.func1.evaluate(work, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function \'{0}\' parameter is error!', 'Char');
            if (args1.isError) {
                return args1;
            }
        }
        const c = String.fromCharCode(args1.intValue);
        return Operand.create(c);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Char');
    }
}

export { Function_CHAR };
