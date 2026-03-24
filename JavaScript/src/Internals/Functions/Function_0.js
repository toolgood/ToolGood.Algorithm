import { FunctionBase } from './FunctionBase.js';

export class Function_0 extends FunctionBase {
    constructor() {
        super();
    }

    evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }

    toString2(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, this.Name);
    }
}