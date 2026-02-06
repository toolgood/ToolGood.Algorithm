import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_HEX2BIN extends Function_2 {
    get Name() {
        return "Hex2Bin";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        if (!/^[0-9A-Fa-f]+$/.test(args1.TextValue)) { return this.ParameterError(1); }
        let num = parseInt(args1.TextValue, 16).toString(2);
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

export { Function_HEX2BIN };

