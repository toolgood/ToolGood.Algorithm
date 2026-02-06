import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_FACT extends Function_1 {
    get Name() {
        return "Fact";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let z = args1.IntValue;
        if (z < 0) {
            return this.FunctionError();
        }
        let d = 1;
        for (let i = 1; i <= z; i++) {
            d *= i;
        }
        return Operand.Create(d);
    }
}

export { Function_FACT };

