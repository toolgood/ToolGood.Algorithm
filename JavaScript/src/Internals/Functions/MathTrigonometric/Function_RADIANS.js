import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_RADIANS extends Function_1 {
    get Name() {
        return "Radians";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let r = args1.DoubleValue / 180 * Math.PI;
        return Operand.Create(r);
    }
}

export { Function_RADIANS };

