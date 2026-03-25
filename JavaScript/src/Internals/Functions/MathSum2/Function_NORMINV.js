import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_NORMINV extends Function_3 {
    get Name() {
        return "NormInv";
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

        let p = args1.DoubleValue;
        if (p <= 0.0 || p >= 1.0) {
            return this.parameterError(1);
        }
        let avg = args2.DoubleValue;
        let STDEV = args3.DoubleValue;
        if (STDEV <= 0.0) {
            return this.parameterError(3);
        }
        return Operand.Create(ExcelFunctions.NormInv(p, avg, STDEV));
    }
}

export { Function_NORMINV };