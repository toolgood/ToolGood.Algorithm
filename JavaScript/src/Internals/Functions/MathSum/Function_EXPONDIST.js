import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_EXPONDIST extends Function_3 {
    get Name() {
        return "ExpOnDist";
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

        let n1 = args1.DoubleValue;
        if (n1 < 0.0) {
            return this.FunctionError();
        }
        return Operand.Create(ExcelFunctions.ExponDist(n1, args2.DoubleValue, args3.BooleanValue));
    }
}

export { Function_EXPONDIST };

