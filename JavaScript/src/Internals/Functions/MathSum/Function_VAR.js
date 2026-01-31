import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_VAR extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.funcs) {
            let aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        if (args.length == 1) {
            return Operand.Error(StringCache.Function_error, "Var");
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error(StringCache.Function_parameter_error, "Var");
        }
        if (list.length <= 1) {
            return Operand.Error(StringCache.Function_parameter_error, "Var");
        }
        let sum = 0;
        let sum2 = 0;
        for (let i = 0; i < list.length; i++) {
            sum += list[i] * list[i];
            sum2 += list[i];
        }
        return Operand.Create((list.length * sum - sum2 * sum2) / list.length / (list.length - 1));
    }
}

export { Function_VAR };
