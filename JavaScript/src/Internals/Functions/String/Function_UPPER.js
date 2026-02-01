import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_UPPER extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_1_error, 'Upper');
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.Create(args1.TextValue.toUpperCase());
    }
}

export { Function_UPPER };

