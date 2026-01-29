import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_NORMINV extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            const converted1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "NormInv", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            const converted2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "NormInv", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            const converted3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "NormInv", 3);
            if (converted3.IsError) return converted3;
            args3 = converted3;
        }
        const p = args1.DoubleValue;
        const avg = args2.DoubleValue;
        const STDEV = args3.DoubleValue;
        return Operand.Create(ExcelFunctions.NormInv(p, avg, STDEV));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "NormInv");
    }
}

export { Function_NORMINV };