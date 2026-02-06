import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_JIS extends Function_1 {
    get Name() {
        return "JIS";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(this.F_base_ToSBC(args1.TextValue));
    }

    F_base_ToSBC(input) {
        let needModify = false;
        for (let i = 0; i < input.length; i++) {
            let c = input.charCodeAt(i);
            if (c === 32 || c < 127) {
                needModify = true;
                break;
            }
        }
        if (!needModify) {
            return input;
        }
        let chars = [];
        for (let i = 0; i < input.length; i++) {
            let c = input.charCodeAt(i);
            if (c === 32) {
                chars.push(String.fromCharCode(12288));
            } else if (c < 127) {
                chars.push(String.fromCharCode(c + 65248));
            } else {
                chars.push(input[i]);
            }
        }
        return chars.join('');
    }
}

export { Function_JIS };

