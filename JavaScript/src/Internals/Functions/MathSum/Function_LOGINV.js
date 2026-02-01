import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_LOGINV extends Function_3 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'LogInv', 1);
            if (args1.IsError) {
                return args1;
            }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'LogInv', 2);
            if (args2.IsError) {
                return args2;
            }
        let args3 = this.c.Evaluate(engine, tempParameter);
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'LogInv', 3);
            if (args3.IsError) {
                return args3;
            }

        let n3 = args3.NumberValue;
        if (n3 < 0.0) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'LogInv');
        }
        return Operand.Create(ExcelFunctions.LogInv(args1.NumberValue, args2.NumberValue, n3));
    }
}

export { Function_LOGINV };

