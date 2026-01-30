import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_NORMSINV extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            let converted1 = args1.ToNumber(StringCache.Function_parameter_error, "NormSInv");
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        let p = args1.DoubleValue;
        return Operand.Create(ExcelFunctions.NormSInv(p));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "NormSInv");
    }
}

export { Function_NORMSINV };