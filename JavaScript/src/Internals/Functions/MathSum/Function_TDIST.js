import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_TDIST extends Function_3 {
    get Name() {
        return "TDist";
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
        let x = args1.DoubleValue;
        let degreesFreedom = args2.IntValue;
        let tails = args3.IntValue;
        if (degreesFreedom <= 0.0 || tails < 1 || tails > 2) {
            return this.functionError();
        }
        return Operand.Create(ExcelFunctions.TDist(x, degreesFreedom, tails));
    }
}

export { Function_TDIST };
