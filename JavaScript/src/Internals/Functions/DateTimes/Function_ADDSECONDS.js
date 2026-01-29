import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';

class Function_ADDSECONDS extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "AddSeconds", 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "AddSeconds", 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const date = new Date(args1.DateValue.getTime());
        date.setSeconds(date.getSeconds() + args2.IntValue);
        return engine.createOperand(new MyDate(date));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "AddSeconds");
    }
}

export { Function_ADDSECONDS };
