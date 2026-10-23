import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_CHAR extends Function_1 {
    get Name() {
        return "Char";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let code = args1.IntValue;
        // 与 C# 一致:超出 0~65535 时参数错误(JS fromCharCode 越界会静默取模)
        if (code < 0 || code > 65535) {
            return this.parameterError(1);
        }
        let c = String.fromCharCode(code);
        return Operand.Create(c);
    }
}

export { Function_CHAR };

