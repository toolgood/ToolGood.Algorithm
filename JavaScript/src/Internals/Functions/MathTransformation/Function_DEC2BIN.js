import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_DEC2BIN extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function \'{0}\' parameter {1} is error!', 'DEC2BIN', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const num = parseInt(args1.intValue).toString(2);
        if (this.func2 !== null) {
            const args2 = this.func2.Evaluate(work, tempParameter);
            if (args2.isNotNumber) {
                args2.toNumber('Function \'{0}\' parameter {1} is error!', 'DEC2BIN', 2);
                if (args2.isError) {
                    return args2;
                }
            }
            if (num.length > args2.intValue) {
                return Operand.Create(num.padLeft(args2.intValue, '0'));
            }
            return Operand.error('Function \'{0}\' parameter {1} is error!', 'DEC2BIN', 2);
        }
        return Operand.Create(num);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'DEC2BIN');
    }
}

export { Function_DEC2BIN };
