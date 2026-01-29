import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_MIN extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this.funcs) {
            const aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        const list = [];
        const o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.error('Function {0} parameter is error!', 'Min');
        }

        if (list.length === 0) {
            return Operand.error('Function {0} parameter is error!', 'Min');
        }

        return Operand.Create(Math.min(...list));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Min');
    }
}

export { Function_MIN };
