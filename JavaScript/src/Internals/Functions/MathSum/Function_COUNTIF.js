import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_COUNTIF extends Function_2 {
    get Name() {
        return "CountIf";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getArray_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.b.evaluate(work, tempParameter);
        if (args2.IsError) { return args2; }

        let list = [];
        let o = FunctionUtil.FlattenToList(args1, list);
        if (o == false) { return this.parameterError(1); }

        let count;
        if (args2.IsNumber) {
            count = FunctionUtil.GetCountIf(list, args2.NumberValue);
        } else {
            let trimmedText = args2.TextValue.trim();
            let parsedValue = parseFloat(trimmedText);
            if (!isNaN(parsedValue)) {
                count = FunctionUtil.GetCountIf(list, parsedValue);
            } else {
                let sunif = trimmedText;
                let m2 = FunctionUtil.ParseSumIfMatch(sunif);
                if (m2 != null) {
                    count = FunctionUtil.GetCountIf(list, m2.operator, m2.value);
                } else {
                    return this.parameterError(2);
                }
            }
        }
        return Operand.Create(count);
    }
}




export { Function_COUNTIF };