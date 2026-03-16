import { UnitFactors } from './UnitFactors.js';

class BaseUnitConverter {
    // Set Unit conversions and initial Left/Right conversion
    instantiate(conversionFactors, leftUnit, rightUnit) {
        this.units = conversionFactors;
        this.unitLeft = leftUnit;
        this.unitRight = rightUnit;
    }

    // Convert the Unit on the Left to the Unit on the Right
    leftToRight(value) {
        let startFactor = this.units.findFactor(this.unitLeft);
        let endFactor = this.units.findFactor(this.unitRight);
        let result = (value / startFactor) * endFactor;
        return result;
    }
}

export { BaseUnitConverter };