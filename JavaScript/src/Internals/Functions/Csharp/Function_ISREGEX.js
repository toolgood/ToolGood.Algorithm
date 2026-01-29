import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_ISREGEX
 */
export class Function_ISREGEX extends Function_2 {
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
        if (args1.IsNotText) {
            args1.ToText('Function \'{0}\' parameter {1} is error!', 'IsRegex', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2.ToText('Function \'{0}\' parameter {1} is error!', 'IsRegex', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        try {
            let regex = new RegExp(args2.TextValue);
            let b = regex.test(args1.TextValue);
            return Operand.Create(b);
        } catch (e) {
            return Operand.Create(false);
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'IsRegex');
    }
}
