package toolgood.algorithm.internals.functions;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;

/**
 * 参数类型推断引擎：getParameter 返回 Operand.None，executeDiyFunction 返回 Operand.None
 */
public class NoneEngine extends AlgorithmEngine {

    public NoneEngine(AlgorithmEngine engine) {
        this.AreaUnit = engine.AreaUnit;
        this.DistanceUnit = engine.DistanceUnit;
        this.SetLastError(engine.getLastError());
        this.MassUnit = engine.MassUnit;
        this.UseLocalTime = engine.UseLocalTime;
        this.ExcelIndex = engine.ExcelIndex;
    }

    @Override
    public Operand getParameter(String parameter) {
        return Operand.None;
    }

    @Override
    public Operand executeDiyFunction(String parameter, List<Operand> args) {
        return Operand.None;
    }
}
