import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_TINV extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            let converted1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "TInv", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            let converted2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "TInv", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        }
        let p = args1.DoubleValue;
        let degreesFreedom = args2.IntValue;
        if (degreesFreedom <= 0.0 || p < 0.0 || p > 1.0) {
            return Operand.Error("Function '{0}' parameter is error!", "TInv");
        }
        return Operand.Create(ExcelFunctions.TInv(p, degreesFreedom));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "TInv");
    }
}

export { Function_TINV };