import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_PERCENTILE extends Function_2 {
    get Name() {
        return "Percentile";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetArray_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.GetNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args1, list);
        if (o == false) { return this.ParameterError(1); }
        let k = args2.DoubleValue;
        return Operand.Create(ExcelFunctions.Percentile(list.map(q => q), k));
    }
}

export { Function_PERCENTILE };
