import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_NORMSINV extends Function_1 {
    get Name() {
        return "NormSInv";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) return args1;
        let p = args1.DoubleValue;
        if (p <= 0.0 || p >= 1.0) {
            return this.parameterError(1);
        }
        return Operand.Create(ExcelFunctions.NormSInv(p));
    }
}

export { Function_NORMSINV };
