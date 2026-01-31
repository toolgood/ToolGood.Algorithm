import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_HARMEAN extends Function_N {
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

        if (args.length === 1) {
            return args[0];
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'HarMean');
        }

        let sum = 0;
        for (let db of list) {
            if (db === 0) {
                return Operand.Error(StringCache.Function_parameter_1_error, 'HarMean');
            }
            sum += 1 / db;
        }
        if (sum === 0) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'HarMean');
        }
        return Operand.Create(list.length / sum);
    }
}

export { Function_HARMEAN };

