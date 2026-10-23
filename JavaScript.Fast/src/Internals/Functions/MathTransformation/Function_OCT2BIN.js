import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_OCT2BIN extends Function_2 {
    get Name() {
        return "Oct2Bin";
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
        // Excel OCT2BIN 结果范围为 -512~511
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
            if (args2.IntValue < 0 || args2.IntValue > 10) { return this.parameterError(2); }
            if (bin.length > args2.IntValue) { return this.parameterError(2); }
            return Operand.Create(bin.padStart(args2.IntValue, '0'));
        }
        return Operand.Create(bin);
    }
}

export { Function_OCT2BIN };

