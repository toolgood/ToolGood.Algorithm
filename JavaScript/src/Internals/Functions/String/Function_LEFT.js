import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_LEFT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1.ToText('Function {0} parameter {1} is error!', 'Left', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        if (args1.TextValue.length === 0) {
            return Operand.Create('');
        }
        if (this.func2 === null) {
            return Operand.Create(args1.TextValue.substring(0, 1));
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2.ToNumber('Function {0} parameter {1} is error!', 'Left', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const length = Math.min(args2.IntValue, args1.TextValue.length);
        return Operand.Create(args1.TextValue.substring(0, length));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Left');
    }
}

export { Function_LEFT };
