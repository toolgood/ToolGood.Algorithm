import { Function_2 } from '../Function_2.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_LOOKCEILING
 */
export class Function_LOOKCEILING extends Function_2 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     */
    constructor(funcs) {
    super(funcs);
  }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'LookCeiling', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotArray) {
            args2 = args2.ToArray(StringCache.Function_parameter_error, 'LookCeiling', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        
        let list = [];
        FunctionUtil.F_base_GetList(args2.ArrayValue, list);
        if (list.length === 0) {
            return Operand.Error(StringCache.Function_parameter_error, 'LookCeiling', 2);
        }
        list.sort((a, b) => a - b);
        let value = args1.NumberValue;
        let result = list[list.length - 1];
        if (result === value) {
            return Operand.Create(result);
        }
        for (let i = 0; i < list.length; i++) {
            let val = list[i];
            if (val >= value) {
                return Operand.Create(val);
            }
        }
        return Operand.Create(result);
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
}

