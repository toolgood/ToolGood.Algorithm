import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_COVAR extends Function_2 {
    get Name() {
        return "Covar";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.a.evaluate(engine, tempParameter); if (args1.IsError) { return args1; }
        let args2 = this.b.evaluate(engine, tempParameter); if (args2.IsError) { return args2; }
        let list1 = [];
        let list2 = [];
        let o1 = FunctionUtil.F_base_GetList(args1, list1);
        let o2 = FunctionUtil.F_base_GetList(args2, list2);
        if (o1 == false) { return this.parameterError(1); }
        if (o2 == false) { return this.parameterError(2); }
        if (list1.length != list2.length) { return Operand.Error("Function '{0}' parameter's count error!", "Covar"); }
        if (list1.length == 0) { return Operand.Error("Function '{0}' parameter's count error!", "Covar"); }

        let mean1 = 0, mean2 = 0, c = 0;
        for (let i = 0; i < list1.length; i++) {
            let delta1 = list1[i] - mean1;
            let delta2 = list2[i] - mean2;
            mean1 += delta1 / (i + 1);
            mean2 += delta2 / (i + 1);
            c += delta1 * (list2[i] - mean2);
        }
        return Operand.Create(c / list1.length);
    }
}

export { Function_COVAR };

