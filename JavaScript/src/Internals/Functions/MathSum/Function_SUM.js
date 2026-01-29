import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_SUM extends Function_N {
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

        const list = [];
        const o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function '{0}' parameter is error!", "Sum");
        }
        return Operand.create(list.reduce((sum, val) => sum + val, 0));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, "Sum");
    }
}

export { Function_SUM };