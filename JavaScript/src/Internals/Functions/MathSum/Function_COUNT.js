import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_COUNT extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(work, tempParameter) {
        let args = [];
        for (let item of this.funcs) {
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
            return Operand.Error("Function {0} parameter is error!", 'Count');
        }
        return Operand.Create(list.length);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Count');
    }
}



export { Function_COUNT };
