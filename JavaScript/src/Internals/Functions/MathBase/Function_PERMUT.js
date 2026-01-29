import { Function_2 } from '../Function_2';

class Function_PERMUT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Permut", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this._arg2.evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Permut", 2);
            if (args2.IsError) { return args2; }
        }

        const total = args1.IntValue;
        const count = args2.IntValue;

        let sum = 1;
        for (let i = 0; i < count; i++) {
            sum *= (total - i);
        }
        return engine.createOperand(sum);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Permut");
    }
}

export { Function_PERMUT };
