import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_FINV extends Function_3 {
    get Name() {
        return "FInv";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.GetNumber_2(engine, tempParameter);
        if (args2.IsError) return args2;

        let args3 = this.GetNumber_3(engine, tempParameter);
        if (args3.IsError) return args3;

        let p = args1.DoubleValue;
        let degreesFreedom = args2.IntValue;
        let degreesFreedom2 = args3.IntValue;
        if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0 || p < 0.0 || p > 1.0) {
            return this.FunctionError();
        }
        return Operand.Create(ExcelFunctions.FInv(p, degreesFreedom, degreesFreedom2));
    }
}

export { Function_FINV };

