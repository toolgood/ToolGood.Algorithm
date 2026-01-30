import { Function_1 } from '../Function_1.js';

class Function_Percentage extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Percentage");
            if (args1.IsError) { return args1; }
        }
        return Operand.Create(args1.NumberValue / 100.0);
    }

    toString(stringBuilder, addBrackets) {
        this.func1.toString(stringBuilder, false);
        stringBuilder.append('%');
    }
}

export { Function_Percentage };
