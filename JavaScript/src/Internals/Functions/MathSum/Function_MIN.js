import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_MIN extends Function_N {
    get Name() {
        return "Min";
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

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return this.functionError();
        }
        if (list.length == 0) {
            return this.functionError();
        }

        let min = list[0];
        for (let i = 1; i < list.length; i++) {
            if (list[i] < min) {
                min = list[i];
            }
        }
        return Operand.Create(min);
    }
}

export { Function_MIN };

