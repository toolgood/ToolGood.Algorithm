import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_HEX2DEC extends Function_1 {
    get Name() {
        return "Hex2Dec";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        if (!/^[0-9A-Fa-f]+$/.test(args1.TextValue)) { return this.FunctionError(); }
        let num = parseInt(args1.TextValue, 16);
        return Operand.Create(num);
    }
}

export { Function_HEX2DEC };

