import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_NORMDIST extends Function_4 {
    get Name() {
        return "NormDist";
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

        let num = args1.DoubleValue;
        let avg = args2.DoubleValue;
        let STDEV = args3.DoubleValue;
        let b = args4.BooleanValue;
        return Operand.Create(ExcelFunctions.normDist(num, avg, STDEV, b));
    }
}

export { Function_NORMDIST };

