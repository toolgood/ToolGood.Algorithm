import { Function_1 } from '../Function_1.js';

class Function_ISODD extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNumber) {
            if (args1.IntValue % 2 == 1) {
                return Operand.Create(true);
            }
        }
        return Operand.Create(false);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsOdd");
    }
}

export { Function_ISODD };
