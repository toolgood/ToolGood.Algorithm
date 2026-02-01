import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_URLDECODE extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToText(StringCache.Function_parameter_error, "UrlDecode");
            if (args1.IsError) {
                return args1;
            }
        let s = args1.TextValue;
        let r = decodeURIComponent(s)
            .replace(/\+/g, ' ');
        return Operand.Create(r);
    }
}

export { Function_URLDECODE };

