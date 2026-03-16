import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_VARP extends Function_N {
    get Name() {
        return "VarP";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        if (args.length == 1) {
            return Operand.Error("Function '{0}' parameter only one error!", "VarP");
        }
        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return this.functionError();
        }
        if (list.length == 0) {
            return this.functionError();
        }
        if (list.length == 1) {
            return Operand.Zero;
        }

        let sum = 0;
        let avg = list.reduce((sum, val) => sum + val, 0) / list.length;
        for (let i = 0; i < list.length; i++) {
            sum += (avg - list[i]) * (avg - list[i]);
        }
        return Operand.Create(sum / list.length);
    }
}

export { Function_VARP };
