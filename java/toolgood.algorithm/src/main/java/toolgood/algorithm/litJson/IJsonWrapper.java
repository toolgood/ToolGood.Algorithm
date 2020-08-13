package toolgood.algorithm.litJson;

public interface IJsonWrapper {
    
    boolean IsArray();
    boolean IsBoolean();
    boolean IsDouble();
    boolean IsObject();
    boolean IsString();
    boolean IsNull();

    void SetBoolean(boolean val);
    void SetDouble(double val);
    void SetJsonType(JsonType type);
    void SetString(String val);
    void SetNull();

    void Add(IJsonWrapper val);

    void Set(String key, IJsonWrapper val);


}