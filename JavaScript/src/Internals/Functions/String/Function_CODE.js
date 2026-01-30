import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_CODE extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error2, 'CODE');
            if (args1.IsError) {
                return args1;
            }
        }
        if (!args1.TextValue) {
            return Operand.Error(StringCache.Function_parameter_error2, 'CODE');
        }
        let c = args1.TextValue[0];
        return Operand.Create(c.charCodeAt(0));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'CODE');
    }
}

export { Function_CODE };
