import { Function_N } from '../Function_N';
import { Operand } from '../../Operand';
import { FunctionUtil } from '../FunctionUtil';

class Function_DEVSQ extends Function_N {
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
            return Operand.error('Function {0} parameter is error!', 'DevSQ');
        }
        if (list.length === 0) {
            return Operand.error('Function {0} parameter is error!', 'DevSQ');
        }
        const avg = list.reduce((sum, val) => sum + val, 0) / list.length;
        let sum = 0;
        for (let i = 0; i < list.length; i++) {
            const diff = list[i] - avg;
            sum += diff * diff;
        }
        return Operand.create(sum);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'DevSQ');
    }
}

export { Function_DEVSQ };
