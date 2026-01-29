import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with multiple parameters
 */
export class Function_N extends FunctionBase {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super();
        this.funcs = funcs;
    }

    /**
     * Adds function to string builder
     * @param {string[]} stringBuilder
     * @param {string} functionName
     */
    AddFunction(stringBuilder, functionName) {
        stringBuilder.push(functionName);
        stringBuilder.push('(');
        for(let i = 0; i < this.funcs.length; i++) {
            if(i > 0) {
                stringBuilder.push(', ');
            }
            this.funcs[i].ToString(stringBuilder, false);
        }
        stringBuilder.push(')');
    }
}
