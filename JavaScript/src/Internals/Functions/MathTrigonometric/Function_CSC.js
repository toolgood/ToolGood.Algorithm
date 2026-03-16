import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_CSC extends Function_1 {
    get Name() {
        return "Csc";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let d = Math.sin(args1.DoubleValue);
        if (d === 0) {
            return this.div0Error();
        }
        return Operand.Create(1.0 / d);
    }
}

export { Function_CSC };

