import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_COS extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function \'{0}\' parameter is error!', 'Cos');
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.Create(Math.cos(args1.DoubleValue));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Cos');
    }
}

export { Function_COS };
