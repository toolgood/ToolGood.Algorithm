import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_TINV extends Function_2 {
    get Name() {
        return "TInv";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) return args2;
        let p = args1.DoubleValue;
        let degreesFreedom = args2.IntValue;
        if (degreesFreedom <= 0.0 || p < 0.0 || p > 1.0) {
            return this.functionError();
        }
        return Operand.Create(ExcelFunctions.tInv(p, degreesFreedom));
    }
}

export { Function_TINV };
