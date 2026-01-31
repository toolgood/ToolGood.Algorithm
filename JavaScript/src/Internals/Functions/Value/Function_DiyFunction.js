import { Function_N } from '../Function_N.js';

class Function_DiyFunction extends Function_N {
    constructor(name, funcs) {
        super(funcs);
        this.funName = name;
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.funcs) {
            let aa = item.Evaluate(engine, tempParameter);
            args.push(aa);
        }
        return engine.ExecuteDiyFunction(this.funName, args);
    }

}

export { Function_DiyFunction };
