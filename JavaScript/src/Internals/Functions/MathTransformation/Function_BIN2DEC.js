import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_BIN2DEC extends Function_2 {
    get Name() {
        return "Bin2Dec";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        if (!/^[01]+$/.test(args1.TextValue)) { return this.parameterError(1); }
        let num = parseInt(args1.TextValue, 2);
        if (this.b != null) {
            let args2 = this.getNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            if (args2.IntValue < 0) {
                return this.parameterError(2);
            }
            let n = num.toString();
            if (n.length <= args2.IntValue) {
                return Operand.Create(n.padStart(args2.IntValue, '0'));
            }
            return this.parameterError(2);
        }
        return Operand.Create(num);
    }
}

export { Function_BIN2DEC };