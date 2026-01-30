import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_BIN2DEC extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'BIN2DEC');
            if (args1.IsError) {
                return args1;
            }
        }

        if (!RegexHelper.BinRegex.test(args1.TextValue)) {
            return Operand.Error(StringCache.Function_parameter_error, 'BIN2DEC');
        }
        let num = parseInt(args1.TextValue, 2);
        return Operand.Create(num);
    }
}

let RegexHelper = {
    BinRegex: /^[01]+$/
};

export { Function_BIN2DEC };

