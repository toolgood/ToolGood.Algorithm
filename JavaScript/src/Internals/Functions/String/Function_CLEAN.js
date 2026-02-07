import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_CLEAN extends Function_1 {
    get Name() {
        return "Clean";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
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

