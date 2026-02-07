import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_FACTDOUBLE extends Function_1 {
    get Name() {
        return "FactDouble";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let z = args1.IntValue;
        if (z < 0) { return this.functionError(); }

        let d = 1;
        for (let i = z; i > 0; i -= 2) {
            d *= i;
        }
        return Operand.Create(d);
    }
}

export { Function_FACTDOUBLE };

