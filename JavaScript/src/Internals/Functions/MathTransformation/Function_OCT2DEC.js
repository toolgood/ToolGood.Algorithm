import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_OCT2DEC extends Function_1 {
    get Name() {
        return "Oct2Dec";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        if (!/^[0-7]+$/.test(args1.TextValue)) { return this.functionError(); }
        let num = parseInt(args1.TextValue, 8);
        return Operand.Create(num);
    }
}

export { Function_OCT2DEC };

