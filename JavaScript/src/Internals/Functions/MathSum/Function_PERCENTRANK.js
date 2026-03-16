import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';

class Function_PERCENTRANK extends Function_3 {
    get Name() {
        return "PercentRank";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getArray_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args1.ArrayValue, list);
        if (o == false) { return this.functionError(); }

        let k = args2.DoubleValue;
        let v = ExcelFunctions.percentRank(list.map(q => q), k);
        let d = 3;
        if (this.c != null) {
            let args3 = this.getNumber_3(engine, tempParameter);
            if (args3.IsError) { return args3; }
            d = args3.IntValue;
        }
        return Operand.Create(Math.round(v * Math.pow(10, d)) / Math.pow(10, d));
    }
}

export { Function_PERCENTRANK };
