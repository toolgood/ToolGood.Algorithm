import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_BIN2OCT extends Function_2 {
    get Name() {
        return "Bin2Oct";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        if (!/^[01]+$/.test(args1.TextValue)) { return this.parameterError(1); }
        let num = parseInt(args1.TextValue, 2).toString(8);
        if (this.b != null) {
            let args2 = this.getNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            if (num.length > args2.IntValue) {
                return Operand.Create(num.padStart(args2.IntValue, '0'));
            }
            return this.parameterError(2);
        }
        return Operand.Create(num);
    }
}

export { Function_BIN2OCT };

