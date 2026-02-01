import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_CLEAN extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'Clean');
            if (args1.IsError) {
                return args1;
            }
        }
        let t = args1.TextValue;
        let needClean = false;
        for (let i = 0; i < t.length; i++) {
            let c = t[i];
            if (c === '\f' || c === '\n' || c === '\r' || c === '\t' || c === '\v') {
                needClean = true;
                break;
            }
        }
        if (!needClean) {
            return args1;
        }
        let result = '';
        for (let i = 0; i < t.length; i++) {
            let c = t[i];
            if (c !== '\f' && c !== '\n' && c !== '\r' && c !== '\t' && c !== '\v') {
                result += c;
            }
        }
        return Operand.Create(result);
    }
}

export { Function_CLEAN };

