import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_ENDSWITH
 */
export class Function_ENDSWITH extends Function_3 {
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
        if (args1.IsNotText) {
            args1.ToText('Function \'{0}\' parameter {1} is error!', 'EndsWith', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2.ToText('Function \'{0}\' parameter {1} is error!', 'EndsWith', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const text = args1.TextValue;
        if (this.func3 === null) {
            return Operand.Create(text.endsWith(args2.TextValue));
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotBoolean) {
            args3.ToBoolean('Function \'{0}\' parameter {1} is error!', 'EndsWith', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        const ignoreCase = args3.BooleanValue;
        if (ignoreCase) {
            return Operand.Create(text.toLowerCase().endsWith(args2.TextValue.toLowerCase()));
        } else {
            return Operand.Create(text.endsWith(args2.TextValue));
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'EndsWith');
    }
}
