import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_FDIST extends Function_3 {
    constructor(funcs) {
    super(funcs);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'FDist', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'FDist', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'FDist', 3);
            if (args3.IsError) {
                return args3;
            }
        }

        let x = args1.NumberValue;
        let degreesFreedom = Math.round(args2.NumberValue);
        let degreesFreedom2 = Math.round(args3.NumberValue);
        if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'FDist');
        }
        return Operand.Create(ExcelFunctions.FDist(x, degreesFreedom, degreesFreedom2));
    }
}

export { Function_FDIST };

