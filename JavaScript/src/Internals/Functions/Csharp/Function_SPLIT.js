import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';


/**
 * Function_SPLIT
 */
export class Function_SPLIT extends Function_2 {

    get Name() {
        return "Split";
    }
    
    constructor(z) {
    super(z);
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
        let separator = args2.TextValue;
        let result = text.split(separator);
        return Operand.Create(result);
    }
    

}

