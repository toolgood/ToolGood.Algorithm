import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_HARMEAN extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this.funcs) {
            const aa = item.Evaluate(engine, tempParameter);
            if (aa.isError) {
                return aa;
            }
            args.push(aa);
        }

        if (args.length === 1) {
            return args[0];
        }

        const list = [];
        const o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.error('Function {0} parameter is error!', 'HarMean');
        }

        let sum = 0;
        for (const db of list) {
            if (db === 0) {
                return Operand.error('Function {0} parameter is error!', 'HarMean');
            }
            sum += 1 / db;
        }
        if (sum === 0) {
            return Operand.error('Function {0} parameter is error!', 'HarMean');
        }
        return Operand.Create(list.length / sum);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'HarMean');
    }
}

export { Function_HARMEAN };
