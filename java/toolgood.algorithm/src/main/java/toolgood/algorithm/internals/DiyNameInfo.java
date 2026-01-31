/**
 * 自定义类型
 */
package toolgood.algorithm.internals;

import java.util.ArrayList;
import java.util.List;

public final class DiyNameInfo {
    /**
     * 自定义 参数
     */
    private final List<KeyInfo> parameters;

    /**
     * 自定义方法
     */
    private final List<KeyInfo> functions;

    /**
     * 自定义类型
     */
    DiyNameInfo() {
        parameters = new ArrayList<>();
        functions = new ArrayList<>();
    }

    public List<KeyInfo> getParameters() {
        return parameters;
    }

    public List<KeyInfo> getFunctions() {
        return functions;
    }

    /**
     * 关键字信息
     */
    public class KeyInfo {
        /**
         * 名称
         */
        private String name;
        /**
         * 开始位置
         */
        private int start;
        /**
         * 结束位置
         */
        private int end;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public int getStart() {
            return start;
        }

        public void setStart(int start) {
            this.start = start;
        }

        public int getEnd() {
            return end;
        }

        public void setEnd(int end) {
            this.end = end;
        }

        @Override
        public String toString() {
            return name;
        }
    }
}
