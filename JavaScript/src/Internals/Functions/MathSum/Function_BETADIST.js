import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_BETADIST extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber('Function \'{0}\' parameter {1} is error!', 'BetaDist', 1);
            if (args1.IsError) return args1;
        }
        let args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber('Function \'{0}\' parameter {1} is error!', 'BetaDist', 2);
            if (args2.IsError) return args2;
        }
        let args3 = this.func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber('Function \'{0}\' parameter {1} is error!', 'BetaDist', 3);
            if (args3.IsError) return args3;
        }
        let x = args1.DoubleValue;
        let alpha = args2.DoubleValue;
        let beta = args3.DoubleValue;

        if (alpha < 0.0 || beta < 0.0) {
            return Operand.Error('Function \'{0}\' parameter is error!', 'BetaDist');
        }
        return Operand.Create(ExcelFunctions.BetaDist(x, alpha, beta));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'BetaDist');
    }
}



export { Function_BETADIST };
