import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with two parameters
 */
export class Function_2 extends FunctionBase {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     */
    constructor(func1, func2) {
        super();
        this.func1 = func1;
        this.func2 = func2;
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
        if(this.func2 != null) {
            stringBuilder.push(', ');
            this.func2.ToString(stringBuilder, false);
        }
        stringBuilder.push(')');
    }
}
