import { Function_1 } from '../Function_1.js';
import { MyDate } from '../../MyDate.js';

class Function_HOUR extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Hour");
            if (args1.IsError) { return args1; }
        }
        try {
            return engine.createOperand(args1.DateValue.getHours());
        } catch (e) {
            return engine.createErrorOperand("Function 'Hour' is error!");
        }
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Hour");
    }
}

export { Function_HOUR };
