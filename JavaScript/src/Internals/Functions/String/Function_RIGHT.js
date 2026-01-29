import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_RIGHT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1.ToText('Function {0} parameter {1} is error!', 'Right', 1);
            if (args1.IsError) {
                return args1;
            }
        }

        if (args1.TextValue.length === 0) {
            return Operand.Create('');
        }
        if (this.func2 === null) {
            return Operand.Create(args1.TextValue.substring(args1.TextValue.length - 1, args1.TextValue.length));
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2.ToNumber('Function {0} parameter {1} is error!', 'Right', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const length = Math.min(args2.IntValue, args1.TextValue.length);
        const start = args1.TextValue.length - length;
        return Operand.Create(args1.TextValue.substring(start, start + length));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Right');
    }
}

export { Function_RIGHT };
