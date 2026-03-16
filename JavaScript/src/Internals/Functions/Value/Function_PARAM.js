import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_PARAM extends Function_2 {
    get Name() {
        return "Param";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        if (tempParameter !== null) {
            let r = tempParameter(work, args1.TextValue);
            if (r !== null) {
                return r;
            }
        }
        let result = work.GetParameter(args1.TextValue);
        if (result.IsError) {
            if (this.b !== null) {
                return this.b.evaluate(work, tempParameter);
            }
        }
        return result;
    }
 
}

export { Function_PARAM };
