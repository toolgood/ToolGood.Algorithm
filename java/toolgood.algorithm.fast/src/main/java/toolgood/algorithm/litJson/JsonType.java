package toolgood.algorithm.litJson;

/**
 * JsonType
 */
public enum JsonType {

    None(1),
    Object(2),
    Array(3),
    String(4),
    Double(5),
    Boolean(6),
    Null(7);
    
    public int value;  
    // 构造方法  
    private JsonType( int index) {  
         this.value = index;  
    }  
}