import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_TDIST extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            const converted1 = args1.toNumber("Function '{0}' parameter {1} is error!", "TDist", 1);
            if (converted1.isError) return converted1;
            args1 = converted1;
        }
        const args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            const converted2 = args2.toNumber("Function '{0}' parameter {1} is error!", "TDist", 2);
            if (converted2.isError) return converted2;
            args2 = converted2;
        }
        const args3 = this.func3.evaluate(engine, tempParameter);
        if (args3.isNotNumber) {
            const converted3 = args3.toNumber("Function '{0}' parameter {1} is error!", "TDist", 3);
            if (converted3.isError) return converted3;
            args3 = converted3;
        }
        const x = args1.doubleValue;
        const degreesFreedom = args2.intValue;
        const tails = args3.intValue;
        if (degreesFreedom <= 0.0 || tails < 1 || tails > 2) {
            return Operand.Error("Function '{0}' parameter is error!", "TDist");
        }
        return Operand.create(ExcelFunctions.TDist(x, degreesFreedom, tails));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "TDist");
    }
}

export { Function_TDIST };