import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_TINV extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            let converted1 = args1.ToNumber(StringCache.Function_parameter_error, "TInv", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        let args2 = this.b.Evaluate(engine, tempParameter);
            let converted2 = args2.ToNumber(StringCache.Function_parameter_error, "TInv", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        let p = args1.NumberValue;
        let degreesFreedom = args2.IntValue;
        if (degreesFreedom <= 0.0 || p < 0.0 || p > 1.0) {
            return Operand.Error(StringCache.Function_parameter_error, "TInv");
        }
        return Operand.Create(ExcelFunctions.TInv(p, degreesFreedom));
    }
}

export { Function_TINV };
