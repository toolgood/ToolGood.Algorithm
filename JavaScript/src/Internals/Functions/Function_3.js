import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with three parameters
 */
export class Function_3 extends FunctionBase {
 
    constructor(z) {
        super();
        this.a = z[0];
        this.b=null;
        this.c=null;
        if (z.length > 1){
            this.b = z[1];
            if (z.length > 2){
                this.c = z[2];
            }
        }
    }
    Evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }

}

