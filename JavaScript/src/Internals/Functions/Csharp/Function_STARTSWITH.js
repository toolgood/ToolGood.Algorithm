import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';


/**
 * Function_STARTSWITH
 */
export class Function_STARTSWITH extends Function_3 {

    get Name() {
        return "StartsWith";
    }
    
    constructor(z) {
    super(z);
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
        
        let text = args1.TextValue;
        let prefix = args2.TextValue;
        
        if (this.c == null) {
            return Operand.Create(text.startsWith(prefix));
        }
        
        let args3 = this.getBoolean_3(engine, tempParameter);
        if (args3.IsError) { return args3; }
        
        let ignoreCase = args3.BooleanValue;
        if (ignoreCase) {
            return Operand.Create(text.toLowerCase().startsWith(prefix.toLowerCase()));
        } else {
            return Operand.Create(text.startsWith(prefix));
        }
    }
    

}

