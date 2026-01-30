import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_FLOOR extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Floor", 1);
            if (args1.IsError) { return args1; }
        }
        if (this.func2 === null) {
            return Operand.Create(Math.floor(args1.NumberValue));
        }

        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "Floor", 2);
            if (args2.IsError) { return args2; }
        }
        let b = args2.NumberValue;
        if (b >= 1) {
            return Operand.Create(args1.IntValue);
        }
        if (b <= 0) {
            return Operand.Error(StringCache.Function_parameter_error, "Floor", 2);
        }

        let a = args1.NumberValue;
        let d = Math.floor(a / b) * b;
        return Operand.Create(d);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Floor");
    }
}

export { Function_FLOOR };
