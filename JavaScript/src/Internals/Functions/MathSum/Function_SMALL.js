import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_SMALL extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotArray) {
            const converted1 = args1.ToArray("Function '{0}' parameter {1} is error!", "Small", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            const converted2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Small", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        }
        const list = [];
        const o = FunctionUtil.F_base_GetList(args1, list);
        if (o == false) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Small", 1);
        }
        list.sort((a, b) => a - b);
        const k = args2.IntValue;
        if (k < 1 - engine.excelIndex || k > list.length - engine.excelIndex) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Small", 2);
        }
        return Operand.Create(list[k - engine.excelIndex]);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "Small");
    }
}

export { Function_SMALL };