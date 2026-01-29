import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_SEC extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function \'{0}\' parameter is error!', 'Sec');
            if (args1.IsError) {
                return args1;
            }
        }
        let d = Math.cos(args1.DoubleValue);
        if (d === 0) {
            return Operand.error('Function \'{0}\') div 0 error!', 'Sec');
        }
        return Operand.Create(1.0 / d);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Sec');
    }
}

export { Function_SEC };
