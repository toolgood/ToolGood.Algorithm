import { Function_N } from '../Function_N';

class Function_DiyFunction extends Function_N {
    constructor(name, funcs) {
        super(funcs);
        this.funName = name;
    }

    evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this.funcs) {
            const aa = item.evaluate(engine, tempParameter);
            args.push(aa);
        }
        return engine.executeDiyFunction(this.funName, args);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, this.funName);
    }
}

export { Function_DiyFunction };
