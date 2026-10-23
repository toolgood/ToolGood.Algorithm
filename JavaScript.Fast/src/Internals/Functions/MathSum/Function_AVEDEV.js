import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_AVEDEV extends Function_N {
    get Name() {
        return "AveDev";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args = [];
        let error = this.tryEvaluateAll(engine, tempParameter, args);
        if (error != null) { return error; }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) { return this.functionError(); }
        if (list.length == 0) { return Operand.Zero; }
        let avg = list.reduce((sum, value) => sum + value, 0) / list.length;
        let sum = 0;
        for (let item of list) {
            sum += Math.abs(item - avg);
        }
        return Operand.Create(sum / list.length);
    }
}

export { Function_AVEDEV };

