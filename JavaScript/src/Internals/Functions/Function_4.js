import { Function_3 } from './Function_3.js';

/**
 * Represents the base class for functions with four parameters
 */
export class Function_4 extends Function_3 {
    /**
     * @param {Array} funcs
     */
    constructor(funcs) {
        super(funcs);
        this.d=null;
        if (funcs.length >= 4) {
            this.d = funcs[3];
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
                if (this.d != null) {
                    stringBuilder.push(', ');
                    this.d.toString2(stringBuilder, false);
                }
            }
        }
        stringBuilder.push(')');
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getText_4(work, tempParameter) {
        let args4 = this.d.evaluate(work, tempParameter);
        if (args4.IsText) return args4;
        return this.convertToText(args4, 4);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getNumber_4(work, tempParameter) {
        let args4 = this.d.evaluate(work, tempParameter);
        if (args4.IsNumber) return args4;
        return this.convertToNumber(args4, 4);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getDate_4(work, tempParameter) {
        let args4 = this.d.evaluate(work, tempParameter);
        if (args4.IsDate) return args4;
        return this.convertToDate(args4, 4);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getBoolean_4(work, tempParameter) {
        let args4 = this.d.evaluate(work, tempParameter);
        if (args4.IsBoolean) return args4;
        return this.convertToBoolean(args4, 4);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    getArray_4(work, tempParameter) {
        let args4 = this.d.evaluate(work, tempParameter);
        if (args4.IsArray) return args4;
        return this.convertToArray(args4, 4);
    }
    
    evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }
   
}

