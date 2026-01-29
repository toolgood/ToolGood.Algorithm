import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_REPT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1.ToText('Function {0} parameter {1} is error!', 'Rept', 1);
            if (args1.IsError) {
                return args1;
            }
        }

        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2.ToNumber('Function {0} parameter {1} is error!', 'Rept', 2);
            if (args2.IsError) {
                return args2;
            }
        }

        const newtext = args1.TextValue;
        const length = args2.IntValue;
        if (length < 0) {
            return Operand.error('Function {0} parameter {1} is error!', 'Rept', 2);
        }
        if (length === 0) {
            return Operand.Create('');
        }
        let result = '';
        for (let i = 0; i < length; i++) {
            result += newtext;
        }
        return Operand.Create(result);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Rept');
    }
}

export { Function_REPT };
