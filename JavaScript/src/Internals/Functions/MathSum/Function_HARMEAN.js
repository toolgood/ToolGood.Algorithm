import { Function_N } from '../Function_N';
import { Operand } from '../../Operand';
import { FunctionUtil } from '../FunctionUtil';

class Function_HARMEAN extends Function_N {
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
        return Operand.create(list.length / sum);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'HarMean');
    }
}

export { Function_HARMEAN };
