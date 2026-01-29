import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_VARP extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this._args) {
            const aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        if (args.length == 1) {
            return Operand.Error("Function '{0}}' parameter only one error!", "VarP");
        }
        const list = [];
        const o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function '{0}' parameter is error!", "VarP");
        }
        if (list.length == 0) {
            return Operand.Error("Function '{0}' parameter is error!", "VarP");
        }
        if (list.length == 1) {
            return Operand.Zero;
        }

        let sum = 0;
        const avg = list.reduce((sum, val) => sum + val, 0) / list.length;
        for (let i = 0; i < list.length; i++) {
            sum += (avg - list[i]) * (avg - list[i]);
        }
        return Operand.Create(sum / list.length);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "VarP");
    }
}

export { Function_VARP };