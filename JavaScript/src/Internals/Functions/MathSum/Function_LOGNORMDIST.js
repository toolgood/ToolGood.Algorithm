import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_LOGNORMDIST extends Function_3 {
    get Name() {
        return "LogNormDist";
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

        let n3 = args3.DoubleValue;
        if (n3 < 0.0) {
            return this.functionError();
        }
        return Operand.Create(ExcelFunctions.lognormDist(args1.DoubleValue, args2.DoubleValue, n3));
    }
}

export { Function_LOGNORMDIST };

