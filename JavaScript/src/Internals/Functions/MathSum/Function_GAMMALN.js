import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_GAMMALN extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_1_error, 'GammaLn');
            if (args1.IsError) {
                return args1;
            }
        return Operand.Create(ExcelFunctions.GAMMALN(args1.NumberValue));
    }
}

export { Function_GAMMALN };

