import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_COUNT extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(work, tempParameter) {
        let args = [];
        for (let item of this.funcs) {
            let aa = item.Evaluate(work, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o === false) {
            return Operand.Error('Function \'{0}\' parameter is error!', 'Count');
        }
        return Operand.Create(list.length);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Count');
    }
}

let FunctionUtil = {
    F_base_GetList(args, list) {
        for (let item of args) {
            if (item.IsNumber) {
                list.push(item.NumberValue);
            } else if (item.IsArray) {
                let o = this.F_base_GetList(item.ArrayValue, list);
                if (o === false) {
                    return false;
                }
            } else if (item.IsJson) {
                let i = item.ToArray(null);
                if (i.IsError) {
                    return false;
                }
                let o = this.F_base_GetList(i.ArrayValue, list);
                if (o === false) {
                    return false;
                }
            } else {
                let o = item.ToNumber(null);
                if (o.IsError) {
                    return false;
                }
                list.push(o.NumberValue);
            }
        }
        return true;
    }
};

export { Function_COUNT };
