import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_LARGE extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToArray(StringCache.Function_parameter_error, 'Large', 1);
            if (args1.IsError) {
                return args1;
            }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'Large', 2);
            if (args2.IsError) {
                return args2;
            }
        let list = [];
        let o = FunctionUtil.F_base_GetList(args1.ArrayValue, list);
        if (!o) {
            return Operand.Error(StringCache.Function_parameter_error, 'Large', 1);
        }

        list.sort((a, b) => b - a); // 降序排序
        let k = Math.round(args2.NumberValue);
        if (k < 1 - engine.ExcelIndex || k > list.length - engine.ExcelIndex) {
            return Operand.Error(StringCache.Function_parameter_error, 'Large', 2);
        }
        return Operand.Create(list[k - engine.ExcelIndex]);
    }
}

export { Function_LARGE };

