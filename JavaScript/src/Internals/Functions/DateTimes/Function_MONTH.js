import { Function_1 } from '../Function_1.js';
import { MyDate } from '../../MyDate.js';

class Function_MONTH extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Month");
            if (args1.IsError) { return args1; }
        }
        try {
            return Operand.Create(args1.DateValue.getMonth() + 1); // JavaScript月份从0开始，需要加1
        } catch (e) {
            return Operand.Error("Function 'Month' is error!");
        }
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Month");
    }
}

export { Function_MONTH };
