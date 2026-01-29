import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_NORMSINV extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            const converted1 = args1.toNumber("Function '{0}' parameter is error!", "NormSInv");
            if (converted1.isError) return converted1;
            args1 = converted1;
        }
        const p = args1.doubleValue;
        return Operand.Create(ExcelFunctions.NormSInv(p));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "NormSInv");
    }
}

export { Function_NORMSINV };