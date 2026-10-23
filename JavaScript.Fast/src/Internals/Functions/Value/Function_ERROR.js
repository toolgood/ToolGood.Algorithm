import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ERROR extends Function_1 {
    get Name() {
        return "Error";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Error(args1.TextValue);
    }

}

export { Function_ERROR };
