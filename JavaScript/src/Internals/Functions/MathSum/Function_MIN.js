import { Function_N } from '../Function_N';
import { Operand } from '../../Operand';
import { FunctionUtil } from '../FunctionUtil';

class Function_MIN extends Function_N {
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
            return Operand.error('Function {0} parameter is error!', 'Min');
        }

        if (list.length === 0) {
            return Operand.error('Function {0} parameter is error!', 'Min');
        }

        return Operand.create(Math.min(...list));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Min');
    }
}

export { Function_MIN };
