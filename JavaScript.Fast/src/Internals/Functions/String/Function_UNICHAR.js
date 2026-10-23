import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_UNICHAR extends Function_1 {
    get Name() {
        return "UniChar";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let code = args1.IntValue;
        // 与 C# 一致:越界(含代理区 0xD800~0xDFFF)时参数错误
        if (code < 0 || code > 0x10FFFF || (code >= 0xD800 && code <= 0xDFFF)) {
            return this.parameterError(1);
        }
        return Operand.Create(String.fromCodePoint(code));
    }
}

export { Function_UNICHAR };
