import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_MEDIAN extends Function_N {
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

        if (!o) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'Median');
        }
        if (list.length === 0) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'Median');
        }

        list.sort((a, b) => a - b); // 升序排序
        return Operand.Create(list[Math.floor(list.length / 2)]);
    }
}

export { Function_MEDIAN };

