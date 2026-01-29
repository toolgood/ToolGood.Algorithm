/**
 * 键信息
 */
export class KeyInfo {
    constructor() {
        this.Name = '';
        this.Start = 0;
        this.End = 0;
    }
}

/**
 * 自定义名称信息
 */
export class DiyNameInfo {
    constructor() {
        this.Names = new Set();
        this.Parameters = [];
        this.Functions = [];
    }

    /**
     * 添加名称
     */
    AddName(name) {
        this.Names.add(name);
    }

    /**
     * 检查名称是否存在
     */
    HasName(name) {
        return this.Names.has(name);
    }

    /**
     * 获取名称数量
     */
    get Count() {
        return this.Names.size;
    }
}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.DiyNameInfo = DiyNameInfo;
    window.KeyInfo = KeyInfo;
}