import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ATANH extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber('Function \'{0}\' parameter is error!', 'Atanh');
            if (args1.IsError) {
                return args1;
            }
        }
        let x = args1.DoubleValue;
        if (x >= 1 || x <= -1) {
            return Operand.Error('Function \'{0}\' parameter is error!', 'Atanh');
        }
        return Operand.Create(Math.atanh(x));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Atanh');
    }
}

export { Function_ATANH };
