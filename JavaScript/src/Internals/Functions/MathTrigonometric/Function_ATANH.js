import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ATANH extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotNumber) {
            args1.ToNumber('Function \'{0}\' parameter is error!', 'Atanh');
            if (args1.isError) {
                return args1;
            }
        }
        const x = args1.doubleValue;
        if (x >= 1 || x <= -1) {
            return Operand.error('Function \'{0}\' parameter is error!', 'Atanh');
        }
        return Operand.Create(Math.atanh(x));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Atanh');
    }
}

export { Function_ATANH };
