import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_TDIST extends Function_3 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "TDist", 1);
            if (args1.IsError) return args1;
            
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "TDist", 2);
            if (args2.IsError) return args2;
            
        let args3 = this.c.Evaluate(engine, tempParameter);
            args3 = args3.ToNumber(StringCache.Function_parameter_error, "TDist", 3);
            if (args3.IsError) return args3;
            
        let x = args1.NumberValue;
        let degreesFreedom = args2.IntValue;
        let tails = args3.IntValue;
        if (degreesFreedom <= 0.0 || tails < 1 || tails > 2) {
            return Operand.Error(StringCache.Function_parameter_error, "TDist");
        }
        return Operand.Create(ExcelFunctions.TDist(x, degreesFreedom, tails));
    }
}

export { Function_TDIST };
