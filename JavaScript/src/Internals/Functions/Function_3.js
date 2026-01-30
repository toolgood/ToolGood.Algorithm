import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with three parameters
 */
export class Function_3 extends FunctionBase {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     * @param {FunctionBase} func3
     */
    constructor(func1, func2, func3) {
        super();
        this.func1 = func1;
        this.func2 = func2;
        this.func3 = func3;
    }
    Evaluate(work, tempParameter = null) {
        throw new Error('Not implemented');
    }

}

