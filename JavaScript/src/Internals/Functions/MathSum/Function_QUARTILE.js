import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_QUARTILE extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotArray) {
            const converted1 = args1.toArray("Function '{0}' parameter {1} is error!", "Quartile", 1);
            if (converted1.isError) return converted1;
            args1 = converted1;
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            const converted2 = args2.toNumber("Function '{0}' parameter {1} is error!", "Quartile", 2);
            if (converted2.isError) return converted2;
            args2 = converted2;
        }

        const list = [];
        const o = FunctionUtil.F_base_GetList(args1, list);
        if (o == false) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Quartile", 1);
        }

        const quant = args2.intValue;
        if (quant < 0 || quant > 4) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Quartile", 2);
        }
        return Operand.create(ExcelFunctions.Quartile(list.map(q => q), quant));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "Quartile");
    }
}

export { Function_QUARTILE };