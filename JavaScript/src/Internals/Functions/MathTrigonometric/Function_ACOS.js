import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ACOS extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function \'{0}\' parameter is error!', 'Acos');
            if (args1.IsError) {
                return args1;
            }
        }
        let x = args1.DoubleValue;
        if (x < -1 || x > 1) {
            return Operand.Error('Function \'{0}\' parameter is error!', 'Acos');
        }
        return Operand.Create(Math.acos(x));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Acos');
    }
}

export { Function_ACOS };
