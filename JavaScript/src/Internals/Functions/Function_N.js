import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with multiple parameters
 */
export class Function_N extends FunctionBase {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super();
        this.z = funcs;
    }
    
    /**
     * @param {Array} stringBuilder
     * @param {boolean} addBrackets
     */
    toString2(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, this.Name);
    }
    
    /**
     * @param {Array} stringBuilder
     * @param {string} functionName
     */
    addFunction(stringBuilder, functionName) {
        stringBuilder.push(functionName);
        stringBuilder.push('(');
        for (let i = 0; i < this.z.length; i++) {
            if (i > 0) {
                stringBuilder.push(', ');
            }
            this.z[i].toString2(stringBuilder, false);
        }
        stringBuilder.push(')');
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @param {number} idx
     * @returns {Operand}
     */
    getText(work, tempParameter, idx) {
        let args1 = this.z[idx].evaluate(work, tempParameter);
        if (args1.IsText) return args1;
        return this.convertToText(args1, idx);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @param {number} idx
     * @returns {Operand}
     */
    getNumber(work, tempParameter, idx) {
        let args1 = this.z[idx].evaluate(work, tempParameter);
        if (args1.IsNumber) return args1;
        return this.convertToNumber(args1, idx);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @param {number} idx
     * @returns {Operand}
     */
    getDate(work, tempParameter, idx) {
        let args1 = this.z[idx].evaluate(work, tempParameter);
        if (args1.IsDate) return args1;
        return this.convertToDate(args1, idx);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @param {number} idx
     * @returns {Operand}
     */
    getBoolean(work, tempParameter, idx) {
        let args1 = this.z[idx].evaluate(work, tempParameter);
        if (args1.IsBoolean) return args1;
        return this.convertToBoolean(args1, idx);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @param {number} idx
     * @returns {Operand}
     */
    getArray(work, tempParameter, idx) {
        let args1 = this.z[idx].evaluate(work, tempParameter);
        if (args1.IsArray) return args1;
        return this.convertToArray(args1, idx);
    }
    
    evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }
}

