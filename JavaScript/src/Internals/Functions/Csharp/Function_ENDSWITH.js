import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_ENDSWITH
 */
export class Function_ENDSWITH extends Function_3 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }
    
    get Name() {
        return "EndsWith";
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
        let args1 = this.GetText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.GetText_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let text = args1.TextValue;
        if (this.c === null) {
            return Operand.Create(text.endsWith(args2.TextValue));
        }

        let args3 = this.GetBoolean_3(engine, tempParameter);
        if (args3.IsError) { return args3; }

        let ignoreCase = args3.BooleanValue;
        if (ignoreCase) {
            return Operand.Create(text.toLowerCase().endsWith(args2.TextValue.toLowerCase()));
        } else {
            return Operand.Create(text.endsWith(args2.TextValue));
        }
    }
    
    
}

