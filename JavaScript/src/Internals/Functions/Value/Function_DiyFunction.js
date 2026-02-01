import { Function_N } from '../Function_N.js';

class Function_DiyFunction extends Function_N {
    constructor(name, z) {
        super(z);
        this.funName = name;
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.Evaluate(engine, tempParameter);
            args.push(aa);
        }
        return engine.ExecuteDiyFunction(this.funName, args);
    }

}

export { Function_DiyFunction };
