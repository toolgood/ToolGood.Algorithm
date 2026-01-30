import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_RIGHT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'Right', 1);
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
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'Right', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let length = Math.min(args2.IntValue, args1.TextValue.length);
        let start = args1.TextValue.length - length;
        return Operand.Create(args1.TextValue.substring(start, start + length));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Right');
    }
}

export { Function_RIGHT };
