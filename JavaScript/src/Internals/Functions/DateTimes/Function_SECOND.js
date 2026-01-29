import { Function_1 } from '../Function_1.js';
import { MyDate } from '../../MyDate.js';

class Function_SECOND extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Second");
            if (args1.IsError) { return args1; }
        }
        try {
            return engine.createOperand(args1.DateValue.getSeconds());
        } catch (e) {
            return engine.createErrorOperand("Function 'Second' is error!");
        }
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Second");
    }
}

export { Function_SECOND };
