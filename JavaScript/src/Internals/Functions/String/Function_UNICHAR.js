import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_UNICHAR extends Function_1 {
    get Name() {
        return "UniChar";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        try {
            return Operand.Create(String.fromCodePoint(args1.IntValue));
        } catch (e) {
            return Operand.Error(this.functionError, 'UNICHAR');
        }
    }
}

export { Function_UNICHAR };
