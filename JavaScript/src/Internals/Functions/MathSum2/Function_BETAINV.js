import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_BETAINV extends Function_3 {
    get Name() {
        return "BetaInv";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.getNumber_2(work, tempParameter);
        if (args2.IsError) return args2;

        let args3 = this.getNumber_3(work, tempParameter);
        if (args3.IsError) return args3;

        let p = args1.DoubleValue;
        let alpha = args2.DoubleValue;
        let beta = args3.DoubleValue;
        if (alpha < 0.0 || beta < 0.0 || p < 0.0 || p > 1.0) {
            return this.functionError();
        }
        return Operand.Create(ExcelFunctions.BetaInv(p, alpha, beta));
    }
}



export { Function_BETAINV };

