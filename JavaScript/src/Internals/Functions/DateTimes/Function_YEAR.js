import { Function_1 } from '../Function_1.js';
import { MyDate } from '../../MyDate.js';

class Function_YEAR extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Year");
            if (args1.IsError) { return args1; }
        }
        try {
            return engine.createOperand(args1.DateValue.getFullYear());
        } catch (e) {
            return engine.createErrorOperand("Function 'Year' is error!");
        }
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Year");
    }
}

export { Function_YEAR };
