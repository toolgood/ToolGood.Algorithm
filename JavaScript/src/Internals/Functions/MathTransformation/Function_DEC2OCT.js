import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_DEC2OCT extends Function_2 {
    get Name() {
        return "Dec2Oct";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let num = Math.trunc(args1.NumberValue);
        // Excel DEC2OCT 范围为 -536870912~536870911
        if (num < -536870912 || num > 536870911) { return this.parameterError(1); }
        if (num < 0) {
            // 负数:10 位八进制补码
            return Operand.Create((num & 0x3FFFFFFF).toString(8).padStart(10, '0'));
        }
        let oct = num.toString(8);
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

export { Function_DEC2OCT };

