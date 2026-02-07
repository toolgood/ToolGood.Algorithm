import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with one parameter
 */
export class Function_1 extends FunctionBase {

    constructor(a) {
        super();
        this.a = a;
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
        this.a.toString2(stringBuilder, false);
        stringBuilder.push(')');
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getText_1(work, tempParameter) {
        let args1 = this.a.evaluate(work, tempParameter);
        if (args1.IsText) return args1;
        return this.convertToText(args1, 1);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getNumber_1(work, tempParameter) {
        let args1 = this.a.evaluate(work, tempParameter);
        if (args1.IsNumber) return args1;
        return this.convertToNumber(args1, 1);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getDate_1(work, tempParameter) {
        let args1 = this.a.evaluate(work, tempParameter);
        if (args1.IsDate) return args1;
        return this.convertToDate(args1, 1);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getBoolean_1(work, tempParameter) {
        let args1 = this.a.evaluate(work, tempParameter);
        if (args1.IsBoolean) return args1;
        return this.convertToBoolean(args1, 1);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getArray_1(work, tempParameter) {
        let args1 = this.a.evaluate(work, tempParameter);
        if (args1.IsArray) return args1;
        return this.convertToArray(args1, 1);
    }
    
    evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }

}

