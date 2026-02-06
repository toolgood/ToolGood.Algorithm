import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_SQRT extends Function_1 {
    get Name() {
        return "Sqrt";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        if (args1.NumberValue < 0) {
            return this.ParameterError(1);
        }
        return Operand.Create(Math.sqrt(args1.NumberValue));
    }
}

export { Function_SQRT };

