import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ACOTH extends Function_1 {
    get Name() {
        return "Acoth";
    }

    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let d = args1.NumberValue;
        if (Math.abs(d) <= 1) {
            return this.parameterError(1);
        }
        return Operand.Create(0.5 * Math.log((d + 1) / (d - 1)));
    }
}

export { Function_ACOTH };