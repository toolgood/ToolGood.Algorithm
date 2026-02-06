import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with one parameter
 */
export class Function_1 extends FunctionBase {
    /**
     * @param {FunctionBase} a
     */
    constructor(a) {
        super();
        this.a = a;
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
        this.a.ToString(stringBuilder, false);
        stringBuilder.push(')');
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetText_1(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsText) return args1;
        return this.ConvertToText(args1, 1);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetNumber_1(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsNumber) return args1;
        return this.ConvertToNumber(args1, 1);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetDate_1(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsDate) return args1;
        return this.ConvertToDate(args1, 1);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetBoolean_1(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsBoolean) return args1;
        return this.ConvertToBoolean(args1, 1);
    }
    
    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    GetArray_1(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsArray) return args1;
        return this.ConvertToArray(args1, 1);
    }
    
    Evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }

}

