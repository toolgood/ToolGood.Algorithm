/**
 * 閹垮秳缍旈弫?
 */
package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.List;
import java.util.ArrayList;
import java.util.Collection;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.litJson.JsonData;

public abstract class Operand {
    /**
     * 閻楀牊婀伴崣?
     */
    public static final Operand VERSION = new OperandString("ToolGood.Algorithm 6.1");

    /**
     * True
     */
    public static final Operand TRUE = new OperandBoolean(true);

    /**
     * False
     */
    public static final Operand FALSE = new OperandBoolean(false);

    /**
     * One
     */
    public static final Operand ONE = Operand.Create(new BigDecimal("1"));

    /**
     * Zero
     */
    public static final Operand ZERO = Operand.Create(new BigDecimal("0"));

    /**
     * Null
     */
    public static final Operand NULL_OPERAND = new OperandNull();

    /**
     * None閿涘牏鏁ゆ禍搴″棘閺佹壆琚崹瀣腹閺傤叏绱漀oneEngine 鏉╂柨娲栧銈咃拷纭风礆
     */
    public static final Operand None = new OperandNone();

    // 閺佸瓨鏆熺紓鎾崇摠閼煎啫娲�: -1000 ~ 1000閿涘苯鍙�2001娑擃亜锟�?
    private static final int INT_CACHE_OFFSET = 1000;
    private static final int INT_CACHE_SIZE = 2001;
    private static final Operand[] INT_CACHE = new Operand[INT_CACHE_SIZE];
    
    static {
        for (int i = 0; i < INT_CACHE_SIZE; i++) {
            INT_CACHE[i] = new OperandInt(i - INT_CACHE_OFFSET);
        }
        // ONE閸滃ERO瀹歌尙绮￠崷銊ュ灥婵瀵查弮鎯邦啎缂冾噯绱濇稉宥夋付鐟曚礁婀潻娆撳櫡闁插秵鏌婄拋鍓х枂
    }

    /**
     * 閺勵垰鎯佹稉铏光敄閸�?
     */
    public boolean IsNull() { return false; }
    /**
     * 閺勵垰鎯佹稉娲姜缁屽搫锟�?
     */
    public boolean IsNotNull() { return true; }

    /**
     * 閺勵垰鎯侀弫鏉跨摟
     */
    public boolean IsNumber() { return false; }
    /**
     * 閺勵垰鎯侀棃鐐存殶鐎�?
     */
    public boolean IsNotNumber() { return true; }

    /**
     * 閺勵垰鎯佺�涙顑佹稉?
     */
    public boolean IsText() { return false; }
    /**
     * 閺勵垰鎯侀棃鐐茬摟缁楋缚瑕�
     */
    public boolean IsNotText() { return true; }

    /**
     * 閺勵垰鎯佺敮鍐ㄧ毜閸�?
     */
    public boolean IsBoolean() { return false; }
    /**
     * 閺勵垰鎯侀棃鐐茬鐏忔柨锟�?
     */
    public boolean IsNotBoolean() { return true; }
    /**
     * 閺勵垰鎯侀弫鎵矋
     */
    public boolean IsArray() { return false; }
    /**
     * 閺勵垰鎯侀棃鐐存殶缂�?
     */
    public boolean IsNotArray() { return true; }
    /**
     * 閺勵垰鎯侀弮銉︽埂
     */
    public boolean IsDate() { return false; }
    /**
     * 閺勵垰鎯侀棃鐐存）閺�?
     */
    public boolean IsNotDate() { return true; }
    /**
     * 閺勵垰鎯丣son鐎电钖�
     */
    public boolean IsJson() { return false; }
    /**
     * 閺勵垰鎯侀棃婵瞫on鐎电钖�
     */
    public boolean IsNotJson() { return true; }
    /**
     * 閺勵垰鎯丣son閺佹壆绮�
     */
    public boolean IsArrayJson() { return false; }
    /**
     * 閺勵垰鎯侀棃婵瞫on閺佹壆绮�
     */
    public boolean IsNotArrayJson() { return true; }

    /**
     * 閺勵垰鎯侀崙娲晩
     */
    public boolean IsError() { return false; }

    /**
     * 闁挎瑨顕ゆ穱鈩冧紖
     */
    public String ErrorMsg() { return null; }
    
    /**
     * 閹垮秳缍旈弫鎵閸�?
     */
    public abstract OperandType Type();

    

    /**
     * 閺佹澘鐡ч崐?
     */
    public BigDecimal NumberValue() { throw new UnsupportedOperationException(); }

    /**
     * double閸�?
     */
    public double DoubleValue() { throw new UnsupportedOperationException(); }

    /**
     * int閸�?
     */
    public int IntValue() { throw new UnsupportedOperationException(); }

    /**
     * long閸�?
     */
    public long LongValue() { throw new UnsupportedOperationException(); }

    /**
     * 鐎涙顑佹稉鎻掞拷?
     */
    public String TextValue() { throw new UnsupportedOperationException(); }

