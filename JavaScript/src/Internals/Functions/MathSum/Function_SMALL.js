import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_SMALL extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotArray) {
            const converted1 = args1.toArray("Function '{0}' parameter {1} is error!", "Small", 1);
            if (converted1.isError) return converted1;
            args1 = converted1;
        }
        const args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            const converted2 = args2.toNumber("Function '{0}' parameter {1} is error!", "Small", 2);
            if (converted2.isError) return converted2;
            args2 = converted2;
        }
        const list = [];
        const o = FunctionUtil.F_base_GetList(args1, list);
        if (o == false) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Small", 1);
        }
        list.sort((a, b) => a - b);
        const k = args2.intValue;
        if (k < 1 - engine.excelIndex || k > list.length - engine.excelIndex) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Small", 2);
        }
        return Operand.create(list[k - engine.excelIndex]);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "Small");
    }
}

export { Function_SMALL };