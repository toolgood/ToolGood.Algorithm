import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_TINV extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        const args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            const converted1 = args1.toNumber("Function '{0}' parameter {1} is error!", "TInv", 1);
            if (converted1.isError) return converted1;
            args1 = converted1;
        }
        const args2 = this._arg2.evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            const converted2 = args2.toNumber("Function '{0}' parameter {1} is error!", "TInv", 2);
            if (converted2.isError) return converted2;
            args2 = converted2;
        }
        const p = args1.doubleValue;
        const degreesFreedom = args2.intValue;
        if (degreesFreedom <= 0.0 || p < 0.0 || p > 1.0) {
            return Operand.Error("Function '{0}' parameter is error!", "TInv");
        }
        return Operand.create(ExcelFunctions.TInv(p, degreesFreedom));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "TInv");
    }
}

export { Function_TINV };