import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_BIN2OCT extends Function_2 {
    get Name() {
        return "Bin2Oct";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        if (!/^[01]+$/.test(args1.TextValue)) { return this.parameterError(1); }
        let text = args1.TextValue;
        if (text.length > 10) { return this.parameterError(1); }
        // 10 位二进制补码解析
        let num = parseInt(text, 2);
        if (num >= 512) { num -= 1024; }
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
            if (args2.IntValue < 0) { return this.parameterError(2); }
            if (oct.length > args2.IntValue) { return this.parameterError(2); }
            return Operand.Create(oct.padStart(args2.IntValue, '0'));
        }
        return Operand.Create(oct);
    }
}

export { Function_BIN2OCT };

