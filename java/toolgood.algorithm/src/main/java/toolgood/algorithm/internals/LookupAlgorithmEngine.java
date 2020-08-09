package toolgood.algorithm.internals;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.litJson.JsonData;

public class LookupAlgorithmEngine extends AlgorithmEngine {
    public Operand Json;

    @Override
    protected Operand GetParameter(String parameter)
    {
        JsonData v = Json.JsonValue() [parameter];
        if (v!=null)
        {
            if (v.IsString()) return Operand.Create(v.StringValue);
            if (v.IsBoolean()) return Operand.Create(v.BooleanValue);
            if (v.IsDouble()) return Operand.Create(v.NumberValue);
            if (v.IsObject()) return Operand.Create(v);
            if (v.IsArray()) return Operand.Create(v);
            if (v.IsNull()) return Operand.CreateNull();
            return Operand.Create(v);
        }
        return super.GetParameter(parameter);
    }
}