    /**
     * 鐢啫鐨甸崐?
     */
    public boolean BooleanValue() { throw new UnsupportedOperationException(); }

    /**
     * 閺佹壆绮嶉崐?
     */
    public List<Operand> ArrayValue() { throw new UnsupportedOperationException(); }

    /**
     * Json閸�?
     */
    JsonData JsonValue() { throw new UnsupportedOperationException(); }

    /**
     * 閺冨爼妫块崐?
     */
    public MyDate DateValue() { throw new UnsupportedOperationException(); }

    
    
    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(boolean obj) {
        return obj ? TRUE : FALSE;
    }

    

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(short obj) {
        if (obj >= -INT_CACHE_OFFSET && obj <= INT_CACHE_OFFSET) {
            return INT_CACHE[obj + INT_CACHE_OFFSET];
        }
        return new OperandInt(obj);
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(int obj) {
        if (obj >= -INT_CACHE_OFFSET && obj <= INT_CACHE_OFFSET) {
            return INT_CACHE[obj + INT_CACHE_OFFSET];
        }
        return new OperandInt(obj);
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(long obj) {
        if (obj >= -INT_CACHE_OFFSET && obj <= INT_CACHE_OFFSET) {
            return INT_CACHE[(int)obj + INT_CACHE_OFFSET];
        }
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(float obj) {
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(double obj) {
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(BigDecimal obj) {
        return new OperandBigDecimal(obj);
    }

    

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(String obj) {
        if (obj == null) {
            return Operand.CreateNull();
        }
        return new OperandString(obj);
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand CreateJson(String txt) {
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                JsonData json = JsonData.parse(txt);
                return Operand.Create(json);
            } catch (Exception e) {
            }
        }
        return Operand.Error("Convert to json Error!");
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(MyDate obj) {
        return new OperandMyDate(obj);
    }



    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(JsonData obj) {
        return new OperandJson(obj);
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(List<Operand> obj) {
        return new OperandArray(obj);
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(String[] obj) {
        List<Operand> array = new ArrayList<>();
        for (String item : obj) {
            array.add(Create(item));
        }
        return new OperandArray(array);
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(Double[] obj) {
        List<Operand> array = new ArrayList<>();
        for (Double item : obj) {
            array.add(Create(new BigDecimal(item.toString())));
        }
        return new OperandArray(array);
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(BigDecimal[] obj) {
        List<Operand> array = new ArrayList<>();
        for (BigDecimal item : obj) {
            array.add(Create(item));
        }
        return new OperandArray(array);
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(Integer[] obj) {
        List<Operand> array = new ArrayList<>();
        for (Integer item : obj) {
            array.add(Create(item));
        }
        return new OperandArray(array);
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Create(Boolean[] obj) {
        List<Operand> array = new ArrayList<>();
        for (Boolean item : obj) {
            array.add(Create(item));
        }
        return new OperandArray(array);
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Error(String msg) {
        return new OperandError(msg);
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand Error(String msg, Object... args) {
        return new OperandError(String.format(msg, args));
    }

    /**
     * 閸掓稑缂撻幙宥勭稊閺�?
     */
    public static Operand CreateNull() {
        return new OperandNull();
    }

    

    

    /**
     * 鏉烆剚鏆熼崐鑲╄閸�?
     */
    public Operand ToNumber(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to number Error!");
    }

    /**
     * 鏉烆剚鏆熼崐鑲╄閸�?
     */
    public Operand ToNumber(String errorMessage, Object... args) {
        return Error(String.format(errorMessage, args));
    }

    /**
     * 鏉炵悿ool缁鐎�
     */
    public Operand ToBoolean(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to bool Error!");
    }

    /**
     * 鏉炵悿ool缁鐎�
     */
    public Operand ToBoolean(String errorMessage, Object... args) {
        return Error(String.format(errorMessage, args));
    }

    /**
     * 鏉炵憘tring缁鐎�
     */
    public Operand ToText(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to string Error!");
    }

    /**
     * 鏉炵憘tring缁鐎�
     */
    public Operand ToText(String errorMessage, Object... args) {
        return Error(String.format(errorMessage, args));
    }

    /**
     * 鏉炵悎yDate缁鐎�
     */
    public Operand ToMyDate(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to date Error!");
    }

    /**
     * 鏉炵悎yDate缁鐎�
     */
    public Operand ToMyDate(String errorMessage, Object... args) {
        return Error(String.format(errorMessage, args));
    }

    /**
     * 鏉炵徆rray缁鐎�
     */
    public Operand ToArray(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to array Error!");
    }

    /**
     * 鏉炵徆rray缁鐎�
     */
    public Operand ToArray(String errorMessage, Object... args) {
        return Error(String.format(errorMessage, args));
    }

    /**
     * 鏉炵悅son缁鐎�
     */
    public Operand ToJson(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to json Error!");
    }

    
}
