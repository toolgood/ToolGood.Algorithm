package toolgood.algorithm;

import toolgood.algorithm.litJson.JsonData;

class OperandJson extends OperandT<JsonData> {
    public OperandJson(final JsonData obj) : base(obj) { }
    public override OperandType Type => OperandType.JSON;
    internal override JsonData JsonValue => Value;
}