import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_HEX2BIN extends Function_2 {
    get Name() {
        return "Hex2Bin";
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
        // Excel HEX2BIN 结果范围为 -512~511
        if (num < -512 || num > 511) { return this.parameterError(1); }
        let bin;
        if (num < 0) {
            // 负数:10 位二进制补码
            bin = (num & 1023).toString(2).padStart(10, '0');
        } else {
            bin = num.toString(2);
        }
        if (this.b != null) {
            let args2 = this.getNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            if (args2.IntValue < 0) { return this.parameterError(2); }
            if (bin.length > args2.IntValue) { return this.parameterError(2); }
            return Operand.Create(bin.padStart(args2.IntValue, '0'));
        }
        return Operand.Create(bin);
    }
}

export { Function_HEX2BIN };

