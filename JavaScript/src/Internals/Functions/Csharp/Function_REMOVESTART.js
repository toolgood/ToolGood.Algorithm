import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_REMOVESTART
 */
export class Function_REMOVESTART extends Function_3 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }
    
    get Name() {
        return "RemoveStart";
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

        let ignoreCase = false;
        if (this.c) {
            let args3 = this.GetBoolean_3(engine, tempParameter);
            if (args3.IsError) { return args3; }
            ignoreCase = args3.BooleanValue;
        }

        let text = args1.TextValue;
        let prefix = args2.TextValue;
        let startsWith = false;

        if (ignoreCase) {
            startsWith = text.toLowerCase().startsWith(prefix.toLowerCase());
        } else {
            startsWith = text.startsWith(prefix);
        }

        if (startsWith) {
            return Operand.Create(text.substring(prefix.length));
        }
        return args1;
    }
    

}

