import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_SEC extends Function_1 {
    get Name() {
        return "Sec";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let d = Math.cos(args1.DoubleValue);
        if (d === 0) {
            return this.Div0Error();
        }
        return Operand.Create(1.0 / d);
    }
}

export { Function_SEC };

