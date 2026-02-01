import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with two parameters
 */
export class Function_2 extends FunctionBase {
 
    constructor(z) {
        super();
        this.a = z[0];
        this.b=null;
        if (z.length > 1){
            this.b = z[1];
        }
    }
    Evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }

}

