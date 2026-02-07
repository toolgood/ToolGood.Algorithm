import { Function_2 } from './Function_2.js';

/**
 * Represents the base class for functions with three parameters
 */
export class Function_3 extends Function_2 {
 
    /**
     * @param {Array} funcs
     */
    constructor(funcs) {
        super(funcs);
        this.c=null;
        if (funcs.length >= 3) {
            this.c = funcs[2];
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
            if (this.c != null) {
                stringBuilder.push(', ');
                this.c.toString2(stringBuilder, false);
            }
        }
        stringBuilder.push(')');
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getText_3(work, tempParameter) {
        let args3 = this.c.evaluate(work, tempParameter);
        if (args3.IsText) return args3;
        return this.convertToText(args3, 3);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getNumber_3(work, tempParameter) {
        let args3 = this.c.evaluate(work, tempParameter);
        if (args3.IsNumber) return args3;
        return this.convertToNumber(args3, 3);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getDate_3(work, tempParameter) {
        let args3 = this.c.evaluate(work, tempParameter);
        if (args3.IsDate) return args3;
        return this.convertToDate(args3, 3);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getBoolean_3(work, tempParameter) {
        let args3 = this.c.evaluate(work, tempParameter);
        if (args3.IsBoolean) return args3;
        return this.convertToBoolean(args3, 3);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getArray_3(work, tempParameter) {
        let args3 = this.c.evaluate(work, tempParameter);
        if (args3.IsArray) return args3;
        return this.convertToArray(args3, 3);
    }
    
    evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }

}

