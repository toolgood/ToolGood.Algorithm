import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_GAMMAINV extends Function_3 {
    constructor(funcs) {
    super(funcs);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'GammaInv', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'GammaInv', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'GammaInv', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        let probability = args1.NumberValue;
        let alpha = args2.NumberValue;
        let beta = args3.NumberValue;
        if (alpha < 0.0 || beta < 0.0 || probability < 0 || probability > 1.0) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'GammaInv');
        }
        return Operand.Create(ExcelFunctions.GammaInv(probability, alpha, beta));
    }
}

export { Function_GAMMAINV };

