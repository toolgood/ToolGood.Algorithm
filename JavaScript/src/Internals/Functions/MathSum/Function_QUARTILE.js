import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_QUARTILE extends Function_2 {
    get Name() {
        return "Quartile";
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

        let quant = args2.IntValue;
        if (quant < 0 || quant > 4) {
            return this.ParameterError(2);
        }
        return Operand.Create(ExcelFunctions.Quartile(list, quant));
    }
}

export { Function_QUARTILE };
