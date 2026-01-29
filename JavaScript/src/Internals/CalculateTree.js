import { CalculateTreeType } from '../Enums/CalculateTreeType.js';

/**
 * 计算树
 */
export class CalculateTree {
    constructor() {
        this.Type = CalculateTreeType.None;
        this.ErrorMessage = null;
        this.nodes = [];
        this.start = 0;
        this.end = 0;
        this.conditionString = '';
    }
}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.CalculateTree = CalculateTree;
}