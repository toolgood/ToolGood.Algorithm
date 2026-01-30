import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_Array extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let i = 0; i < this.funcs.length; i++) {
            let item = this.funcs[i];
            let aa = item.Evaluate(engine, tempParameter);
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
