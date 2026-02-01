import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_POISSON extends Function_3 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            let converted1 = args1.ToNumber(StringCache.Function_parameter_error, "Poisson", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        let args2 = this.b.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            let converted2 = args2.ToNumber(StringCache.Function_parameter_error, "Poisson", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        }
        let args3 = this.c.Evaluate(engine, tempParameter);
        if (args3.IsNotBoolean) {
            let converted3 = args3.ToBoolean(StringCache.Function_parameter_error, "Poisson", 3);
            if (converted3.IsError) return converted3;
            args3 = converted3;
        }
        let k = args1.IntValue;
        let lambda = args2.NumberValue;
        let state = args3.BooleanValue;
        if (!(lambda > 0.0)) {
            return Operand.Error(StringCache.Function_parameter_error, "Poisson");
        }
        return Operand.Create(ExcelFunctions.Poisson(k, lambda, state));
    }
}

export { Function_POISSON };
