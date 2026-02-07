import { FunctionBase } from '../FunctionBase.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_GUID
 */
export class Function_GUID extends FunctionBase {
    /**
     * @constructor
     */
    constructor() {
        super();
    }
    
    get Name() {
        return "Guid";
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    evaluate(engine, tempParameter) {
        return Operand.Create(this.generateGuid());
    }
    
    /**
     * 生成GUID
     * @returns {string}
     */
    generateGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            let r = Math.random() * 16 | 0;
            let v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
    

    toString2(stringBuilder, addBrackets) {
        stringBuilder.push("GUID()");
    }
}

