import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_SUMSQ extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let i = 0; i < this.funcs.length; i++) {
            let aa = this.funcs[i].Evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error(StringCache.Function_parameter_error, "SumSQ");
        }

        let d = 0;
        for (let i = 0; i < list.length; i++) {
            let a = list[i];
            d += a * a;
        }
        return Operand.Create(d);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "SumSQ");
    }
}

export { Function_SUMSQ };