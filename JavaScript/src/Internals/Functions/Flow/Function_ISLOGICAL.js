import { Function_1 } from '../Function_1.js';

class Function_ISLOGICAL extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsBoolean) {
            return Operand.Create(true);
        }
        return Operand.Create(false);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsLogical");
    }
}

export { Function_ISLOGICAL };
