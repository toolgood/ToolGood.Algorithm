import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_VAR extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this._args) {
            const aa = item.evaluate(engine, tempParameter);
            if (aa.isError) {
                return aa;
            }
            args.push(aa);
        }

        if (args.length == 1) {
            return Operand.Error("Function '{0}}' parameter only one error!", "Var");
        }

        const list = [];
        const o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function '{0}' parameter is error!", "Var");
        }
        if (list.length <= 1) {
            return Operand.Error("Function '{0}' parameter is error!", "Var");
        }
        let sum = 0;
        let sum2 = 0;
        for (let i = 0; i < list.length; i++) {
            sum += list[i] * list[i];
            sum2 += list[i];
        }
        return Operand.create((list.length * sum - sum2 * sum2) / list.length / (list.length - 1));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "Var");
    }
}

export { Function_VAR };