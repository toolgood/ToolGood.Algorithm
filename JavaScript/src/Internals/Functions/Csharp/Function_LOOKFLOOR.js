import { Function_2 } from '../Function_2.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_LOOKFLOOR
 */
export class Function_LOOKFLOOR extends Function_2 {
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
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function \'{0}\' parameter {1} is error!', 'LookFloor', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotArray) {
            args2.ToArray('Function \'{0}\' parameter {1} is error!', 'LookFloor', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        
        let list = [];
        FunctionUtil.f_base_GetList(args2, list);
        if (list.length === 0) {
            return Operand.Error('Function \'{0}\' parameter {1} is error!', 'LookFloor', 2);
        }
        list.sort((a, b) => a - b);
        let value = args1.NumberValue;
        let result = list[0];
        if (result === value) {
            return args1;
        }
        for (let i = 1; i < list.length; i++) {
            let val = list[i];
            if (val < value) {
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
        this.AddFunction(stringBuilder, 'LookFloor');
    }
}
