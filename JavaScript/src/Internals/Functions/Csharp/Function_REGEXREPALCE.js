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
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1.ToText('Function \'{0}\' parameter {1} is error!', 'RegexReplace', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2.ToText('Function \'{0}\' parameter {1} is error!', 'RegexReplace', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotText) {
            args3.ToText('Function \'{0}\' parameter {1} is error!', 'RegexReplace', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        
        try {
            let regex = new RegExp(args2.TextValue, 'g');
            let b = args1.TextValue.replace(regex, args3.TextValue);
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
        this.AddFunction(stringBuilder, 'RegexReplace');
    }
}
