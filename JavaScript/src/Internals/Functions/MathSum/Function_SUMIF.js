import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_SUMIF extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isError) {
            return args1;
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isError) {
            return args2;
        }

        const list = [];
        const o = FunctionUtil.F_base_GetList(args1, list);
        if (o == false) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "SumIf", 1);
        }

        let sumdbs;
        if (this.func3 != null) {
            const args3 = this.func3.Evaluate(engine, tempParameter);
            if (args3.isError) {
                return args3;
            }
            sumdbs = [];
            const o2 = FunctionUtil.F_base_GetList(args3, sumdbs);
            if (o2 == false) {
                return Operand.Error("Function '{0}' parameter {1} is error!", "SumIf", 3);
            }
        } else {
            sumdbs = list;
        }

        let sum;
        if (args2.isNumber) {
            sum = FunctionUtil.F_base_countif(list, args2.numberValue) * args2.numberValue;
        } else {
            const trimmedText = args2.textValue.trim();
            const parsed = parseFloat(trimmedText);
            if (!isNaN(parsed)) {
                sum = FunctionUtil.F_base_sumif(list, parsed, sumdbs);
            } else {
                const sunif = trimmedText;
                const m2 = FunctionUtil.sumifMatch(sunif);
                if (m2 != null) {
                    sum = FunctionUtil.F_base_sumif(list, m2[0], m2[1], sumdbs);
                } else {
                    return Operand.Error("Function '{0}' parameter {1} is error!", "SumIf", 2);
                }
            }
        }
        return Operand.Create(sum);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "SumIf");
    }
}

export { Function_SUMIF };