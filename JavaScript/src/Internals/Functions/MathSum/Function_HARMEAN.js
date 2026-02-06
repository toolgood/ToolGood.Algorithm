import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_HARMEAN extends Function_N {
    get Name() {
        return "HarMean";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        if (args.length === 1) {
            return args[0];
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return this.FunctionError();
        }

        let sum = 0;
        for (let db of list) {
            if (db === 0) {
                return this.FunctionError();
            }
            sum += 1 / db;
        }
        if (sum === 0) {
            return this.FunctionError();
        }
        return Operand.Create(list.length / sum);
    }
}

export { Function_HARMEAN };

