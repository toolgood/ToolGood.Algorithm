import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_QUARTILE extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            let converted1 = args1.ToArray(StringCache.Function_parameter_error, "Quartile", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        let args2 = this.b.Evaluate(engine, tempParameter);
            let converted2 = args2.ToNumber(StringCache.Function_parameter_error, "Quartile", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;

        let list = [];
        
        let o = FunctionUtil.F_base_GetList(args1.ArrayValue, list);
        if (o == false) {
            return Operand.Error(StringCache.Function_parameter_error, "Quartile", 1);
        }

        let quant = args2.IntValue;
        if (quant < 0 || quant > 4) {
            return Operand.Error(StringCache.Function_parameter_error, "Quartile", 2);
        }
        
        let result = ExcelFunctions.Quartile(list, quant);
        return Operand.Create(result);
    }
}

export { Function_QUARTILE };
