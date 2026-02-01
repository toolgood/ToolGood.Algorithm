import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_WEIBULL extends Function_4 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Weibull", 1);
            if (args1.IsError) return args1;
            
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "Weibull", 2);
            if (args2.IsError) return args2;
            
        let args3 = this.c.Evaluate(engine, tempParameter);
            args3 = args3.ToNumber(StringCache.Function_parameter_error, "Weibull", 3);
            if (args3.IsError) return args3;
            
        let args4 = this.d.Evaluate(engine, tempParameter);
            args4 = args4.ToBoolean(StringCache.Function_parameter_error, "Weibull", 4);
            if (args4.IsError) return args4;
            
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
