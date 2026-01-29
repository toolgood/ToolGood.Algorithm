import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_CLEAN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotText) {
            args1.ToText('Function \'{0}\' parameter is error!', 'Clean');
            if (args1.isError) {
                return args1;
            }
        }
        const t = args1.textValue;
        let needClean = false;
        for (let i = 0; i < t.length; i++) {
            const c = t[i];
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
            const c = t[i];
            if (c !== '\f' && c !== '\n' && c !== '\r' && c !== '\t' && c !== '\v') {
                result += c;
            }
        }
        return Operand.Create(result);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Clean');
    }
}

export { Function_CLEAN };
