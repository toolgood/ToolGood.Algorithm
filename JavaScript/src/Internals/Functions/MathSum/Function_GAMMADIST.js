import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_GAMMADIST extends Function_4 {
    get Name() {
        return "GammaDist";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.GetNumber_2(engine, tempParameter);
        if (args2.IsError) return args2;

        let args3 = this.GetNumber_3(engine, tempParameter);
        if (args3.IsError) return args3;

        let args4 = this.GetBoolean_4(engine, tempParameter);
        if (args4.IsError) return args4;

        let x = args1.DoubleValue;
        let alpha = args2.DoubleValue;
        let beta = args3.DoubleValue;
        let cumulative = args4.BooleanValue;
        if (alpha < 0.0 || beta < 0.0) {
            return this.FunctionError();
        }
        return Operand.Create(ExcelFunctions.GammaDist(x, alpha, beta, cumulative));
    }
}

export { Function_GAMMADIST };

