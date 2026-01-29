import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_ATAN2 extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function \'{0}\' parameter {1} is error!', 'Atan2', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.isNotNumber) {
            args2.toNumber('Function \'{0}\' parameter {1} is error!', 'Atan2', 2);
            if (args2.isError) {
                return args2;
            }
        }
        return Operand.create(Math.atan2(args2.doubleValue, args1.doubleValue));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Atan2');
    }
}

export { Function_ATAN2 };
