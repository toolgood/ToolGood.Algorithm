import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_TRIMEND
 */
export class Function_TRIMEND extends Function_2 {
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
            args1.toText('Function \'{0}\' parameter {1} is error!', 'TrimEnd', 1);
            if (args1.isError) {
                return args1;
            }
        }
        
        if (this.func2 === null) {
            return Operand.Create(args1.textValue.trimEnd());
        }
        
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotText) {
            args2.toText('Function \'{0}\' parameter {1} is error!', 'TrimEnd', 2);
            if (args2.isError) {
                return args2;
            }
        }
        
        const trimChars = args2.textValue.split('');
        let text = args1.textValue;
        let index = text.length - 1;
        
        while (index >= 0 && trimChars.includes(text[index])) {
            index--;
        }
        
        return Operand.Create(text.substring(0, index + 1));
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'TrimEnd');
    }
}
