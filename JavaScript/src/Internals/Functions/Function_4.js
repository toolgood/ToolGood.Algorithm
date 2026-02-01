import { FunctionBase } from './FunctionBase.js';

/**
 * Represents the base class for functions with four parameters
 */
export class Function_4 extends FunctionBase {
    constructor(funcs) {
        super();
        this.func1 = funcs[0];
        this.func2=null;
        this.func3=null;
        this.func4=null;
        if (funcs.length > 1){
            this.func2 = funcs[1];
            if (funcs.length > 2){
                this.func3 = funcs[2];
                if (funcs.length > 3){
                    this.func4 = funcs[3];
                }
            }
        }
    }
    Evaluate(work, tempParameter = null) {
        throw new Error('Not implemented');
    }
   
}

