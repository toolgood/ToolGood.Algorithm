import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_CSC extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber('Function \'{0}\' parameter is error!', 'Csc');
            if (args1.IsError) {
                return args1;
            }
        }
        let d = Math.sin(args1.DoubleValue);
        if (d === 0) {
            return Operand.Error('Function \'{0}\') div 0 error!', 'Csc');
        }
        return Operand.Create(1.0 / d);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Csc');
    }
}

export { Function_CSC };
