/**
 * 关键字信息
 */
package toolgood.algorithm.internals;

public class DiyNameKeyInfo {
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
