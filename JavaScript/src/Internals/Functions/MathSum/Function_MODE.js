import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_MODE extends Function_N {
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
            return Operand.error('Function {0} parameter is error!', 'Mode');
        }

        if (list.length === 0) {
            return Operand.error('Function {0} parameter is error!', 'Mode');
        }

        const dict = {};
        for (const item of list) {
            if (dict[item] !== undefined) {
                dict[item] += 1;
            } else {
                dict[item] = 1;
            }
        }

        // 按出现次数降序排序，返回出现次数最多的数字
        const sorted = Object.entries(dict).sort((a, b) => b[1] - a[1]);
        return Operand.create(parseFloat(sorted[0][0]));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Mode');
    }
}

export { Function_MODE };
