import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_ISREGEX
 */
export class Function_ISREGEX extends Function_2 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }
    
    get Name() {
        return "IsRegex";
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

        try {
            let regex = new RegExp(args2.TextValue);
            let b = regex.test(args1.TextValue);
            return Operand.Create(b);
        } catch (e) {
            return Operand.Create(false);
        }
    }
    

}

