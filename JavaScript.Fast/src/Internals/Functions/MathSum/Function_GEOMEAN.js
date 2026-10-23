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
        let error = this.tryEvaluateAll(engine, tempParameter, args);
        if (error != null) {
            return error;
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (o == false) {
            return this.parameterError(1);
        }
        if (list.length == 0) {
            return this.parameterError(1);
        }
        let product = 1.0;
        for (let num of list) {
            if (num <= 0) {
                return this.parameterError(1);
            }
            product *= num;
        }
        // 连乘溢出(Infinity),对齐 C# decimal OverflowException → FunctionError
        if (!isFinite(product)) {
            return this.functionError();
        }
        let geoMean = Math.pow(product, 1.0 / list.length);
        return Operand.Create(geoMean);
    }
}

export { Function_GEOMEAN };

