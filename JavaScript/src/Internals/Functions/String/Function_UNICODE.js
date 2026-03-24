import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_UNICODE extends Function_1 {
    get Name() {
        return "Unicode";
    }

    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        if (args1.TextValue.length == 0) {
            return this.parameterError(1);
        }
        return Operand.Create(args1.TextValue.codePointAt(0));
    }
}

export { Function_UNICODE };