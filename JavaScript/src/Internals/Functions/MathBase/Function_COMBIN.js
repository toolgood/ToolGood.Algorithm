import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_COMBIN extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Combin", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "Combin", 2);
            if (args2.IsError) { return args2; }
        }

        let total = args1.IntValue;
        let count = args2.IntValue;
        if (total < 0 || count < 0 || total < count) {
            return Operand.Error(StringCache.Function_parameter_error, "Combin");
        }
        let sum = 1;
        let sum2 = 1;
        for (let i = 0; i < count; i++) {
            sum *= (total - i);
            sum2 *= (i + 1);
        }
        return Operand.Create(sum / sum2);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Combin");
    }
}

export { Function_COMBIN };
