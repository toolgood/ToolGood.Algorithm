import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_T extends Function_1 {
    get Name() {
        return "T";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.a.evaluate(work, tempParameter);
        if (args1.IsText) {
            return args1;
        }
        return Operand.Create('');
    }
}

export { Function_T };

