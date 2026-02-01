import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_PERCENTILE extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotArray) {
            let converted1 = args1.ToArray(StringCache.Function_parameter_error, "Percentile", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        let args2 = this.b.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            let converted2 = args2.ToNumber(StringCache.Function_parameter_error, "Percentile", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        }
        let list = [];
        let o = FunctionUtil.F_base_GetList(args1.ArrayValue, list);
        if (o == false) {
            return Operand.Error(StringCache.Function_parameter_error, "Percentile", 1);
        }
        let k = args2.NumberValue;
        return Operand.Create(ExcelFunctions.Percentile(list.map(q => q), k));
    }
}

export { Function_PERCENTILE };
