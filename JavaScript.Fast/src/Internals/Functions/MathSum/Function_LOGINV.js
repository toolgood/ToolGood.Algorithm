import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_LOGINV extends Function_3 {
    get Name() {
        return "LogInv";
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
        let n2 = args2.DoubleValue;
        let n3 = args3.DoubleValue;
        if (probability <= 0.0 || probability >= 1.0) { return this.parameterError(1); }
        if (n3 <= 0.0) { return this.parameterError(3); }
        return Operand.Create(ExcelFunctions.logInv(probability, n2, n3));
    }
}

export { Function_LOGINV };

