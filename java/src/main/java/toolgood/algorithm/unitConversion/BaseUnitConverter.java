package toolgood.algorithm.unitConversion;

import java.math.BigDecimal;

public abstract class BaseUnitConverter  {
    public String UnitLeft;
    public String UnitRight;
    protected UnitFactors Units;


    protected void Instantiate(UnitFactors conversionFactors, String leftUnit, String rightUnit) {
        Units = conversionFactors;
        UnitLeft = leftUnit;
        UnitRight = rightUnit;
    }

    public BigDecimal LeftToRight(BigDecimal value) throws UnitNotSupportedException {
        BigDecimal startFactor = Units.FindFactor(UnitLeft);
        BigDecimal endFactor = Units.FindFactor(UnitRight);
        BigDecimal result = (value.divide(startFactor)).multiply(endFactor);
        return result;
    }

}
