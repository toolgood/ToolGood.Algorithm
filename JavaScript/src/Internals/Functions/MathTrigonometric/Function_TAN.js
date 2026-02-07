import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_TAN extends Function_1 {
    get Name() {
        return "Tan";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(Math.tan(args1.DoubleValue));
    }
}

export { Function_TAN };

