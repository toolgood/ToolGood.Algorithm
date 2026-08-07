import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_MEDIAN extends Function_N {
    get Name() {
        return "Median";
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

        list.sort((a, b) => a - b);
        let mid = Math.floor(list.length / 2);
        if (list.length % 2 === 0) {
            return Operand.Create((list[mid - 1] + list[mid]) / 2);
        }
        return Operand.Create(list[mid]);
    }
}

export { Function_MEDIAN };

