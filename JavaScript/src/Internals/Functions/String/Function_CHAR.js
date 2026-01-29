import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_CHAR extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function \'{0}\' parameter is error!', 'Char');
            if (args1.IsError) {
                return args1;
            }
        }
        const c = String.fromCharCode(args1.IntValue);
        return Operand.Create(c);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Char');
    }
}

export { Function_CHAR };
