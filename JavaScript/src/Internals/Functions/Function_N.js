import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with multiple parameters
 */
export class Function_N extends FunctionBase {
    /**
     * @param {FunctionBase[]} z
     */
    constructor(z) {
        super();
        this.z = z;
    }
    Evaluate(work, tempParameter = null) {
        throw new Error('Not implemented');
    }
}

