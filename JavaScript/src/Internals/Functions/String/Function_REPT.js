import { Function_2 } from '../Function_2';
import { Operand } from '../../../Operand.js';

class Function_REPT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function {0} parameter {1} is error!', 'Rept', 1);
            if (args1.isError) {
                return args1;
            }
        }

        const args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            args2.toNumber('Function {0} parameter {1} is error!', 'Rept', 2);
            if (args2.isError) {
                return args2;
            }
        }

        const newtext = args1.textValue;
        const length = args2.intValue;
        if (length < 0) {
            return Operand.error('Function {0} parameter {1} is error!', 'Rept', 2);
        }
        if (length === 0) {
            return Operand.create('');
        }
        let result = '';
        for (let i = 0; i < length; i++) {
            result += newtext;
        }
        return Operand.create(result);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Rept');
    }
}

export { Function_REPT };
