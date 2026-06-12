package toolgood.algorithm.internals;

import java.util.List;

/**
 * 自定义类型
 */
public class DiyNameInfo {
    /**
     * 自定义 参数
     */
    public List<DiyNameKeyInfo> Parameters;

    /**
     * 自定义方法
     */
    public List<DiyNameKeyInfo> Functions;

    /**
     * 自定义类型
     */
    public DiyNameInfo(List<DiyNameKeyInfo> p, List<DiyNameKeyInfo> f) {
        Parameters = p;
        Functions = f;
    }
}
