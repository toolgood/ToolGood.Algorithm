import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_TRIM extends Function_1 {
    get Name() {
        return "Trim";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(args1.TextValue.trim());
    }
}

export { Function_TRIM };

