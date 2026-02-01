import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_CODE extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
            args1 = args1.ToText(StringCache.Function_parameter_error, 'CODE');
            if (args1.IsError) {
                return args1;
            }
        if (!args1.TextValue) {
            return Operand.Error(StringCache.Function_parameter_error, 'CODE');
        }
        let c = args1.TextValue[0];
        return Operand.Create(c.charCodeAt(0));
    }
}

export { Function_CODE };

