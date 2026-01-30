import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_COUNTIF extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotArray) {
            args1.ToArray('Function \'{0}\' parameter {1} is error!', 'CountIf', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.IsError) {
            return args2;
        }
        let list = [];
        let o = FunctionUtil.F_base_GetList(args1, list);
        if (o === false) {
            return Operand.Error('Function \'{0}\' parameter {1} is error!', 'CountIf', 1);
        }
        let count;
        if (args2.IsNumber) {
            count = FunctionUtil.F_base_countif(list, args2.NumberValue);
        } else {
            let trimmedText = args2.TextValue.trim();
            let parsedValue = parseFloat(trimmedText);
            if (!isNaN(parsedValue)) {
                count = FunctionUtil.F_base_countif(list, parsedValue);
            } else {
                let sunif = trimmedText;
                let m2 = FunctionUtil.sumifMatch(sunif);
                if (m2 !== null) {
                    count = FunctionUtil.F_base_countif(list, m2[0], m2[1]);
                } else {
                    return Operand.Error('Function \'{0}\' parameter {1} is error!', 'CountIf', 2);
                }
            }
        }
        return Operand.Create(count);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'CountIf');
    }
}



export { Function_COUNTIF };
