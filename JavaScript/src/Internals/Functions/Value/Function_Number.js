import { Function_0 } from '../Function_0.js';
import { Operand } from '../../../Operand.js';
import { NumberUnitTypeHelper } from '../../NumberUnitTypeHelper.js';

class Function_Number extends Function_0 {
    constructor(d, unit) {
        super();
        this.d = d;
        this.unit = unit;
    }

    get Name() {
        return "Num";
    }

    evaluate(engine, tempParameter) {
        let dict = NumberUnitTypeHelper.getUnitTypedict();
        let d2 = NumberUnitTypeHelper.transformationUnit(this.d, dict[this.unit], engine.DistanceUnit, engine.AreaUnit, engine.VolumeUnit, engine.MassUnit);
        return Operand.Create(d2);
    }

    toString2(stringBuilder, addBrackets) {
        stringBuilder.push(this.d);
        stringBuilder.push(this.unit);
    }
}

export { Function_Number };