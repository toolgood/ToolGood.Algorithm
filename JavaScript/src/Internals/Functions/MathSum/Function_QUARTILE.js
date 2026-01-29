import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_QUARTILE extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotArray) {
            let converted1 = args1.ToArray("Function '{0}' parameter {1} is error!", "Quartile", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            let converted2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Quartile", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args1, list);
        if (o == false) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Quartile", 1);
        }

        let quant = args2.IntValue;
        if (quant < 0 || quant > 4) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Quartile", 2);
        }
        return Operand.Create(ExcelFunctions.Quartile(list.map(q => q), quant));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Quartile");
    }
}

export { Function_QUARTILE };