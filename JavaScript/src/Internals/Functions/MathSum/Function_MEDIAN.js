import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_MEDIAN extends Function_N {
    get Name() {
        return "Median";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);

        if (o == false) {
            return this.functionError();
        }
        if (list.length == 0) {
            return this.functionError();
        }

        list.sort((a, b) => a - b);
        return Operand.Create(list[Math.floor(list.length / 2)]);
    }
}

export { Function_MEDIAN };

