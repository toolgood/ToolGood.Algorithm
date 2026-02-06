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
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.Evaluate(engine, tempParameter);
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
            let o = FunctionUtil.F_base_GetList(args1, list);
            if (!o) { return this.ParameterError(1); }
            
            let args2 = this.ConvertToText(args[1], 2);
            if (args2.IsError) { return args2; }
            
            return Operand.Create(list.join(args2.TextValue));
        } else {
            args1 = this.ConvertToText(args1, 1);
            if (args1.IsError) { return args1; }
            
            let list = [];
            for (let i = 1; i < args.length; i++) {
                let o = FunctionUtil.F_base_GetList(args[i], list);
                if (!o) { return this.ParameterError(i + 1); }
            }
            
            return Operand.Create(list.join(args1.TextValue));
        }
    }
    

}

