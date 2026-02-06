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
        if (funcs.length >= 4) {
            this.d = funcs[3];
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
                if (this.d != null) {
                    stringBuilder.push(', ');
                    this.d.ToString(stringBuilder, false);
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
    GetText_4(work, tempParameter) {
        let args4 = this.d.Evaluate(work, tempParameter);
        if (args4.IsText) return args4;
        return this.ConvertToText(args4, 4);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetNumber_4(work, tempParameter) {
        let args4 = this.d.Evaluate(work, tempParameter);
        if (args4.IsNumber) return args4;
        return this.ConvertToNumber(args4, 4);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetDate_4(work, tempParameter) {
        let args4 = this.d.Evaluate(work, tempParameter);
        if (args4.IsDate) return args4;
        return this.ConvertToDate(args4, 4);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetBoolean_4(work, tempParameter) {
        let args4 = this.d.Evaluate(work, tempParameter);
        if (args4.IsBoolean) return args4;
        return this.ConvertToBoolean(args4, 4);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetArray_4(work, tempParameter) {
        let args4 = this.d.Evaluate(work, tempParameter);
        if (args4.IsArray) return args4;
        return this.ConvertToArray(args4, 4);
    }
    
    Evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }
   
}

