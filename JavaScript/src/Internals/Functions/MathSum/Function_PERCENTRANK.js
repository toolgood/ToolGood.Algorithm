import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_PERCENTRANK extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotArray) {
            let converted1 = args1.ToArray(StringCache.Function_parameter_error2, "PercentRank", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            let converted2 = args2.ToNumber(StringCache.Function_parameter_error2, "PercentRank", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args1, list);
        if (o == false) {
            return Operand.Error(StringCache.Function_parameter_error2, "PercentRank");
        }

        let k = args2.DoubleValue;
        let v = ExcelFunctions.PercentRank(list.map(q => q), k);
        let d = 3;
        if (this.func3 != null) {
            let args3 = this.func3.Evaluate(engine, tempParameter);
            if (args3.IsNotNumber) {
                let converted3 = args3.ToNumber(StringCache.Function_parameter_error2, "PercentRank", 3);
                if (converted3.IsError) return converted3;
                args3 = converted3;
            }
            d = args3.IntValue;
        }
        return Operand.Create(Math.round(v * Math.pow(10, d)) / Math.pow(10, d));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "PercentRank");
    }
}

export { Function_PERCENTRANK };