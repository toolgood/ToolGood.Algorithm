import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_BETADIST extends Function_3 {
    get Name() {
        return "BetaDist";
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

        let x = args1.DoubleValue;
        if (x < 0.0 || x > 1.0) {
            return this.parameterError(1);
        }
        let alpha = args2.DoubleValue;
        if (alpha <= 0.0) {
            return this.parameterError(2);
        }
        let beta = args3.DoubleValue;
        if (beta <= 0.0) {
            return this.parameterError(3);
        }
        return Operand.Create(ExcelFunctions.BetaDist(x, alpha, beta));
    }
}



export { Function_BETADIST };

