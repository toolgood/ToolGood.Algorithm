import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_REGEXREPALCE
 */
export class Function_REGEXREPALCE extends Function_3 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     * @param {FunctionBase} func3
     */
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter {1} is error!', 'RegexReplace', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotText) {
            args2.toText('Function \'{0}\' parameter {1} is error!', 'RegexReplace', 2);
            if (args2.isError) {
                return args2;
            }
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.isNotText) {
            args3.toText('Function \'{0}\' parameter {1} is error!', 'RegexReplace', 3);
            if (args3.isError) {
                return args3;
            }
        }
        
        try {
            const regex = new RegExp(args2.textValue, 'g');
            const b = args1.textValue.replace(regex, args3.textValue);
            return Operand.Create(b);
        } catch (e) {
            return Operand.error('Function \'RegexReplace\' is error!');
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'RegexReplace');
    }
}
