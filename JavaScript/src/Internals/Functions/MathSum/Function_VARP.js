import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_VARP extends Function_N {
    get Name() {
        return "VarP";
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
        if (list.length == 1) {
            return Operand.Create(0);
        }

        let mean = 0, m2 = 0;
        for (let i = 0; i < list.length; i++) {
            let delta = list[i] - mean;
            mean += delta / (i + 1);
            m2 += delta * (list[i] - mean);
        }
        return Operand.Create(m2 / list.length);
    }
}

export { Function_VARP };
