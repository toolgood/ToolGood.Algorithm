import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_DEC2BIN extends Function_2 {
    get Name() {
        return "Dec2Bin";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let num = Math.trunc(args1.NumberValue);
        // Excel DEC2BIN 范围为 -512~511
        if (num < -512 || num > 511) { return this.parameterError(1); }
        if (num < 0) {
            // 负数:10 位二进制补码
            return Operand.Create((num & 1023).toString(2).padStart(10, '0'));
        }
        let bin = num.toString(2);
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

export { Function_DEC2BIN };

