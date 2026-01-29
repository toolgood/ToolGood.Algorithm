import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_SUMSQ extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        const args = [];
        for (let i = 0; i < this._args.length; i++) {
            const aa = this._args[i].Evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        const list = [];
        const o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function '{0}' parameter is error!", "SumSQ");
        }

        let d = 0;
        for (let i = 0; i < list.length; i++) {
            const a = list[i];
            d += a * a;
        }
        return Operand.Create(d);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "SumSQ");
    }
}

export { Function_SUMSQ };