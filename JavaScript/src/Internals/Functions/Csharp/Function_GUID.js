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
    evaluate(engine) {
        return Operand.create(this.generateGuid());
    }
    
    /**
     * 生成GUID
     * @returns {string}
     */
    generateGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            const r = Math.random() * 16 | 0;
            const v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        stringBuilder.push('GUID()');
    }
}
