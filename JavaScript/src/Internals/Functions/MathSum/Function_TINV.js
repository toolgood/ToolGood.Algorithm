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

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.GetNumber_2(engine, tempParameter);
        if (args2.IsError) return args2;
        let p = args1.DoubleValue;
        let degreesFreedom = args2.IntValue;
        if (degreesFreedom <= 0.0 || p < 0.0 || p > 1.0) {
            return this.FunctionError();
        }
        return Operand.Create(ExcelFunctions.TInv(p, degreesFreedom));
    }
}

export { Function_TINV };
