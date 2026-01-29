import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_POISSON extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            const converted1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Poisson", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            const converted2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Poisson", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotBoolean) {
            const converted3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "Poisson", 3);
            if (converted3.IsError) return converted3;
            args3 = converted3;
        }
        const k = args1.IntValue;
        const lambda = args2.DoubleValue;
        const state = args3.BooleanValue;
        if (!(lambda > 0.0)) {
            return Operand.Error("Function '{0}' parameter is error!", "Poisson");
        }
        return Operand.Create(ExcelFunctions.Poisson(k, lambda, state));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "Poisson");
    }
}

export { Function_POISSON };