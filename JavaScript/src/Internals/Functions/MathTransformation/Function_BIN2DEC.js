import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_BIN2DEC extends Function_2 {
    get Name() {
        return "Bin2Dec";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        if (!/^[01]+$/.test(args1.TextValue)) { return this.functionError(); }
        let num = parseInt(args1.TextValue, 2);
        return Operand.Create(num);
    }
}

export { Function_BIN2DEC };

