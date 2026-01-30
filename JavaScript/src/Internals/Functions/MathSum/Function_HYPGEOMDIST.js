import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_HYPGEOMDIST extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber('Function {0} parameter {1} is error!', 'HypgeomDist', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber('Function {0} parameter {1} is error!', 'HypgeomDist', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber('Function {0} parameter {1} is error!', 'HypgeomDist', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        let args4 = this.func4.Evaluate(engine, tempParameter);
        if (args4.IsNotNumber) {
            args4 = args4.ToNumber('Function {0} parameter {1} is error!', 'HypgeomDist', 4);
            if (args4.IsError) {
                return args4;
            }
        }
        let k = Math.round(args1.DoubleValue);
        let draws = Math.round(args2.DoubleValue);
        let success = Math.round(args3.DoubleValue);
        let population = Math.round(args4.DoubleValue);
        if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population)) {
            return Operand.error('Function {0} parameter is error!', 'HypgeomDist');
        }
        return Operand.Create(ExcelFunctions.HypgeomDist(k, draws, success, population));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'HypgeomDist');
    }
}

export { Function_HYPGEOMDIST };
