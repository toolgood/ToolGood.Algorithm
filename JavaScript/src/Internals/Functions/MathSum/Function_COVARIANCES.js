import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_COVARIANCES extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber && args1.IsError) {
            return args1;
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber && args2.IsError) {
            return args2;
        }

        const list1 = [];
        const list2 = [];

        const o1 = FunctionUtil.F_base_GetList(args1, list1);
        const o2 = FunctionUtil.F_base_GetList(args2, list2);
        if (!o1) {
            return Operand.error('Function {0} parameter {1} is error!', 'CovarIanceS', 1);
        }
        if (!o2) {
            return Operand.error('Function {0} parameter {1} is error!', 'CovarIanceS', 2);
        }
        if (list1.length !== list2.length) {
            return Operand.error('Function {0} parameter\'s count error!', 'CovarIanceS');
        }
        if (list1.length === 1) {
            return Operand.error('Function {0} parameter\'s count error!', 'CovarIanceS');
        }

        const avg1 = list1.reduce((sum, val) => sum + val, 0) / list1.length;
        const avg2 = list2.reduce((sum, val) => sum + val, 0) / list2.length;
        let sum = 0;
        for (let i = 0; i < list1.length; i++) {
            sum += (list1[i] - avg1) * (list2[i] - avg2);
        }
        const val = sum / (list1.length - 1);
        return Operand.Create(val);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'CovarIanceS');
    }
}

export { Function_COVARIANCES };
