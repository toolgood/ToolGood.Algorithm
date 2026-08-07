import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_DEC2HEX extends Function_2 {
    get Name() {
        return "Dec2Hex";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let num = Math.trunc(args1.NumberValue);
        // Excel DEC2HEX 范围为 -549755813888~549755813887
        if (num < -549755813888 || num > 549755813887) { return this.parameterError(1); }
        if (num < 0) {
            // 负数:10 位十六进制补码
            return Operand.Create((num + 0x10000000000).toString(16).toUpperCase().padStart(10, '0'));
        }
        let hex = num.toString(16).toUpperCase();
        if (this.b != null) {
            let args2 = this.getNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            if (args2.IntValue < 0) { return this.parameterError(2); }
            if (hex.length > args2.IntValue) { return this.parameterError(2); }
            return Operand.Create(hex.padStart(args2.IntValue, '0'));
        }
        return Operand.Create(hex);
    }
}

export { Function_DEC2HEX };

