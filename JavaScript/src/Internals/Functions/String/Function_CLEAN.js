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
            if (t.charCodeAt(i) < 32) {
                needClean = true;
                break;
            }
        }
        if (!needClean) {
            return args1;
        }
        let result = '';
        for (let i = 0; i < t.length; i++) {
            if (t.charCodeAt(i) >= 32) {
                result += t[i];
            }
        }
        return Operand.Create(result);
    }
}

export { Function_CLEAN };

