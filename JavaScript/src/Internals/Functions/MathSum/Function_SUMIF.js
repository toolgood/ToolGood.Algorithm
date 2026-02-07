import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_SUMIF extends Function_3 {
    get Name() {
        return "SumIf";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetArray_1(engine, tempParameter); if (args1.IsError) { return args1; }
        let args2 = this.b.Evaluate(engine, tempParameter); if (args2.IsError) { return args2; }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args1, list);
        if (o == false) { return this.ParameterError(1); }

        let sumdbs;
        if (this.c != null) {
            let args3 = this.GetArray_3(engine, tempParameter); if (args3.IsError) { return args3; }
            sumdbs = [];
            let o2 = FunctionUtil.F_base_GetList(args3, sumdbs);
            if (o2 == false) { return this.ParameterError(3); }
        } else {
            sumdbs = list;
        }

        let sum;
        if (args2.IsNumber) {
            sum = FunctionUtil.F_base_countif(list, args2.NumberValue) * args2.NumberValue;
        } else {
            if (args2.IsText) {
                let trimmedText = args2.TextValue.trim();
                let parsed = parseFloat(trimmedText);
                if (!isNaN(parsed)) {
                    sum = FunctionUtil.F_base_sumif(list, parsed, sumdbs);
                } else {
                    let sunif = trimmedText;
                    let m2 = FunctionUtil.sumifMatch(sunif);
                    if (m2 != null) {
                        sum = FunctionUtil.F_base_sumif(list, m2.operator, m2.value, sumdbs);
                    } else {
                        return this.ParameterError(2);
                    }
                }
            } else {
                return this.ParameterError(2);
            }
        }
        return Operand.Create(sum);
    }
}

export { Function_SUMIF };
