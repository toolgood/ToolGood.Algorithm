import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_DELTA extends Function_2 {
    get Name() {
        return "Delta";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let num1 = args1.NumberValue;

        let num2 = 0;
        if (this.b != null) {
            let args2 = this.getNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }
            num2 = args2.NumberValue;
        }

        return Operand.Create(num1 == num2 ? 1 : 0);
    }
}

export { Function_DELTA };