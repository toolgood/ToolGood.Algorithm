import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_HEX2DEC extends Function_2 {
    get Name() {
        return "Hex2Dec";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        let text = args1.TextValue;
        if (!/^[0-9A-Fa-f]+$/.test(text)) { return this.parameterError(1); }
        if (text.length > 10) { return this.parameterError(1); }
        // 10 位十六进制补码解析
        let num = parseInt(text, 16);
        if (num >= 0x8000000000) { num -= 0x10000000000; }
        if (this.b != null) {
            let args2 = this.getNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            if (args2.IntValue < 0) { return this.parameterError(2); }
            let n = num.toString();
            if (n.length > args2.IntValue) { return this.parameterError(2); }
            if (num < 0) {
                n = "-" + n.substring(1).padStart(args2.IntValue - 1, '0');
            } else {
                n = n.padStart(args2.IntValue, '0');
            }
            return Operand.Create(n);
        }
        return Operand.Create(num);
    }
}

export { Function_HEX2DEC };

