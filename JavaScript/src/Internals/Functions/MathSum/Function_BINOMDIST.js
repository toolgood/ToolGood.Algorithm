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

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.getNumber_2(work, tempParameter);
        if (args2.IsError) return args2;

        let args3 = this.getNumber_3(work, tempParameter);
        if (args3.IsError) return args3;

        let args4 = this.getBoolean_4(work, tempParameter);
        if (args4.IsError) return args4;

        let n2 = args2.IntValue;
        let n3 = args3.DoubleValue;
        if (!(n3 >= 0.0 && n3 <= 1.0 && n2 >= 0)) {
            return this.functionError();
        }
        return Operand.Create(ExcelFunctions.binomDist(args1.IntValue, n2, n3, args4.BooleanValue));
    }
}



export { Function_BINOMDIST };

