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

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) return args2;

        let args3 = this.getNumber_3(engine, tempParameter);
        if (args3.IsError) return args3;

        let args4 = this.getBoolean_4(engine, tempParameter);
        if (args4.IsError) return args4;

        let x = args1.DoubleValue;
        let alpha = args2.DoubleValue;
        let beta = args3.DoubleValue;
        let cumulative = args4.BooleanValue;
        if (alpha < 0.0 || beta < 0.0) {
            return this.functionError();
        }
        return Operand.Create(ExcelFunctions.gammaDist(x, alpha, beta, cumulative));
    }
}

export { Function_GAMMADIST };

