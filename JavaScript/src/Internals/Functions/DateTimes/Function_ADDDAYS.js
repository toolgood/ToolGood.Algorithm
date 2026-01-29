import { Function_2 } from '../Function_2';
import { MyDate } from '../../MyDate';

class Function_ADDDAYS extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "AddDays", 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this._arg2.evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "AddDays", 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const date = new Date(args1.DateValue.getTime());
        date.setDate(date.getDate() + args2.DoubleValue);
        return engine.createOperand(new MyDate(date));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "AddDays");
    }
}

export { Function_ADDDAYS };
