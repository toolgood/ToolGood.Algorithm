import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_CONCATENATE extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(work, tempParameter) {
        if (this.funcs.length === 0) {
            return Operand.Create('');
        }
        if (this.funcs.length === 1) {
            let a = this.funcs[0].Evaluate(work, tempParameter);
            if (a.IsNotText) {
                a.ToText(StringCache.Function_parameter_error2, 'Concatenate', 1);
                if (a.IsError) {
                    return a;
                }
            }
            return a;
        }
        let result = '';
        for (let i = 0; i < this.funcs.length; i++) {
            let a = this.funcs[i].Evaluate(work, tempParameter);
            if (a.IsNotText) {
                a.ToText(StringCache.Function_parameter_error2, 'Concatenate', i + 1);
                if (a.IsError) {
                    return a;
                }
            }
            result += a.TextValue;
        }
        return Operand.Create(result);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Concatenate');
    }
}

export { Function_CONCATENATE };
