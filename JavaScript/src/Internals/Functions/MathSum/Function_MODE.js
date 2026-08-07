import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_MODE extends Function_N {
    get Name() {
        return "Mode";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return this.functionError();
        }

        let dict = {};
        for (let item of list) {
            if (dict[item] !== undefined) {
                dict[item] += 1;
            } else {
                dict[item] = 1;
            }
        }

        let sorted = Object.entries(dict).sort((a, b) => b[1] - a[1]);
        // 所有值都只出现一次时,无众数,返回错误
        if (sorted[0][1] == 1) {
            return this.functionError();
        }
        return Operand.Create(parseFloat(sorted[0][0]));
    }
}

export { Function_MODE };

