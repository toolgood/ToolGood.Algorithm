import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_BINOMDIST extends Function_4 {
    constructor(z) {
    super(z);
  }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'BinomDist', 1);
            if (args1.IsError) return args1;
        let args2 = this.b.Evaluate(work, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'BinomDist', 2);
            if (args2.IsError) return args2;
        let args3 = this.c.Evaluate(work, tempParameter);
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'BinomDist', 3);
            if (args3.IsError) return args3;
        let args4 = this.d.Evaluate(work, tempParameter);
            args4 = args4.ToBoolean(StringCache.Function_parameter_error, 'BinomDist', 4);
            if (args4.IsError) return args4;

        let n2 = args2.IntValue;
        let n3 = args3.NumberValue;
        if (!(n3 >= 0.0 && n3 <= 1.0 && n2 >= 0)) {
            return Operand.Error(StringCache.Function_parameter_error, 'BinomDist');
        }
        return Operand.Create(ExcelFunctions.BinomDist(args1.IntValue, n2, n3, args4.BooleanValue));
    }
}



export { Function_BINOMDIST };

