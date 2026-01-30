import { Function_1 } from '../Function_1.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_MONTH extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate(StringCache.Function_parameter_error2, "Month");
            if (args1.IsError) { return args1; }
        }
        try {
            return Operand.Create(args1.DateValue.ToDateTime().getMonth() + 1); // JavaScript月份从0开始，需要加1
        } catch (e) {
            return Operand.Error(StringCache.Function_error1, "Month");
        }
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Month");
    }
}

export { Function_MONTH };
