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

    Evaluate(work, tempParameter) {
        let args1 = this.GetNumber_1(work, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.GetNumber_2(work, tempParameter);
        if (args2.IsError) return args2;

        let args3 = this.GetNumber_3(work, tempParameter);
        if (args3.IsError) return args3;

        let x = args1.DoubleValue;
        let alpha = args2.DoubleValue;
        let beta = args3.DoubleValue;

        if (alpha < 0.0 || beta < 0.0) {
            return this.FunctionError();
        }
        return Operand.Create(ExcelFunctions.BetaDist(x, alpha, beta));
    }
}



export { Function_BETADIST };

