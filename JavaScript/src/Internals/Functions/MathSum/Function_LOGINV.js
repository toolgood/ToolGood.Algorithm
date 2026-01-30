import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_LOGINV extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber('Function {0} parameter {1} is error!', 'LogInv', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber('Function {0} parameter {1} is error!', 'LogInv', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber('Function {0} parameter {1} is error!', 'LogInv', 3);
            if (args3.IsError) {
                return args3;
            }
        }

        let n3 = args3.DoubleValue;
        if (n3 < 0.0) {
            return Operand.error('Function {0} parameter is error!', 'LogInv');
        }
        return Operand.Create(ExcelFunctions.LogInv(args1.DoubleValue, args2.DoubleValue, n3));
    }
}

export { Function_LOGINV };

