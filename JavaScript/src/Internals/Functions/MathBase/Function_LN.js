import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_LN extends Function_1 {
    get Name() {
        return "Ln";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let n = args1.NumberValue;
        if (n <= 0) {
            return this.parameterError(1);
        }
        return Operand.Create(Math.log(n));
    }
}

export { Function_LN };

