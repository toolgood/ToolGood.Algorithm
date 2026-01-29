import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_WEIBULL extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    evaluate(engine, tempParameter) {
        const args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            const converted1 = args1.toNumber("Function '{0}' parameter {1} is error!", "Weibull", 1);
            if (converted1.isError) return converted1;
            args1 = converted1;
        }
        const args2 = this._arg2.evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            const converted2 = args2.toNumber("Function '{0}' parameter {1} is error!", "Weibull", 2);
            if (converted2.isError) return converted2;
            args2 = converted2;
        }
        const args3 = this._arg3.evaluate(engine, tempParameter);
        if (args3.isNotNumber) {
            const converted3 = args3.toNumber("Function '{0}' parameter {1} is error!", "Weibull", 3);
            if (converted3.isError) return converted3;
            args3 = converted3;
        }
        const args4 = this._arg4.evaluate(engine, tempParameter);
        if (args4.isNotBoolean) {
            const converted4 = args4.toBoolean("Function '{0}' parameter {1} is error!", "Weibull", 4);
            if (converted4.isError) return converted4;
            args4 = converted4;
        }
        const x = args1.doubleValue;
        const shape = args2.doubleValue;
        const scale = args3.doubleValue;
        const state = args4.booleanValue;
        if (shape <= 0.0 || scale <= 0.0) {
            return Operand.Error("Function '{0}' parameter is error!", "Weibull");
        }

        return Operand.create(ExcelFunctions.Weibull(x, shape, scale, state));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "Weibull");
    }
}

export { Function_WEIBULL };