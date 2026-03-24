import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_LOG10 extends Function_1 {
    get Name() {
        return "Log10";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let z = args1.NumberValue;
        if (z <= 0) {
            return this.parameterError(1);
        }
        return Operand.Create(Math.log10(z));
    }
}

export { Function_LOG10 };