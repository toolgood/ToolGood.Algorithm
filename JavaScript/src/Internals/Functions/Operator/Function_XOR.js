import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_XOR extends Function_N {
    get Name() {
        return "Xor";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let trueCount = 0;
        for (let i = 0; i < this.z.length; i++) {
            let a = this.getBoolean(engine, tempParameter, i);
            if (a.IsError) { return a; }
            if (a.BooleanValue) trueCount++;
        }
        return (trueCount % 2 == 1) ? Operand.True : Operand.False;
    }
}

export { Function_XOR };