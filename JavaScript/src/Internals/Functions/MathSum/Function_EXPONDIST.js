import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_EXPONDIST extends Function_3 {
    constructor(funcs) {
    super(funcs);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'ExponDist', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'ExponDist', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotBoolean) {
            args3 = args3.ToBoolean(StringCache.Function_parameter_error, 'ExponDist', 3);
            if (args3.IsError) {
                return args3;
            }
        }

        let n1 = args1.NumberValue;
        if (n1 < 0.0) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'ExponDist');
        }
        return Operand.Create(ExcelFunctions.ExponDist(n1, args2.NumberValue, args3.BooleanValue));
    }
}

export { Function_EXPONDIST };

