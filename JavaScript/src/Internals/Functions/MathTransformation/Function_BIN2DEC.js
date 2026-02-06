import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_BIN2DEC extends Function_1 {
    get Name() {
        return "Bin2Dec";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        if (!/^[01]+$/.test(args1.TextValue)) { return this.FunctionError(); }
        let num = parseInt(args1.TextValue, 2);
        return Operand.Create(num);
    }
}

export { Function_BIN2DEC };

