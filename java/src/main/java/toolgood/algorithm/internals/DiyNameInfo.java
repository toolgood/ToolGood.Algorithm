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
    private List<DiyNameKeyInfo> parameters;

    /**
     * 自定义方法
     */
    private List<DiyNameKeyInfo> functions;

    /**
     * 自定义类型
     */
    public DiyNameInfo() {
        parameters = new ArrayList<DiyNameKeyInfo>();
        functions = new ArrayList<DiyNameKeyInfo>();
    }

    public List<DiyNameKeyInfo> getParameters() {
        return parameters;
    }

    public List<DiyNameKeyInfo> getFunctions() {
        return functions;
    }
}
