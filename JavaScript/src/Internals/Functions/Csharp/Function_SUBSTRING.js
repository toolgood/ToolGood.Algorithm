import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_SUBSTRING
 */
export class Function_SUBSTRING extends Function_3 {

    get Name() {
        return "Substring";
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

        let args2 = this.GetNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        
        let text = args1.TextValue;
        let startIndex = args2.IntValue - engine.ExcelIndex;
        
        if (this.c === null) {
            return Operand.Create(text.substring(startIndex));
        }
        
        let args3 = this.GetNumber_3(engine, tempParameter);
        if (args3.IsError) { return args3; }
        
        let length = args3.IntValue;
        let endIndex = startIndex + length;
        return Operand.Create(text.substring(startIndex, endIndex));
    }
    

}

