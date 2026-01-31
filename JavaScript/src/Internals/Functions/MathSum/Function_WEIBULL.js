import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_WEIBULL extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            let converted1 = args1.ToNumber(StringCache.Function_parameter_error, "Weibull", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            let converted2 = args2.ToNumber(StringCache.Function_parameter_error, "Weibull", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            let converted3 = args3.ToNumber(StringCache.Function_parameter_error, "Weibull", 3);
            if (converted3.IsError) return converted3;
            args3 = converted3;
        }
        let args4 = this.func4.Evaluate(engine, tempParameter);
        if (args4.IsNotBoolean) {
            let converted4 = args4.ToBoolean(StringCache.Function_parameter_error, "Weibull", 4);
            if (converted4.IsError) return converted4;
            args4 = converted4;
        }
        let x = args1.NumberValue;
        let shape = args2.NumberValue;
        let scale = args3.NumberValue;
        let state = args4.BooleanValue;
        if (shape <= 0.0 || scale <= 0.0) {
            return Operand.Error(StringCache.Function_parameter_error, "Weibull");
        }

        return Operand.Create(ExcelFunctions.Weibull(x, shape, scale, state));
    }
}

export { Function_WEIBULL };
