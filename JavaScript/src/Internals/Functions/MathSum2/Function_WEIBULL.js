import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_WEIBULL extends Function_4 {
    get Name() {
        return "Weibull";
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
        let shape = args2.DoubleValue;
        let scale = args3.DoubleValue;
        let state = args4.BooleanValue;
        if (shape <= 0.0 || scale <= 0.0) {
            return this.functionError();
        }

        return Operand.Create(ExcelFunctions.Weibull(x, shape, scale, state));
    }
}

export { Function_WEIBULL };
