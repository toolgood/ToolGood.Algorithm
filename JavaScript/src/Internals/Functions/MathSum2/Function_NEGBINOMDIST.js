import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_NEGBINOMDIST extends Function_3 {
    get Name() {
        return "NegBinomDist";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) return args2;

        let args3 = this.getNumber_3(engine, tempParameter);
        if (args3.IsError) return args3;

        let k = args1.IntValue;
        if (k < 0) {
            return this.parameterError(1);
        }
        let r = args2.DoubleValue;
        if (r <= 0.0) {
            return this.parameterError(2);
        }
        let p = args3.DoubleValue;
        if (p < 0.0 || p > 1.0) {
            return this.parameterError(3);
        }
        return Operand.Create(ExcelFunctions.NegbinomDist(k, r, p));
    }
}

export { Function_NEGBINOMDIST };

