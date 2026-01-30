import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_BINOMDIST extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'BinomDist', 1);
            if (args1.IsError) return args1;
        }
        let args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'BinomDist', 2);
            if (args2.IsError) return args2;
        }
        let args3 = this.func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'BinomDist', 3);
            if (args3.IsError) return args3;
        }
        let args4 = this.func4.Evaluate(work, tempParameter);
        if (args4.IsNotBoolean) {
            args4 = args4.ToBoolean(StringCache.Function_parameter_error, 'BinomDist', 4);
            if (args4.IsError) return args4;
        }

        let n2 = args2.IntValue;
        let n3 = args3.DoubleValue;
        if (!(n3 >= 0.0 && n3 <= 1.0 && n2 >= 0)) {
            return Operand.Error(StringCache.Function_parameter_error, 'BinomDist');
        }
        return Operand.Create(ExcelFunctions.BinomDist(args1.IntValue, n2, n3, args4.BooleanValue));
    }
}



export { Function_BINOMDIST };

