import { Function_3 } from '../Function_3.js';

class Function_IFERROR extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsError) {
            return this.func2.Evaluate(engine, tempParameter);
        }
        return this.func3.Evaluate(engine, tempParameter);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IfError");
    }
}

export { Function_IFERROR };
