import { Function_1 } from './Function_1.js';

/**
 * Represents the base class for functions with two parameters
 */
export class Function_2 extends Function_1 {
 
    /**
     * @param {FunctionBase|Array} a
     * @param {FunctionBase} b
     */
    constructor(a, b) {
        if (Array.isArray(a)) {
            super(a[0]);
            this.b=null;
            if (a.length >= 2) {
                this.b = a[1];
            }
        } else {
            super(a);
            this.b = b;
        }
    }
    
    /**
     * @param {Array} stringBuilder
     * @param {string} functionName
     */
    addFunction(stringBuilder, functionName) {
        stringBuilder.push(functionName);
        stringBuilder.push('(');
        this.a.toString2(stringBuilder, false);
        if (this.b != null) {
            stringBuilder.push(', ');
            this.b.toString2(stringBuilder, false);
        }
        stringBuilder.push(')');
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getText_2(work, tempParameter) {
        let args2 = this.b.evaluate(work, tempParameter);
        if (args2.IsText) return args2;
        return this.convertToText(args2, 2);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getNumber_2(work, tempParameter) {
        let args2 = this.b.evaluate(work, tempParameter);
        if (args2.IsNumber) return args2;
        return this.convertToNumber(args2, 2);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getDate_2(work, tempParameter) {
        let args2 = this.b.evaluate(work, tempParameter);
        if (args2.IsDate) return args2;
        return this.convertToDate(args2, 2);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getBoolean_2(work, tempParameter) {
        let args2 = this.b.evaluate(work, tempParameter);
        if (args2.IsBoolean) return args2;
        return this.convertToBoolean(args2, 2);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getArray_2(work, tempParameter) {
        let args2 = this.b.evaluate(work, tempParameter);
        if (args2.IsArray) return args2;
        return this.convertToArray(args2, 2);
    }
    
    evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }

}

