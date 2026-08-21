import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_HYPGEOMDIST extends Function_4 {
    get Name() {
        return "HypgeomDist";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) return args1;

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) return args2;

        let args3 = this.getNumber_3(engine, tempParameter);
        if (args3.IsError) return args3;

        let args4 = this.getNumber_4(engine, tempParameter);
        if (args4.IsError) return args4;

        let k = args1.IntValue;
        let draws = args2.IntValue;
        let success = args3.IntValue;
        let population = args4.IntValue;
        if (k < 0) { return this.parameterError(1); }
        if (draws < 0) { return this.parameterError(2); }
        if (success < 0) { return this.parameterError(3); }
        if (population < 0) { return this.parameterError(4); }
        if (k > draws) { return this.parameterError(1); }
        if (success > population) { return this.parameterError(3); }
        if (draws > population) { return this.parameterError(2); }
        return Operand.Create(ExcelFunctions.hypgeomDist(k, draws, success, population));
    }
}

export { Function_HYPGEOMDIST };

