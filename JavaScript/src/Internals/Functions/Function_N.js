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
    Evaluate(work, tempParameter = null) {
        throw new Error('Not implemented');
    }
}

