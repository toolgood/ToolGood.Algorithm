import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_MAX extends Function_N {
    get Name() {
        return "Max";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return this.FunctionError();
        }

        return Operand.Create(Math.max(...list));
    }
}

export { Function_MAX };

