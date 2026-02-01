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
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Poisson", 1);
            if (args1.IsError) return args1;
            
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "Poisson", 2);
            if (args2.IsError) return args2;
            
        let args3 = this.c.Evaluate(engine, tempParameter);
            args3 = args3.ToBoolean(StringCache.Function_parameter_error, "Poisson", 3);
            if (args3.IsError) return args3;
            
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
