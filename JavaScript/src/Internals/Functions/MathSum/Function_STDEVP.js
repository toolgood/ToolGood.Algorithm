import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_STDEVP extends Function_N {
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
            return Operand.Error(StringCache.Function_parameter_error, "StdevP");
        }
        if (list.length == 0) {
            return Operand.Error(StringCache.Function_parameter_error, "StdevP");
        }

        let avg = list.reduce((sum, val) => sum + val, 0) / list.length;
        let sum = 0;
        for (let i = 0; i < list.length; i++) {
            sum += (list[i] - avg) * (list[i] - avg);
        }
        return Operand.Create(Math.sqrt(sum / list.length));
    }
}

export { Function_STDEVP };
