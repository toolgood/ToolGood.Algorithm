package toolgood.algorithm.internals.functions;

import java.util.List;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.MyParameter;

class NoneEngine extends AlgorithmEngine {
    public NoneEngine(AlgorithmEngine engine) {
        super(engine.IgnoreCase);
        this.AreaUnit = engine.AreaUnit;
        this.DistanceUnit = engine.DistanceUnit;
        this.LastError = engine.LastError;
        this.MassUnit = engine.MassUnit;
        this.UseLocalTime = engine.UseLocalTime;
        this.UseExcelIndex = engine.UseExcelIndex;
    }

    @Override
    protected Operand GetParameter(MyParameter parameter) {
        return Operand.CreateNull();
    }

    @Override
    protected Operand ExecuteDiyFunction(String parameter, List<Operand> args) {
        return Operand.CreateNull();
    }
}
