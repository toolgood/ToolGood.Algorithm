import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_AVERAGEIF extends Function_3 {
    get Name() {
        return "AverageIf";
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
        let count;
        if (args2.IsNumber) {
            count = FunctionUtil.F_base_countif(list, args2.NumberValue);
            sum = count * args2.NumberValue;
        } else {
            if (args2.IsText) {
                let TextValue = args2.TextValue.trim();
                let parsedValue = parseFloat(TextValue);
                if (!isNaN(parsedValue)) {
                    count = FunctionUtil.F_base_countif(list, parsedValue);
                    sum = FunctionUtil.F_base_sumif(list, parsedValue, sumdbs);
                } else {
                    let sunif = TextValue;
                    let m2 = FunctionUtil.sumifMatch(sunif);
                    if (m2 != null) {
                        count = FunctionUtil.F_base_countif(list, m2.operator, m2.value);
                        sum = FunctionUtil.F_base_sumif(list, m2.operator, m2.value, sumdbs);
                    } else {
                        return this.ParameterError(2);
                    }
                }
            } else {
                return this.ParameterError(2);
            }
        }
        if (count == 0) {
            return this.Div0Error();
        }
        return Operand.Create(sum / count);
    }
}

export { Function_AVERAGEIF };

