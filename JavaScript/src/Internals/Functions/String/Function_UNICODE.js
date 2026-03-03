import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_UNICODE extends Function_1 {
    get Name() {
        return "Unicode";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        if (!args1.TextValue || args1.TextValue.length === 0) {
            return Operand.Error(this.functionError, 'UNICODE');
        }
        return Operand.Create(args1.TextValue.codePointAt(0));
    }
}

export { Function_UNICODE };
