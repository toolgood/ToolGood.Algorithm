import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_MAX extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this.funcs) {
            const aa = item.evaluate(engine, tempParameter);
            if (aa.isError) {
                return aa;
            }
            args.push(aa);
        }

        const list = [];
        const o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.error('Function {0} parameter is error!', 'Max');
        }

        if (list.length === 0) {
            return Operand.error('Function {0} parameter is error!', 'Max');
        }

        return Operand.create(Math.max(...list));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Max');
    }
}

export { Function_MAX };
