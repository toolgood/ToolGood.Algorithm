import { Function_N } from '../Function_N';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil';

class Function_GEOMEAN extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this.funcs) {
            const aa = item.evaluate(engine, tempParameter);
            if (aa.isError) {
                return aa;
            }
            args.push(aa);
        }

        const list = [];
        const o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.error('Function {0} parameter is error!', 'GeoMean');
        }
        if (list.length === 0) {
            return Operand.error('Function {0} parameter is error!', 'GeoMean');
        }
        let product = 1.0;
        for (const num of list) {
            if (num <= 0) {
                return Operand.error('Function {0} parameter is error!', 'GeoMean');
            }
            product *= num;
        }
        const geoMean = Math.pow(product, 1.0 / list.length);
        return Operand.create(geoMean);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'GeoMean');
    }
}

export { Function_GEOMEAN };
