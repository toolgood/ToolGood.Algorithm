import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_SUM extends Function_N {
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

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error(StringCache.Function_parameter_error2, "Sum");
        }
        return Operand.Create(list.reduce((sum, val) => sum + val, 0));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Sum");
    }
}

export { Function_SUM };