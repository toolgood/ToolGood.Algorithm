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
    AddFunction(stringBuilder, functionName) {
        stringBuilder.push(functionName);
        stringBuilder.push('(');
        this.a.ToString(stringBuilder, false);
        if (this.b != null) {
            stringBuilder.push(', ');
            this.b.ToString(stringBuilder, false);
        }
        stringBuilder.push(')');
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetText_2(work, tempParameter) {
        let args2 = this.b.Evaluate(work, tempParameter);
        if (args2.IsText) return args2;
        return this.ConvertToText(args2, 2);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetNumber_2(work, tempParameter) {
        let args2 = this.b.Evaluate(work, tempParameter);
        if (args2.IsNumber) return args2;
        return this.ConvertToNumber(args2, 2);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetDate_2(work, tempParameter) {
        let args2 = this.b.Evaluate(work, tempParameter);
        if (args2.IsDate) return args2;
        return this.ConvertToDate(args2, 2);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetBoolean_2(work, tempParameter) {
        let args2 = this.b.Evaluate(work, tempParameter);
        if (args2.IsBoolean) return args2;
        return this.ConvertToBoolean(args2, 2);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetArray_2(work, tempParameter) {
        let args2 = this.b.Evaluate(work, tempParameter);
        if (args2.IsArray) return args2;
        return this.ConvertToArray(args2, 2);
    }
    
    Evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }

}

