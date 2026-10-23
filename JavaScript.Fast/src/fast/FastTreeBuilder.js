/**
 * AST -> ConditionTree / CalculateTree，以及 DIY 名称收集。
 * 等价替换 Antlr 版的 MathSplitVisitor / MathSplitVisitor2 / DiyNameVisitor。
 */
import { CharUtil } from '../Internals/Visitors/CharUtil.js';
import { ConditionTreeType } from '../Enums/ConditionTreeType.js';
import { CalculateTreeType } from '../Enums/CalculateTreeType.js';
import { funcDict } from './FunctionDict.js';

const KEY_STRIP = /^["'\s\t\r\n\f]+|["'\s\t\r\n\f]+$/g;
const MAX_TREE_DEPTH = 2000;

function isTrueName(t) { return t === 'TRUE' || t === 'YES' || t === '是' || t === '有'; }
function isFalseName(t) { return t === 'FALSE' || t === 'NO' || t === '不是' || t === '否' || t === '没有' || t === '无'; }
function isVersionName(t) { return t === 'ALGORITHMVERSION' || t === 'ENGINEVERSION'; }

/** 与 Antlr 版一致的懒加载文本 getter（共享同一个函数对象） */
function conditionStringGetter() {
    if (this._text == null && this._source != null && this.end >= this.start) {
        this._text = this._source.substring(this.start, this.end + 1);
    }
    return this._text;
}

function createTree(base, source) {
    base._source = source;
    Object.defineProperty(base, 'conditionString', {
        enumerable: true,
        configurable: true,
        get: conditionStringGetter,
    });
    return base;
}

export class FastTreeBuilder {
    // ---------------------------------------------------------------- ConditionTree

    /**
     * 只有 && / || 拆成节点，其余整体作为文本叶子；
     * 括号不产生节点，只把 HasBracket 打到被包裹的节点/叶子上。
     */
    static toConditionTree(source, ast) {
        return this._cond(source, ast, 0);
    }

    static _cond(source, node, depth) {
        if (++depth > MAX_TREE_DEPTH) {
            throw new Error(`Condition is nested too deeply (more than ${MAX_TREE_DEPTH} levels).`);
        }
        if (node.k === 'bracket') {
            const inner = this._cond(source, node.a, depth);
            inner.HasBracket = true;
            return inner;
        }
        if (node.k === 'bin' && (node.op === '&&' || node.op === '||')) {
            const tree = createTree({
                HasBracket: false,
                nodes: [],
                Type: node.op === '&&' ? ConditionTreeType.And : ConditionTreeType.Or,
                start: node.s,
                end: node.e,
            }, source);
            tree.nodes.push(this._cond(source, node.a, depth));
            tree.nodes.push(this._cond(source, node.b, depth));
            return tree;
        }
        return createTree({
            Type: ConditionTreeType.String,
            start: node.s,
            end: node.e,
        }, source);
    }

    // ---------------------------------------------------------------- CalculateTree

    static toCalculateTree(source, ast) {
        return this._calc(source, ast, 0);
    }

    static _calc(source, node, depth) {
        if (++depth > MAX_TREE_DEPTH) {
            throw new Error(`Expression is nested too deeply (more than ${MAX_TREE_DEPTH} levels).`);
        }
        if (node.k === 'bracket') {
            const inner = this._calc(source, node.a, depth);
            inner.HasBracket = true;
            return inner;
        }
        if (node.k === 'bin') {
            const type = this._calcType(node.op);
            if (type !== null) {
                const tree = createTree({
                    HasBracket: false,
                    nodes: [],
                    Type: type,
                    start: node.s,
                    end: node.e,
                }, source);
                tree.nodes.push(this._calc(source, node.a, depth));
                tree.nodes.push(this._calc(source, node.b, depth));
                return tree;
            }
        }
        return createTree({
            Type: CalculateTreeType.String,
            start: node.s,
            end: node.e,
        }, source);
    }

    static _calcType(op) {
        switch (op) {
            case '*': return CalculateTreeType.Mul;
            case '/': return CalculateTreeType.Div;
            case '%': return CalculateTreeType.Mod;
            case '+': return CalculateTreeType.Add;
            case '-': return CalculateTreeType.Sub;
            case '&': return CalculateTreeType.Connect;
            case '>=': return CalculateTreeType.OpGe;
            case '<=': return CalculateTreeType.OpLe;
            case '>': return CalculateTreeType.OpGt;
            case '<': return CalculateTreeType.OpLt;
            case '=': return CalculateTreeType.OpEq;
            case '!=': return CalculateTreeType.OpNe;
            case '&&': return CalculateTreeType.And;
            case '||': return CalculateTreeType.Or;
            default: return null;
        }
    }

    // ---------------------------------------------------------------- DiyName

    /**
     * 收集自定义参数与自定义函数。
     * 规则与 Antlr 版 DiyNameVisitor 一致：普通标识符记为参数，
     * 非内置函数的 `名字(...)` 记为自定义函数；TRUE/FALSE/NULL/E/PI/RAND/... 等特殊名忽略。
     * 用显式栈遍历，长链公式不会爆栈。
     */
    static collectDiyNames(source, ast) {
        const result = { Parameters: [], Functions: [] };
        const stack = [ast];
        while (stack.length > 0) {
            const node = stack.pop();
            if (node.k === 'id') {
                const t = CharUtil.standardString(node.name);
                if (!isTrueName(t) && !isFalseName(t) && !isVersionName(t)) {
                    result.Parameters.push({ Name: node.name, Start: node.s, End: node.e });
                }
            } else if (node.k === 'call') {
                const t = CharUtil.standardString(node.name);
                if (!isSpecialCallName(t) && !funcDict[t]) {
                    result.Functions.push({ Name: node.name, Start: node.nameS, End: node.nameE });
                }
            } else if (node.k === 'prop') {
                // 与 Antlr 版一致：.name 形式的关键字不计入
            }
            pushChildren(stack, node);
        }
        return result;
    }
}

function isSpecialCallName(t) {
    return t === 'E' || t === 'PI' || t === 'RAND' || t === 'GUID' || t === 'NOW' || t === 'TODAY'
        || isTrueName(t) || isFalseName(t) || isVersionName(t);
}

/** 逆序压栈，保证按原文从左到右访问子节点 */
function pushChildren(stack, node) {
    switch (node.k) {
        case 'bracket': case 'not': case 'pct':
            stack.push(node.a);
            break;
        case 'bin': case 'idx':
            stack.push(node.b);
            stack.push(node.a);
            break;
        case 'if':
            stack.push(node.c);
            stack.push(node.b);
            stack.push(node.a);
            break;
        case 'call':
            for (let i = node.args.length - 1; i >= 0; i--) { stack.push(node.args[i]); }
            break;
        case 'array': case 'json':
            for (let i = node.items.length - 1; i >= 0; i--) { stack.push(node.items[i]); }
            break;
        case 'jsonitem':
            stack.push(node.a);
            break;
        default:
            break;
    }
}

export { KEY_STRIP };
