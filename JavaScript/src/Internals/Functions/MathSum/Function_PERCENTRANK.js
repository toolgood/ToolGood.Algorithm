import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_PERCENTRANK extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotArray) {
            const converted1 = args1.ToArray("Function '{0}' parameter {1} is error!", "PercentRank", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            const converted2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "PercentRank", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        }

        const list = [];
        const o = FunctionUtil.F_base_GetList(args1, list);
        if (o == false) {
            return Operand.Error("Function '{0}' parameter is error!", "PercentRank");
        }

        const k = args2.DoubleValue;
        const v = ExcelFunctions.PercentRank(list.map(q => q), k);
        let d = 3;
        if (this.func3 != null) {
            const args3 = this.func3.Evaluate(engine, tempParameter);
            if (args3.IsNotNumber) {
                const converted3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "PercentRank", 3);
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