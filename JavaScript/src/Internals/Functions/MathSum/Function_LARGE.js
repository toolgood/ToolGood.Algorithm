import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_LARGE extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (!args1.IsArray) {
            args1 = args1.ToArray('Function {0} parameter {1} is error!', 'Large', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber('Function {0} parameter {1} is error!', 'Large', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let list = [];
        let o = FunctionUtil.F_base_GetList(args1.ArrayValue, list);
        if (!o) {
            return Operand.Error('Function {0} parameter {1} is error!', 'Large', 1);
        }

        list.sort((a, b) => b - a); // 降序排序
        let k = Math.round(args2.DoubleValue);
        if (k < 1 - engine.ExcelIndex || k > list.length - engine.ExcelIndex) {
            return Operand.Error('Function {0} parameter {1} is error!', 'Large', 2);
        }
        return Operand.Create(list[k - engine.ExcelIndex]);
    }
}

export { Function_LARGE };

