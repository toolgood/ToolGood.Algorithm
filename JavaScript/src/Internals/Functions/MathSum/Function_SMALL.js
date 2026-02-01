import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_SMALL extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            let converted1 = args1.ToArray(StringCache.Function_parameter_error, "Small", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        let args2 = this.b.Evaluate(engine, tempParameter);
            let converted2 = args2.ToNumber(StringCache.Function_parameter_error, "Small", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        let list = [];
        let o = FunctionUtil.F_base_GetList(args1.ArrayValue, list);
        if (o == false) {
            return Operand.Error(StringCache.Function_parameter_error, "Small", 1);
        }
        list.sort((a, b) => a - b);
        let k = Math.round(args2.NumberValue);
        if (k < 1 - engine.ExcelIndex || k > list.length - engine.ExcelIndex) {
            return Operand.Error(StringCache.Function_parameter_error, "Small", 2);
        }
        return Operand.Create(list[k - engine.ExcelIndex]);
    }
}

export { Function_SMALL };
