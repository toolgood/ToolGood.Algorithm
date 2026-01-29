import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_COUNT extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(work, tempParameter) {
        const args = [];
        for (const item of this.funcs) {
            const aa = item.Evaluate(work, tempParameter);
            if (aa.isError) {
                return aa;
            }
            args.push(aa);
        }

        const list = [];
        const o = FunctionUtil.F_base_GetList(args, list);
        if (o === false) {
            return Operand.error('Function \'{0}\' parameter is error!', 'Count');
        }
        return Operand.create(list.length);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Count');
    }
}

const FunctionUtil = {
    F_base_GetList(args, list) {
        for (const item of args) {
            if (item.isNumber) {
                list.push(item.numberValue);
            } else if (item.isArray) {
                const o = this.F_base_GetList(item.arrayValue, list);
                if (o === false) {
                    return false;
                }
            } else if (item.isJson) {
                const i = item.toArray(null);
                if (i.isError) {
                    return false;
                }
                const o = this.F_base_GetList(i.arrayValue, list);
                if (o === false) {
                    return false;
                }
            } else {
                const o = item.toNumber(null);
                if (o.isError) {
                    return false;
                }
                list.push(o.numberValue);
            }
        }
        return true;
    }
};

export { Function_COUNT };
