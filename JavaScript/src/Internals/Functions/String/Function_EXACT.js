import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_EXACT extends Function_2 {
    get Name() {
        return "EXACT";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.getText_2(work, tempParameter);
        if (args2.IsError) { return args2; }
        return Operand.Create(args1.TextValue === args2.TextValue);
    }
}

export { Function_EXACT };

