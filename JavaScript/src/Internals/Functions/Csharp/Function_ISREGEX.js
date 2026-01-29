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
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter {1} is error!', 'IsRegex', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotText) {
            args2.toText('Function \'{0}\' parameter {1} is error!', 'IsRegex', 2);
            if (args2.isError) {
                return args2;
            }
        }
        try {
            const regex = new RegExp(args2.textValue);
            const b = regex.test(args1.textValue);
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
        this.addFunction(stringBuilder, 'IsRegex');
    }
}
