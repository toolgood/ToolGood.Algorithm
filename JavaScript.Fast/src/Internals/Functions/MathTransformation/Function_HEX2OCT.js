import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_HEX2OCT extends Function_2 {
    get Name() {
        return "Hex2Oct";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        if (!/^[0-9A-Fa-f]+$/.test(args1.TextValue)) { return this.parameterError(1); }
        let text = args1.TextValue;
        if (text.length > 10) { return this.parameterError(1); }
        // 10 位十六进制补码解析
        let num = parseInt(text, 16);
        if (num >= 0x8000000000) { num -= 0x10000000000; }
        // Excel HEX2OCT 结果范围为 -536870912~536870911
        if (num < -536870912 || num > 536870911) { return this.parameterError(1); }
        let oct;
        if (num < 0) {
            // 负数:10 位八进制补码
            oct = (num & 0x3FFFFFFF).toString(8).padStart(10, '0');
        } else {
            oct = num.toString(8);
        }
        if (this.b != null) {
            let args2 = this.getNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            if (args2.IntValue < 0 || args2.IntValue > 10) { return this.parameterError(2); }
            if (oct.length > args2.IntValue) { return this.parameterError(2); }
            return Operand.Create(oct.padStart(args2.IntValue, '0'));
        }
        return Operand.Create(oct);
    }
}

export { Function_HEX2OCT };

