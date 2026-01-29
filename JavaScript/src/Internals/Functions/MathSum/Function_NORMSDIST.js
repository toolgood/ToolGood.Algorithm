import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_NORMSDIST extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            const converted1 = args1.toNumber("Function '{0}' parameter is error!", "NormSDist");
            if (converted1.isError) return converted1;
            args1 = converted1;
        }
        const num = args1.doubleValue;
        return Operand.create(ExcelFunctions.NormSDist(num));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "NormSDist");
    }
}

export { Function_NORMSDIST };