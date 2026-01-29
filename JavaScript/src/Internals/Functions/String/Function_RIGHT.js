import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_RIGHT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function {0} parameter {1} is error!', 'Right', 1);
            if (args1.isError) {
                return args1;
            }
        }

        if (args1.textValue.length === 0) {
            return Operand.Create('');
        }
        if (this.func2 === null) {
            return Operand.Create(args1.textValue.substring(args1.textValue.length - 1, args1.textValue.length));
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            args2.toNumber('Function {0} parameter {1} is error!', 'Right', 2);
            if (args2.isError) {
                return args2;
            }
        }
        const length = Math.min(args2.intValue, args1.textValue.length);
        const start = args1.textValue.length - length;
        return Operand.Create(args1.textValue.substring(start, start + length));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Right');
    }
}

export { Function_RIGHT };
