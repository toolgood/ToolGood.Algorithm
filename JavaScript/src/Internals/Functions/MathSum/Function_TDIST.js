import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_TDIST extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            let converted1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "TDist", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            let converted2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "TDist", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            let converted3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "TDist", 3);
            if (converted3.IsError) return converted3;
            args3 = converted3;
        }
        let x = args1.DoubleValue;
        let degreesFreedom = args2.IntValue;
        let tails = args3.IntValue;
        if (degreesFreedom <= 0.0 || tails < 1 || tails > 2) {
            return Operand.Error("Function '{0}' parameter is error!", "TDist");
        }
        return Operand.Create(ExcelFunctions.TDist(x, degreesFreedom, tails));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "TDist");
    }
}

export { Function_TDIST };