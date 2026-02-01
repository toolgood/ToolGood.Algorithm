import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_NORMDIST extends Function_4 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'NormDist', 1);
            if (args1.IsError) {
                return args1;
            }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'NormDist', 2);
            if (args2.IsError) {
                return args2;
            }
        let args3 = this.c.Evaluate(engine, tempParameter);
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'NormDist', 3);
            if (args3.IsError) {
                return args3;
            }
        let args4 = this.d.Evaluate(engine, tempParameter);
            args4 = args4.ToBoolean(StringCache.Function_parameter_error, 'NormDist', 4);
            if (args4.IsError) {
                return args4;
            }

        let num = args1.NumberValue;
        let avg = args2.NumberValue;
        let STDEV = args3.NumberValue;
        let b = args4.BooleanValue;
        return Operand.Create(ExcelFunctions.NormDist(num, avg, STDEV, b));
    }
}

export { Function_NORMDIST };

