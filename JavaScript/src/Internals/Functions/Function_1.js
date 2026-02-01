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
    Evaluate(work, tempParameter = null) {
        throw new Error('Not implemented');
    }

}

