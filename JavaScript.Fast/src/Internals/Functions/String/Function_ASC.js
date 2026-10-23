import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ASC extends Function_1 {
    get Name() {
        return "ASC";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(this.F_base_ToDBC(args1.TextValue));
    }

    F_base_ToDBC(input) {
        let needModify = false;
        for (let i = 0; i < input.length; i++) {
            let c = input.charCodeAt(i);
            if (c === 12288 || (c > 65280 && c < 65375)) {
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
            if (c === 12288) {
                chars.push(String.fromCharCode(32));
            } else if (c > 65280 && c < 65375) {
                chars.push(String.fromCharCode(c - 65248));
            } else {
                chars.push(input[i]);
            }
        }
        return chars.join('');
    }
}

export { Function_ASC };

