import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_POISSON extends Function_3 {
    get Name() {
        return "Poisson";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) return args2;

        let args3 = this.getBoolean_3(engine, tempParameter);
        if (args3.IsError) return args3;
        let k = args1.IntValue;
        let lambda = args2.DoubleValue;
        let state = args3.BooleanValue;
        if (!(lambda > 0.0)) {
            return this.functionError();
        }
        return Operand.Create(ExcelFunctions.poisson(k, lambda, state));
    }
}

export { Function_POISSON };
