import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_NORMINV extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            const converted1 = args1.toNumber("Function '{0}' parameter {1} is error!", "NormInv", 1);
            if (converted1.isError) return converted1;
            args1 = converted1;
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            const converted2 = args2.toNumber("Function '{0}' parameter {1} is error!", "NormInv", 2);
            if (converted2.isError) return converted2;
            args2 = converted2;
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.isNotNumber) {
            const converted3 = args3.toNumber("Function '{0}' parameter {1} is error!", "NormInv", 3);
            if (converted3.isError) return converted3;
            args3 = converted3;
        }
        const p = args1.doubleValue;
        const avg = args2.doubleValue;
        const STDEV = args3.doubleValue;
        return Operand.Create(ExcelFunctions.NormInv(p, avg, STDEV));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "NormInv");
    }
}

export { Function_NORMINV };