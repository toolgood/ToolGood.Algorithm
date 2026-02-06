import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_REGEX
 */
export class Function_REGEX extends Function_2 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }
    
    get Name() {
        return "Regex";
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
            let b = regex.exec(args1.TextValue);
            if (!b) {
                return this.FunctionError();
            }
            return Operand.Create(b[0]);
        } catch (e) {
            return this.FunctionError();
        }
    }
    

}

