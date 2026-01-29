import { Function_2 } from '../Function_2.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_LOOKCEILING
 */
export class Function_LOOKCEILING extends Function_2 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     */
    constructor(func1, func2) {
        super(func1, func2);
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            args1.ToNumber('Function \'{0}\' parameter {1} is error!', 'LookCeiling', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotArray) {
            args2.ToArray('Function \'{0}\' parameter {1} is error!', 'LookCeiling', 2);
            if (args2.isError) {
                return args2;
            }
        }
        
        const list = [];
        FunctionUtil.f_base_GetList(args2, list);
        if (list.length === 0) {
            return Operand.error('Function \'{0}\' parameter {1} is error!', 'LookCeiling', 2);
        }
        list.sort((a, b) => b - a);
        const value = args1.numberValue;
        let result = list[0];
        if (result === value) {
            return args1;
        }
        for (let i = 1; i < list.length; i++) {
            const val = list[i];
            if (val > value) {
                result = val;
            } else if (val === value) {
                return args1;
            } else {
                break;
            }
        }
        return Operand.Create(result);
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'LookCeiling');
    }
}
