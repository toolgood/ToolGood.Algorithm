package toolgood.algorithm;

import toolgood.algorithm.litJson.JsonData;

class OperandJson extends OperandT<JsonData> {
    public OperandJson(final JsonData obj) {
        super(obj);
    }
 
    @Override
    public OperandType Type() {
        return OperandType.JSON;
    }

    @Override
    public JsonData JsonValue(){
        return Value;
    }
}