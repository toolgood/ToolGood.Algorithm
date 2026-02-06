import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_BINOMDIST extends Function_4 {
    get Name() {
        return "BinomDist";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetNumber_1(work, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.GetNumber_2(work, tempParameter);
        if (args2.IsError) return args2;

        let args3 = this.GetNumber_3(work, tempParameter);
        if (args3.IsError) return args3;

        let args4 = this.GetBoolean_4(work, tempParameter);
        if (args4.IsError) return args4;

        let n2 = args2.IntValue;
        let n3 = args3.DoubleValue;
        if (!(n3 >= 0.0 && n3 <= 1.0 && n2 >= 0)) {
            return this.FunctionError();
        }
        return Operand.Create(ExcelFunctions.BinomDist(args1.IntValue, n2, n3, args4.BooleanValue));
    }
}



export { Function_BINOMDIST };

