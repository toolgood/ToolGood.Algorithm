import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_BIN2HEX extends Function_2 {
    get Name() {
        return "Bin2Hex";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        if (!/^[01]+$/.test(args1.TextValue)) { return this.ParameterError(1); }
        let num = parseInt(args1.TextValue, 2).toString(16).toUpperCase();
        if (this.b != null) {
            let args2 = this.GetNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            if (num.length > args2.IntValue) {
                return Operand.Create(num.padStart(args2.IntValue, '0'));
            }
            return this.ParameterError(2);
        }
        return Operand.Create(num);
    }
}

export { Function_BIN2HEX };

