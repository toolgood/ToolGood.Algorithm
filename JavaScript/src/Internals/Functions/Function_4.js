import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with four parameters
 */
export class Function_4 extends FunctionBase {
    constructor(z) {
        super();
        this.a = z[0];
        this.b=null;
        this.c=null;
        this.d=null;
        if (z.length > 1){
            this.b = z[1];
            if (z.length > 2){
                this.c = z[2];
                if (z.length > 3){
                    this.d = z[3];
                }
            }
        }
    }
    Evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }
   
}

