import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_NEGBINOMDIST extends Function_3 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'NegbinomDist', 1);
            if (args1.IsError) {
                return args1;
            }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'NegbinomDist', 2);
            if (args2.IsError) {
                return args2;
            }
        let args3 = this.c.Evaluate(engine, tempParameter);
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'NegbinomDist', 3);
            if (args3.IsError) {
                return args3;
            }
        let k = Math.round(args1.NumberValue);
        let r = args2.NumberValue;
        let p = args3.NumberValue;

        if (!(r >= 0.0 && p >= 0.0 && p <= 1.0)) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'NegbinomDist');
        }
        return Operand.Create(ExcelFunctions.NegbinomDist(k, r, p));
    }
}

export { Function_NEGBINOMDIST };

