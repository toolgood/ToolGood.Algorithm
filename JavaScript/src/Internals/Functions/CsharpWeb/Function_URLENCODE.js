import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_URLENCODE extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, "UrlEncode");
            if (args1.IsError) {
                return args1;
            }
        }
        let s = args1.TextValue;
        let r = encodeURIComponent(s)
            .replace(/%20/g, '+')
            .toLowerCase();
        return Operand.Create(r);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "UrlEncode");
    }
}

export { Function_URLENCODE };
