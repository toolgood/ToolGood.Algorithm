package toolgood.algorithm.litJson;

import java.math.BigDecimal;

public interface IJsonWrapper {
    
    boolean IsArray();
    boolean IsBoolean();
    boolean IsDouble();
    boolean IsObject();
    boolean IsString();
    boolean IsNull();

    void SetBoolean(boolean val);
    void SetDouble(BigDecimal val);
    void SetJsonType(JsonType type);
    void SetString(String val);
    void SetNull();

    void Add(IJsonWrapper val);

    void Set(String key, IJsonWrapper val);


}