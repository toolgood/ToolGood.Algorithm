import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_TRIMSTART
 */
export class Function_TRIMSTART extends Function_2 {
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
            args1.toText('Function \'{0}\' parameter {1} is error!', 'TrimStart', 1);
            if (args1.isError) {
                return args1;
            }
        }
        
        if (this.func2 === null) {
            return Operand.create(args1.textValue.trimStart());
        }
        
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotText) {
            args2.toText('Function \'{0}\' parameter {1} is error!', 'TrimStart', 2);
            if (args2.isError) {
                return args2;
            }
        }
        
        const trimChars = args2.textValue.split('');
        let text = args1.textValue;
        let index = 0;
        
        while (index < text.length && trimChars.includes(text[index])) {
            index++;
        }
        
        return Operand.create(text.substring(index));
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'TrimStart');
    }
}
