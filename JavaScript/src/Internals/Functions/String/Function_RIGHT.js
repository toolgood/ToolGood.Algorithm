import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_RIGHT extends Function_2 {
    get Name() {
        return "Right";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        if (args1.TextValue.length === 0) {
            return Operand.Create('');
        }
        if (this.b === null) {
            return Operand.Create(args1.TextValue.substring(args1.TextValue.length - 1, args1.TextValue.length));
        }
        let args2 = this.getNumber_2(work, tempParameter);
        if (args2.IsError) { return args2; }
        // 与 C# 一致:负数长度时报参数错误
        if (args2.IntValue < 0) {
            return this.parameterError(2);
        }
        let length = Math.min(args2.IntValue, args1.TextValue.length);
        let start = args1.TextValue.length - length;
        return Operand.Create(args1.TextValue.substring(start, start + length));
    }
}

export { Function_RIGHT };

