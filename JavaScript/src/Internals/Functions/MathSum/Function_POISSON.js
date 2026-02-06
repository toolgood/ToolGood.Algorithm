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

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.GetNumber_2(engine, tempParameter);
        if (args2.IsError) return args2;

        let args3 = this.GetBoolean_3(engine, tempParameter);
        if (args3.IsError) return args3;
        let k = args1.IntValue;
        let lambda = args2.DoubleValue;
        let state = args3.BooleanValue;
        if (!(lambda > 0.0)) {
            return this.FunctionError();
        }
        return Operand.Create(ExcelFunctions.Poisson(k, lambda, state));
    }
}

export { Function_POISSON };
