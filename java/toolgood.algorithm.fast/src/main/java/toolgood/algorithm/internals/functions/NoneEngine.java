package toolgood.algorithm.internals.functions;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;

public class NoneEngine extends AlgorithmEngine {
    public NoneEngine(AlgorithmEngine engine) {
        super(engine.IgnoreCase);
        this.AreaUnit = engine.AreaUnit;
        this.DistanceUnit = engine.DistanceUnit;
        this.LastError = engine.LastError;
        this.MassUnit = engine.MassUnit;
        this.UseLocalTime = engine.UseLocalTime;
        this.ExcelIndex = engine.ExcelIndex;
    }

    @Override
    public Operand GetParameter(String parameter) {
        return Operand.Null;
    }

    @Override
    public Operand ExecuteDiyFunction(String parameter, List<Operand> args) {
        return Operand.Null;
    }
}
