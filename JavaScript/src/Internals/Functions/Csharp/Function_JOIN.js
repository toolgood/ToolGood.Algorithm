import { Function_N } from '../Function_N.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_JOIN
 */
export class Function_JOIN extends Function_N {
    /**
     * @param {FunctionBase[]} z
     */
    constructor(z) {
        super(z);
    }
    
    get Name() {
        return "Join";
    }
    F_base_GetList(item, list){
            if(item.IsError){
                return false;
            } else if(item.IsArray) {
                for (let i = 0; i < item.ArrayValue.length; i++) {
                    let o = this.F_base_GetList(item.ArrayValue[i], list);
                    if (!o) { return false}
                }
            } else if(item.IsJson) {
                var array = item.ToArray(null);
                if(array.IsError) { return false; }
                for (let i = 0; i < item.ArrayValue.length; i++) {
                    let o = this.F_base_GetList(item.ArrayValue[i], list);
                    if (!o) { return false }
                }
            } else if(item.IsNull) {
            } else {
                var o = item.ToText(null);
                if(o.IsError) { return false; }
                list.push(o.TextValue);
            }
        return true;
    }
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }
        
        let args1 = args[0];
        if (args1.IsJson) {
            let o = args1.ToArray(null);
            if (!o.IsError) {
                args1 = o;
            }
        }
        
        if (args1.IsArray) {
            let list = [];
            let o = this.F_base_GetList(args1, list);
            if (!o) { return this.parameterError(1); }
            
            let args2 = this.convertToText(args[1], 2);
            if (args2.IsError) { return args2; }
            
            return Operand.Create(list.join(args2.TextValue));
        } else {
            args1 = this.convertToText(args1, 1);
            if (args1.IsError) { return args1; }
            
            let list = [];
            for (let i = 1; i < args.length; i++) {
                let o = this.F_base_GetList(args[i], list);
                if (!o) { return this.parameterError(i + 1); }
            }
            
            return Operand.Create(list.join(args1.TextValue));
        }
    }

}

