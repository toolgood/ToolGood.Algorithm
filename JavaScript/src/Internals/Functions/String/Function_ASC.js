import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ASC extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            args1.ToText('Function \'{0}\' parameter is error!', 'ASC');
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.Create(this.F_base_ToDBC(args1.TextValue));
    }

    F_base_ToDBC(input) {
        let needModify = false;
        for (let i = 0; i < input.length; i++) {
            const c = input.charCodeAt(i);
            if (c === 12288 || (c > 65280 && c < 65375)) {
                needModify = true;
                break;
            }
        }
        if (!needModify) {
            return input;
        }
        const chars = [];
        for (let i = 0; i < input.length; i++) {
            const c = input.charCodeAt(i);
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

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'ASC');
    }
}

export { Function_ASC };
