import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_REGEXREPALCE
 */
export class Function_REGEXREPALCE extends Function_3 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }
    
    get Name() {
        return "RegexReplace";
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

        let args3 = this.getText_3(engine, tempParameter);
        if (args3.IsError) { return args3; }

        try {
            let regex = new RegExp(args2.TextValue, 'g');
            let b = args1.TextValue.replace(regex, args3.TextValue);
            return Operand.Create(b);
        } catch (e) {
            return this.functionError();
        }
    }
    

}

