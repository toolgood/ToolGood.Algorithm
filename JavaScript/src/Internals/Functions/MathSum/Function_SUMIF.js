import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_SUMIF extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsError) {
            return args1;
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsError) {
            return args2;
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args1, list);
        if (o == false) {
            return Operand.Error("Function {0} parameter {1} is error!", "SumIf", 1);
        }

        let sumdbs;
        if (this.func3 != null) {
            let args3 = this.func3.Evaluate(engine, tempParameter);
            if (args3.IsError) {
                return args3;
            }
            sumdbs = [];
            let o2 = FunctionUtil.F_base_GetList(args3, sumdbs);
            if (o2 == false) {
                return Operand.Error("Function {0} parameter {1} is error!", "SumIf", 3);
            }
        } else {
            sumdbs = list;
        }

        let sum;
        if (args2.IsNumber) {
            sum = FunctionUtil.F_base_countif(list, args2.NumberValue) * args2.NumberValue;
        } else {
            let trimmedText = args2.TextValue.trim();
            let parsed = parseFloat(trimmedText);
            if (!isNaN(parsed)) {
                sum = FunctionUtil.F_base_sumif(list, parsed, sumdbs);
            } else {
                let sunif = trimmedText;
                let m2 = FunctionUtil.sumifMatch(sunif);
                if (m2 != null) {
                    sum = FunctionUtil.F_base_sumif(list, m2[0], m2[1], sumdbs);
                } else {
                    return Operand.Error("Function {0} parameter {1} is error!", "SumIf", 2);
                }
            }
        }
        return Operand.Create(sum);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "SumIf");
    }
}

export { Function_SUMIF };