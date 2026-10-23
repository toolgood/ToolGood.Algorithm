import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_SUMSQ extends Function_N {
    get Name() {
        return "SumSq";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args = [];
        for (let i = 0; i < this.z.length; i++) {
            let aa = this.z[i].evaluate(engine, tempParameter);
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

        let d = 0;
        for (let i = 0; i < list.length; i++) {
            let a = list[i];
            d += a * a;
        }
        return Operand.Create(d);
    }
}

export { Function_SUMSQ };
