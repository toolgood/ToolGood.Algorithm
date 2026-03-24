import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_COTH extends Function_1 {
    get Name() {
        return "Coth";
    }

    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let d = Math.sinh(args1.NumberValue);
        if (d == 0) {
            return this.div0Error();
        }
        return Operand.Create(Math.cosh(args1.NumberValue) / d);
    }
}

export { Function_COTH };