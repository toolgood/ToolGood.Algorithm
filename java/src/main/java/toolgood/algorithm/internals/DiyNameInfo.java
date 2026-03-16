/**
 * 自定义类�?
 */
package toolgood.algorithm.internals;

import java.util.ArrayList;
import java.util.List;

public final class DiyNameInfo {
    /**
     * 自定�?参数
     */
    public final List<KeyInfo> Parameters;

    /**
     * 自定义方�?
     */
    public final List<KeyInfo> Functions;

    /**
     * 自定义类�?
     */
   public DiyNameInfo() {
        Parameters = new ArrayList<>();
        Functions = new ArrayList<>();
    }
 

    /**
     * 关键字信�?
     */
    public class KeyInfo {
        /**
         * 名称
         */
        public String Name;
        /**
         * 开始位�?
         */
        public int Start;
        /**
         * 结束位置
         */
        public int End;
      
        @Override
        public String toString() {
            return Name;
        }
    }
}
