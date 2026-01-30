import { Function_3 } from '../Function_3.js';
import { MyDate } from '../../MyDate.js';
import { StringCache } from '../../../Internals/StringCache.js';
import { Operand } from '../../../Operand.js';


class Function_TIME extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Time", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "Time", 2);
            if (args2.IsError) { return args2; }
        }

        let d;
        if (this.func3 !== null) {
            let args3 = this.func3.Evaluate(engine, tempParameter);
            if (args3.IsNotNumber) {
                args3 = args3.ToNumber(StringCache.Function_parameter_error, "Time", 3);
                if (args3.IsError) { return args3; }
            }
            d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, args3.IntValue);
        } else {
            d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, 0);
        }
        return Operand.Create(d);
    }
}

export { Function_TIME };

