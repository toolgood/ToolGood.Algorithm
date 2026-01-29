import { Function_3 } from '../Function_3.js';

class Function_IFERROR extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.IsError) {
            return this.func2.evaluate(engine, tempParameter);
        }
        return this.func3.evaluate(engine, tempParameter);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IfError");
    }
}

export { Function_IFERROR };
