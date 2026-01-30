import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_BIN2OCT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'BIN2OCT', 1);
            if (args1.IsError) {
                return args1;
            }
        }

        if (!RegexHelper.BinRegex.test(args1.TextValue)) {
            return Operand.Error(StringCache.Function_parameter_error, 'BIN2OCT', 1);
        }
        let num = parseInt(args1.TextValue, 2).toString(8);
        if (this.func2 !== null) {
            let args2 = this.func2.Evaluate(work, tempParameter);
            if (args2.IsNotNumber) {
                args2 = args2.ToNumber(StringCache.Function_parameter_error, 'BIN2OCT', 2);
                if (args2.IsError) {
                    return args2;
                }
            }
            if (num.length > args2.IntValue) {
                return Operand.Error(StringCache.Function_parameter_error, 'BIN2OCT', 2);
            }
            return Operand.Create(num.padLeft(args2.IntValue, '0'));
        }
        return Operand.Create(num);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'BIN2OCT');
    }
}

let RegexHelper = {
    BinRegex: /^[01]+$/
};

export { Function_BIN2OCT };
