import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_OCT2DEC extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'OCT2DEC');
            if (args1.IsError) {
                return args1;
            }
        }

        if (!RegexHelper.OctRegex.test(args1.TextValue)) {
            return Operand.Error(StringCache.Function_parameter_error, 'OCT2DEC');
        }
        let num = parseInt(args1.TextValue, 8);
        return Operand.Create(num);
    }
}

let RegexHelper = {
    OctRegex: /^[0-7]+$/
};

export { Function_OCT2DEC };

