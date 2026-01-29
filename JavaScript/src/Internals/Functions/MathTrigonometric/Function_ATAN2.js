import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_ATAN2 extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function \'{0}\' parameter {1} is error!', 'Atan2', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber) {
            args2.ToNumber('Function \'{0}\' parameter {1} is error!', 'Atan2', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        return Operand.Create(Math.atan2(args2.DoubleValue, args1.DoubleValue));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Atan2');
    }
}

export { Function_ATAN2 };
