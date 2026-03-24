import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_UNICHAR extends Function_1 {
    get Name() {
        return "UniChar";
    }

    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let code = args1.IntValue;
        if (code < 0 || code > 0x10FFFF || (code >= 0xD800 && code <= 0xDFFF)) {
            return this.parameterError(1);
        }
        try {
            return Operand.Create(String.fromCodePoint(code));
        } catch (e) {
            return this.parameterError(1);
        }
    }
}

export { Function_UNICHAR };