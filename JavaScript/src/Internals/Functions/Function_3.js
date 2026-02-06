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
        if (funcs.length >= 3) {
            this.c = funcs[2];
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
            if (this.c != null) {
                stringBuilder.push(', ');
                this.c.ToString(stringBuilder, false);
            }
        }
        stringBuilder.push(')');
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetText_3(work, tempParameter) {
        let args3 = this.c.Evaluate(work, tempParameter);
        if (args3.IsText) return args3;
        return this.ConvertToText(args3, 3);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetNumber_3(work, tempParameter) {
        let args3 = this.c.Evaluate(work, tempParameter);
        if (args3.IsNumber) return args3;
        return this.ConvertToNumber(args3, 3);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetDate_3(work, tempParameter) {
        let args3 = this.c.Evaluate(work, tempParameter);
        if (args3.IsDate) return args3;
        return this.ConvertToDate(args3, 3);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetBoolean_3(work, tempParameter) {
        let args3 = this.c.Evaluate(work, tempParameter);
        if (args3.IsBoolean) return args3;
        return this.ConvertToBoolean(args3, 3);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetArray_3(work, tempParameter) {
        let args3 = this.c.Evaluate(work, tempParameter);
        if (args3.IsArray) return args3;
        return this.ConvertToArray(args3, 3);
    }
    
    Evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }

}

