import { Function_N } from '../Function_N.js';

class Function_DiyFunction extends Function_N {
    get Name() {
        return "DiyFunction";
    }

    constructor(name, z) {
        super(z);
        this.funName = name;
    }

    evaluate(work, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.evaluate(work, tempParameter);
            args.push(aa);
        }
        return work.ExecuteDiyFunction(this.funName, args);
    }

    toString2(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, this.funName);
    }

}

export { Function_DiyFunction };
