import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_Array extends Function_N {
    get Name() {
        return "Array";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args = [];
        for (let i = 0; i < this.z.length; i++) {
            let item = this.z[i];
            let aa = item.evaluate(work, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }
        let result = Operand.Create(args);
        return result;
    }

}

export { Function_Array };
