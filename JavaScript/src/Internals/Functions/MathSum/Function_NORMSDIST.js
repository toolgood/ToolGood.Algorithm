import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_NORMSDIST extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            const converted1 = args1.ToNumber("Function '{0}' parameter is error!", "NormSDist");
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        const num = args1.DoubleValue;
        return Operand.Create(ExcelFunctions.NormSDist(num));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "NormSDist");
    }
}

export { Function_NORMSDIST };