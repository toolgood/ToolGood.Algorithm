import { ConditionTreeType } from '../Enums/ConditionTreeType.js';

/**
 * 条件树
 */
export class ConditionTree {
    constructor() {
        this.Type = ConditionTreeType.None;
        this.ErrorMessage = null;
        this.left = null;
        this.right = null;
        this.start = 0;
        this.end = 0;
        this.conditionString = '';
    }
}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.ConditionTree = ConditionTree;
}