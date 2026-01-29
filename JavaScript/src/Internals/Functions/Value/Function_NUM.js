import { FunctionBase } from '../FunctionBase';
import { Operand } from '../../Operand';

class Function_NUM extends FunctionBase {
    constructor(func1, func2) {
        super();
        this.d = func1;
        this.unit = func2;
    }

    evaluate(engine, tempParameter) {
        const dict = NumberUnitTypeHelper.getUnitTypedict();
        const d2 = NumberUnitTypeHelper.transformationUnit(this.d, dict[this.unit], engine.distanceUnit, engine.areaUnit, engine.volumeUnit, engine.massUnit);
        return Operand.create(d2);
    }

    toString(stringBuilder, addBrackets) {
        stringBuilder.append(this.d);
        stringBuilder.append(this.unit);
    }
}

export { Function_NUM };
