import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_STDEV extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.funcs) {
            let aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function '{0}' parameter is error!", "Stdev");
        }
        if (list.length == 0) {
            return Operand.Error("Function '{0}' parameter is error!", "Stdev");
        }

        let avg = list.reduce((sum, val) => sum + val, 0) / list.length;
        let sum = 0;
        for (let i = 0; i < list.length; i++) {
            sum += (list[i] - avg) * (list[i] - avg);
        }
        return Operand.Create(Math.sqrt(sum / (list.length - 1)));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Stdev");
    }
}

export { Function_STDEV };