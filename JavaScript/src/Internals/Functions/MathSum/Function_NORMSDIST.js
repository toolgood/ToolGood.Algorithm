import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_NORMSDIST extends Function_1 {
    get Name() {
        return "NormSDist";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) return args1;
        let num = args1.DoubleValue;
        return Operand.Create(ExcelFunctions.normSDist(num));
    }
}

export { Function_NORMSDIST };
