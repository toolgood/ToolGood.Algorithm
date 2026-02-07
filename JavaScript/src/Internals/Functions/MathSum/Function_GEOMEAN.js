import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_GEOMEAN extends Function_N {
    get Name() {
        return "GeoMean";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return this.functionError();
        }
        if (list.length == 0) {
            return this.functionError();
        }
        let product = 1.0;
        for (let num of list) {
            if (num <= 0) {
                return this.functionError();
            }
            product *= num;
        }
        let geoMean = Math.pow(product, 1.0 / list.length);
        return Operand.Create(geoMean);
    }
}

export { Function_GEOMEAN };

