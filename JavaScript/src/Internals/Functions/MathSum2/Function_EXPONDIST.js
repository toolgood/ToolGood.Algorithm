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

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) return args2;

        let args3 = this.getBoolean_3(engine, tempParameter);
        if (args3.IsError) return args3;

        let n1 = args1.DoubleValue;
        if (n1 < 0.0) {
            return this.parameterError(1);
        }
        let rate = args2.DoubleValue;
        if (rate <= 0.0) {
            return this.parameterError(2);
        }
        return Operand.Create(ExcelFunctions.ExponDist(n1, rate, args3.BooleanValue));
    }
}

export { Function_EXPONDIST };

