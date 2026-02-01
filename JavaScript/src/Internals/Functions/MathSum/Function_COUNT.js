import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_COUNT extends Function_N {
    constructor(z) {
        super(z);
    }

    Evaluate(work, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.Evaluate(work, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        // 处理 count(Array(1,2,3,4)) 这种情况
        if (args.length === 1 && args[0].IsArray) {
            return Operand.Create(args[0].ArrayValue.length);
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o === false) {
            return Operand.Error(StringCache.Function_parameter_error, 'Count');
        }
        return Operand.Create(list.length);
    }
}



export { Function_COUNT };

