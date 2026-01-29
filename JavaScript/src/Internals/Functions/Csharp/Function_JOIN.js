import { Function_N } from '../Function_N.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_JOIN
 */
export class Function_JOIN extends Function_N {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    evaluate(engine) {
        const args = [];
        for (let item of this._args) {
            const aa = item.evaluate(engine);
            if (aa.isError) {
                return aa;
            }
            args.push(aa);
        }
        
        let args1 = args[0];
        if (args1.isJson) {
            const o = args1.toArray(null);
            if (!o.isError) {
                args1 = o;
            }
        }
        
        if (args1.isArray) {
            const list = [];
            const o = FunctionUtil.f_base_GetList(args1, list);
            if (!o) {
                return Operand.error('Function \'{0}\' parameter {1} is error!', 'Join', 1);
            }
            
            const args2 = args[1].toText('Function \'{0}\' parameter {1} is error!', 'Join', 2);
            if (args2.isError) {
                return args2;
            }
            
            return Operand.create(list.join(args2.textValue));
        } else {
            args1 = args1.toText('Function \'{0}\' parameter {1} is error!', 'Join', 1);
            if (args1.isError) {
                return args1;
            }
            
            const list = [];
            for (let i = 1; i < args.length; i++) {
                const o = FunctionUtil.f_base_GetList(args[i], list);
                if (!o) {
                    return Operand.error('Function \'{0}\' parameter {1} is error!', 'Join', i + 1);
                }
            }
            
            return Operand.create(list.join(args1.textValue));
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Join');
    }
}
