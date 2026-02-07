import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_GAMMAINV extends Function_3 {
    get Name() {
        return "GammaInv";
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

        let probability = args1.DoubleValue;
        let alpha = args2.DoubleValue;
        let beta = args3.DoubleValue;
        if (alpha < 0.0 || beta < 0.0 || probability < 0 || probability > 1.0) {
            return this.functionError();
        }
        return Operand.Create(ExcelFunctions.GammaInv(probability, alpha, beta));
    }
}

export { Function_GAMMAINV };

