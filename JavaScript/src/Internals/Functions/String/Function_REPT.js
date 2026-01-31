import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_REPT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'Rept', 1);
            if (args1.IsError) {
                return args1;
            }
        }

        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'Rept', 2);
            if (args2.IsError) {
                return args2;
            }
        }

        let newtext = args1.TextValue;
        let length = args2.IntValue;
        if (length < 0) {
            return Operand.Error(StringCache.Function_parameter_error, 'Rept', 2);
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
}

export { Function_REPT };

