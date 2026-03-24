import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_SECH extends Function_1 {
    get Name() {
        return "Sech";
    }

    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(1.0 / Math.cosh(args1.NumberValue));
    }
}

export { Function_SECH };