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
    ToString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, this.Name);
    }
    
    /**
     * @param {Array} stringBuilder
     * @param {string} functionName
     */
    AddFunction(stringBuilder, functionName) {
        stringBuilder.push(functionName);
        stringBuilder.push('(');
        for (let i = 0; i < this.z.length; i++) {
            if (i > 0) {
                stringBuilder.push(', ');
            }
            this.z[i].ToString(stringBuilder, false);
        }
        stringBuilder.push(')');
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @param {number} idx
     * @returns {Operand}
     */
    GetText(work, tempParameter, idx) {
        let args1 = this.z[idx].Evaluate(work, tempParameter);
        if (args1.IsText) return args1;
        return this.ConvertToText(args1, idx);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @param {number} idx
     * @returns {Operand}
     */
    GetNumber(work, tempParameter, idx) {
        let args1 = this.z[idx].Evaluate(work, tempParameter);
        if (args1.IsNumber) return args1;
        return this.ConvertToNumber(args1, idx);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @param {number} idx
     * @returns {Operand}
     */
    GetDate(work, tempParameter, idx) {
        let args1 = this.z[idx].Evaluate(work, tempParameter);
        if (args1.IsDate) return args1;
        return this.ConvertToDate(args1, idx);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @param {number} idx
     * @returns {Operand}
     */
    GetBoolean(work, tempParameter, idx) {
        let args1 = this.z[idx].Evaluate(work, tempParameter);
        if (args1.IsBoolean) return args1;
        return this.ConvertToBoolean(args1, idx);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @param {number} idx
     * @returns {Operand}
     */
    GetArray(work, tempParameter, idx) {
        let args1 = this.z[idx].Evaluate(work, tempParameter);
        if (args1.IsArray) return args1;
        return this.ConvertToArray(args1, idx);
    }
    
    Evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }
}

