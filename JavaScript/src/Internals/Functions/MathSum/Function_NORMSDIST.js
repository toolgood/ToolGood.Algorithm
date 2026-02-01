import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_NORMSDIST extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            let converted1 = args1.ToNumber(StringCache.Function_parameter_error, "NormSDist");
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        let num = args1.NumberValue;
        return Operand.Create(ExcelFunctions.NormSDist(num));
    }
}

export { Function_NORMSDIST };
