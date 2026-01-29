import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_LEFT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function {0} parameter {1} is error!', 'Left', 1);
            if (args1.isError) {
                return args1;
            }
        }
        if (args1.textValue.length === 0) {
            return Operand.create('');
        }
        if (this.func2 === null) {
            return Operand.create(args1.textValue.substring(0, 1));
        }
        const args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            args2.toNumber('Function {0} parameter {1} is error!', 'Left', 2);
            if (args2.isError) {
                return args2;
            }
        }
        const length = Math.min(args2.intValue, args1.textValue.length);
        return Operand.create(args1.textValue.substring(0, length));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Left');
    }
}

export { Function_LEFT };
