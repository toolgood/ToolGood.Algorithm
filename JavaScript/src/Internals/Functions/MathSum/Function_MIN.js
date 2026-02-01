import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_MIN extends Function_N {
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
            return Operand.Error(StringCache.Function_parameter_1_error, 'Min');
        }

        if (list.length === 0) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'Min');
        }

        return Operand.Create(Math.min(...list));
    }
}

export { Function_MIN };

