import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';


/**
 * Function_TRIMSTART
 */
export class Function_TRIMSTART extends Function_2 {

    get Name() {
        return "TrimStart";
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

        if (this.b == null) {
            return Operand.Create(args1.TextValue.trimStart());
        }
        
        let args2 = this.GetText_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        
        let trimChars = args2.TextValue.split('');
        let text = args1.TextValue;
        let index = 0;
        
        while (index < text.length && trimChars.includes(text[index])) {
            index++;
        }
        
        return Operand.Create(text.substring(index));
    }
    

}

