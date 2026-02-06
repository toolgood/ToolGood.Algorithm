import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_CHAR extends Function_1 {
    get Name() {
        return "Char";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let c = String.fromCharCode(args1.IntValue);
        return Operand.Create(c);
    }
}

export { Function_CHAR };

