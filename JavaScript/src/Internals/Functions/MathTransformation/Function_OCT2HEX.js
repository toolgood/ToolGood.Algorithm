import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_OCT2HEX extends Function_2 {
    get Name() {
        return "Oct2Hex";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        if (!/^[0-7]+$/.test(args1.TextValue)) { return this.parameterError(1); }
        let text = args1.TextValue;
        if (text.length > 10) { return this.parameterError(1); }
        // 10 位八进制补码解析
        let num = parseInt(text, 8);
        if (num >= 536870912) { num -= 1073741824; }
        let hex;
        if (num < 0) {
            // 负数:10 位十六进制补码
            hex = (num + 0x10000000000).toString(16).toUpperCase().padStart(10, '0');
        } else {
            hex = num.toString(16).toUpperCase();
        }
        if (this.b != null) {
            let args2 = this.getNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            if (args2.IntValue < 0 || args2.IntValue > 10) { return this.parameterError(2); }
            if (hex.length > args2.IntValue) { return this.parameterError(2); }
            return Operand.Create(hex.padStart(args2.IntValue, '0'));
        }
        return Operand.Create(hex);
    }
}

export { Function_OCT2HEX };

