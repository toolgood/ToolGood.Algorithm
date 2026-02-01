import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_DEVSQ extends Function_N {
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
        if (!o) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'DevSQ');
        }
        if (list.length === 0) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'DevSQ');
        }
        let avg = list.reduce((sum, val) => sum + val, 0) / list.length;
        let sum = 0;
        for (let i = 0; i < list.length; i++) {
            let diff = list[i] - avg;
            sum += diff * diff;
        }
        return Operand.Create(sum);
    }
}

export { Function_DEVSQ };

