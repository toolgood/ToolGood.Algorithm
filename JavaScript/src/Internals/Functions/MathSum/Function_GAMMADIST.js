import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_GAMMADIST extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'GammaDist', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'GammaDist', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'GammaDist', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        let args4 = this.func4.Evaluate(engine, tempParameter);
        if (args4.IsNotBoolean) {
            args4 = args4.ToBoolean(StringCache.Function_parameter_error, 'GammaDist', 4);
            if (args4.IsError) {
                return args4;
            }
        }
        let x = args1.NumberValue;
        let alpha = args2.NumberValue;
        let beta = args3.NumberValue;
        let cumulative = args4.BooleanValue;
        if (alpha < 0.0 || beta < 0.0) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'GammaDist');
        }
        return Operand.Create(ExcelFunctions.GammaDist(x, alpha, beta, cumulative));
    }
}

export { Function_GAMMADIST };

