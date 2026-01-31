package toolgood.algorithm.litJson;

public enum JsonToken {
    None(1),

    ObjectStart(2),
    PropertyName(3),
    ObjectEnd(4),

    ArrayStart(5),
    ArrayEnd(6),

    Double(7),

    String(8),

    Boolean(9),
    Null(10);


    public int value;  
    // 构造方法  
    private JsonToken( int index) {  
         this.value = index;  
    }  
}
