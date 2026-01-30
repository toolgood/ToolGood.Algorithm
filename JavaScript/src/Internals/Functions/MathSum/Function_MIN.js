import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_MIN extends Function_N {
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
            return Operand.error('Function {0} parameter is error!', 'Min');
        }

        if (list.length === 0) {
            return Operand.error('Function {0} parameter is error!', 'Min');
        }

        return Operand.Create(Math.min(...list));
    }
}

export { Function_MIN };

