import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with four parameters
 */
export class Function_4 extends FunctionBase {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     * @param {FunctionBase} func3
     * @param {FunctionBase} func4
     */
    constructor(func1, func2, func3, func4) {
        super();
        this.func1 = func1;
        this.func2 = func2;
        this.func3 = func3;
        this.func4 = func4;
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
            if(this.func3 != null) {
                stringBuilder.push(', ');
                this.func3.ToString(stringBuilder, false);
                if(this.func4 != null) {
                    stringBuilder.push(', ');
                    this.func4.ToString(stringBuilder, false);
                }
            }
        }
        stringBuilder.push(')');
    }
}
