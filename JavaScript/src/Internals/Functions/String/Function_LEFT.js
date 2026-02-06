import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_LEFT extends Function_2 {
    get Name() {
        return "Left";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        if (args1.TextValue.length === 0) {
            return Operand.Create('');
        }
        if (this.b === null) {
            return Operand.Create(args1.TextValue.substring(0, 1));
        }
        let args2 = this.GetNumber_2(work, tempParameter);
        if (args2.IsError) { return args2; }
        let length = Math.min(args2.IntValue, args1.TextValue.length);
        return Operand.Create(args1.TextValue.substring(0, length));
    }
}

export { Function_LEFT };

