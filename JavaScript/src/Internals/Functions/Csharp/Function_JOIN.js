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
    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.funcs) {
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
            let o = FunctionUtil.f_base_GetList(args1, list);
            if (!o) {
                return Operand.Error("Function {0} parameter {1} is error!", 'Join', 1);
            }
            
            let args2 = args[1].ToText("Function {0} parameter {1} is error!", 'Join', 2);
            if (args2.IsError) {
                return args2;
            }
            
            return Operand.Create(list.join(args2.TextValue));
        } else {
            args1 = args1.ToText("Function {0} parameter {1} is error!", 'Join', 1);
            if (args1.IsError) {
                return args1;
            }
            
            let list = [];
            for (let i = 1; i < args.length; i++) {
                let item = args[i];
                if (item.IsNumber) {
                    list.push(item.NumberValue);
                } else if (item.IsText) {
                    list.push(item.TextValue);
                } else if (item.IsBoolean) {
                    list.push(item.BooleanValue);
                } else if (item.IsArray) {
                    for (let arrItem of item.ArrayValue) {
                        if (arrItem.IsNumber) {
                            list.push(arrItem.NumberValue);
                        } else if (arrItem.IsText) {
                            list.push(arrItem.TextValue);
                        } else if (arrItem.IsBoolean) {
                            list.push(arrItem.BooleanValue);
                        } else {
                            // 处理原始值
                            list.push(arrItem);
                        }
                    }
                }
            }
            
            return Operand.Create(list.join(args1.TextValue));
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Join');
    }
}
