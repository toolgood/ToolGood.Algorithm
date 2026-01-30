import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_GAMMALN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber('Function {0} parameter is error!', 'GammaLn');
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.Create(ExcelFunctions.GAMMALN(args1.DoubleValue));
    }
}

export { Function_GAMMALN };

