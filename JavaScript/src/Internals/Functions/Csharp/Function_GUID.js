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
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
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
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
}

