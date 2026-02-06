import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_LOWER extends Function_1 {
    get Name() {
        return "Lower";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(args1.TextValue.toLowerCase());
    }
}

export { Function_LOWER };

