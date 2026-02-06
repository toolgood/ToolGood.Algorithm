import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_COVARIANCES extends Function_2 {
    get Name() {
        return "Covariances";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter); if (args1.IsError) { return args1; }
        let args2 = this.b.Evaluate(engine, tempParameter); if (args2.IsError) { return args2; }

        let list1 = [];
        let list2 = [];

        let o1 = FunctionUtil.F_base_GetList(args1, list1);
        let o2 = FunctionUtil.F_base_GetList(args2, list2);
        if (o1 == false) { return this.ParameterError(1); }
        if (o2 == false) { return this.ParameterError(2); }
        if (list1.length != list2.length) { return Operand.Error("Function '{0}' parameter's count error!", "CovarIanceS"); }
        if (list1.length == 1) { return Operand.Error("Function '{0}' parameter's count error!", "CovarIanceS"); }

        let avg1 = list1.reduce((sum, val) => sum + val, 0) / list1.length;
        let avg2 = list2.reduce((sum, val) => sum + val, 0) / list2.length;
        let sum = 0;
        for (let i = 0; i < list1.length; i++) {
            sum += (list1[i] - avg1) * (list2[i] - avg2);
        }
        let val = sum / (list1.length - 1);
        return Operand.Create(val);
    }
}

export { Function_COVARIANCES };

