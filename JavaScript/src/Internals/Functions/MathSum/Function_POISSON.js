import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_POISSON extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            const converted1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Poisson", 1);
            if (converted1.isError) return converted1;
            args1 = converted1;
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            const converted2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Poisson", 2);
            if (converted2.isError) return converted2;
            args2 = converted2;
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.isNotBoolean) {
            const converted3 = args3.toBoolean("Function '{0}' parameter {1} is error!", "Poisson", 3);
            if (converted3.isError) return converted3;
            args3 = converted3;
        }
        const k = args1.intValue;
        const lambda = args2.doubleValue;
        const state = args3.booleanValue;
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