import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with two parameters
 */
export class Function_2 extends FunctionBase {
 
    constructor(funcs) {
        super();
        this.func1 = funcs[0];
        this.func2=null;
        if (funcs.length > 1){
            this.func2 = funcs[1];
        }
    }
    Evaluate(work, tempParameter = null) {
        throw new Error('Not implemented');
    }

}

