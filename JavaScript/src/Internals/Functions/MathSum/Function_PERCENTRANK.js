import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_PERCENTRANK extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotArray) {
            const converted1 = args1.toArray("Function '{0}' parameter {1} is error!", "PercentRank", 1);
            if (converted1.isError) return converted1;
            args1 = converted1;
        }
        const args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            const converted2 = args2.toNumber("Function '{0}' parameter {1} is error!", "PercentRank", 2);
            if (converted2.isError) return converted2;
            args2 = converted2;
        }

        const list = [];
        const o = FunctionUtil.F_base_GetList(args1, list);
        if (o == false) {
            return Operand.Error("Function '{0}' parameter is error!", "PercentRank");
        }

        const k = args2.doubleValue;
        const v = ExcelFunctions.PercentRank(list.map(q => q), k);
        let d = 3;
        if (this.func3 != null) {
            const args3 = this.func3.evaluate(engine, tempParameter);
            if (args3.isNotNumber) {
                const converted3 = args3.toNumber("Function '{0}' parameter {1} is error!", "PercentRank", 3);
                if (converted3.isError) return converted3;
                args3 = converted3;
            }
            d = args3.intValue;
        }
        return Operand.create(Math.round(v * Math.pow(10, d)) / Math.pow(10, d));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "PercentRank");
    }
}

export { Function_PERCENTRANK };