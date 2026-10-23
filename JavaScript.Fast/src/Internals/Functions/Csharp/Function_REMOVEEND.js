import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_REMOVEEND
 */
export class Function_REMOVEEND extends Function_3 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }
    
    get Name() {
        return "RemoveEnd";
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getText_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let ignoreCase = false;
        if (this.c) {
            let args3 = this.getBoolean_3(engine, tempParameter);
            if (args3.IsError) { return args3; }
            ignoreCase = args3.BooleanValue;
        }

        let text = args1.TextValue;
        let suffix = args2.TextValue;
        let endsWith = false;

        if (ignoreCase) {
            endsWith = text.toLowerCase().endsWith(suffix.toLowerCase());
        } else {
            endsWith = text.endsWith(suffix);
        }

        if (endsWith) {
            return Operand.Create(text.substring(0, text.length - suffix.length));
        }
        return args1;
    }
    

}

