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

    evaluate(engine, tempParameter) {
        let args = [];
        let error = this.tryEvaluateAll(engine, tempParameter, args);
        if (error != null) {
            return error;
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return this.functionError();
        }
        if (list.length == 0) {
            return this.functionError();
        }

        let sum = 0;
        for (let db of list) {
            if (db === 0) {
                return this.functionError();
            }
            sum += 1 / db;
        }
        if (sum === 0) {
            return this.functionError();
        }
        return Operand.Create(list.length / sum);
    }
}

export { Function_HARMEAN };

