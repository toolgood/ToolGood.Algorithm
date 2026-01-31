import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_COVAR extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber && args1.IsError) {
            return args1;
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber && args2.IsError) {
            return args2;
        }
        let list1 = [];
        let list2 = [];
        let o1 = FunctionUtil.F_base_GetList(args1.ArrayValue, list1);
        let o2 = FunctionUtil.F_base_GetList(args2.ArrayValue, list2);
        if (!o1) {
            return Operand.Error(StringCache.Function_parameter_error, 'Covar', 1);
        }
        if (!o2) {
            return Operand.Error(StringCache.Function_parameter_error, 'Covar', 2);
        }
        if (list1.length !== list2.length) {
            return Operand.Error(StringCache.Function_error, 'Covar');
        }
        if (list1.length === 0) {
            return Operand.Error(StringCache.Function_error, 'Covar');
        }

        let avg1 = list1.reduce((sum, val) => sum + val, 0) / list1.length;
        let avg2 = list2.reduce((sum, val) => sum + val, 0) / list2.length;
        let sum = 0;
        for (let i = 0; i < list1.length; i++) {
            sum += (list1[i] - avg1) * (list2[i] - avg2);
        }
        let val = sum / list1.length;
        return Operand.Create(val);
    }
}

export { Function_COVAR };

