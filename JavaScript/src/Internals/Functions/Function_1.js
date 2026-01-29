import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with one parameter
 */
export class Function_1 extends FunctionBase {
    /**
     * @param {FunctionBase} func1
     */
    constructor(func1) {
        super();
        this.func1 = func1;
    }
    Evaluate(work, tempParameter = null) {
        throw new Error('Not implemented');
    }
    /**
     * Adds function to string builder
     * @param {string[]} stringBuilder
     * @param {string} functionName
     */
    AddFunction(stringBuilder, functionName) {
        stringBuilder.push(functionName);
        stringBuilder.push('(');
        this.func1.ToString(stringBuilder, false);
        stringBuilder.push(')');
    }
}
