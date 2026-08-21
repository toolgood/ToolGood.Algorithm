import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_WEIBULL extends Function_4 {
    get Name() {
        return "Weibull";
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

        let args4 = this.getBoolean_4(engine, tempParameter);
        if (args4.IsError) return args4;
        let x = args1.DoubleValue;
        let shape = args2.DoubleValue;
        let scale = args3.DoubleValue;
        let state = args4.BooleanValue;
        if (shape <= 0.0) { return this.parameterError(2); }
        if (scale <= 0.0) { return this.parameterError(3); }

        let result = ExcelFunctions.weibull(x, shape, scale, state);
        // x 过大时 Pow/Exp 溢出为 Infinity,或 x=0 且 shape<1 时 0 的负次幂产生 NaN,对应 C# 捕获异常返回错误
        if (!isFinite(result)) { return this.functionError(); }
        return Operand.Create(result);
    }
}

export { Function_WEIBULL };
