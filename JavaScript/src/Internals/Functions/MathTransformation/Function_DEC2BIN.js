import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_DEC2BIN extends Function_2 {
    get Name() {
        return "Dec2Bin";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let num = args1.IntValue.toString(2);
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

export { Function_DEC2BIN };

