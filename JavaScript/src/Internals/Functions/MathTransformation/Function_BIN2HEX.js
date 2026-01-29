import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_BIN2HEX extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter {1} is error!', 'BIN2HEX', 1);
            if (args1.isError) {
                return args1;
            }
        }

        if (!RegexHelper.BinRegex.test(args1.textValue)) {
            return Operand.error('Function \'{0}\' parameter {1} is error!', 'BIN2HEX', 1);
        }
        const num = parseInt(args1.textValue, 2).toString(16).toUpperCase();
        if (this.func2 !== null) {
            const args2 = this.func2.Evaluate(work, tempParameter);
            if (args2.isNotNumber) {
                args2.toNumber('Function \'{0}\' parameter {1} is error!', 'BIN2HEX', 2);
                if (args2.isError) {
                    return args2;
                }
            }
            if (num.length > args2.intValue) {
                return Operand.create(num.padLeft(args2.intValue, '0'));
            }
            return Operand.error('Function \'{0}\' parameter {1} is error!', 'BIN2HEX', 2);
        }
        return Operand.create(num);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'BIN2HEX');
    }
}

const RegexHelper = {
    BinRegex: /^[01]+$/
};

export { Function_BIN2HEX };
