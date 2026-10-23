/**
 * AST -> FunctionBase 树。等价替换 Antlr 版 MathFunctionVisitor 的 visit* 方法，
 * 函数分派直接查 funcDict（名称 → 工厂），没有 300 个 case 的 switch。
 */
import { Operand } from '../Operand.js';
import { CharUtil } from '../Internals/Visitors/CharUtil.js';
import { NumberUnitTypeHelper } from '../Internals/NumberUnitTypeHelper.js';
import { funcDict as functionDict, findFunction } from './FunctionDict.js';

import { Function_Value } from '../Internals/Functions/Value/Function_Value.js';
import { Function_NUM } from '../Internals/Functions/Value/Function_NUM.js';
import { Function_PARAMETER } from '../Internals/Functions/Value/Function_PARAMETER.js';
import { Function_DiyFunction } from '../Internals/Functions/Value/Function_DiyFunction.js';
import { Function_Array } from '../Internals/Functions/Value/Function_Array.js';
import { Function_ArrayJson } from '../Internals/Functions/Value/Function_ArrayJson.js';
import { Function_ArrayJsonItem } from '../Internals/Functions/Value/Function_ArrayJsonItem.js';
import { Function_GetJsonValue } from '../Internals/Functions/Value/Function_GetJsonValue.js';

import { Function_Add } from '../Internals/Functions/Operator/Function_Add.js';
import { Function_Sub } from '../Internals/Functions/Operator/Function_Sub.js';
import { Function_Mul } from '../Internals/Functions/Operator/Function_Mul.js';
import { Function_Div } from '../Internals/Functions/Operator/Function_Div.js';
import { Function_Mod } from '../Internals/Functions/Operator/Function_Mod.js';
import { Function_Connect } from '../Internals/Functions/Operator/Function_Connect.js';
import { Function_AND } from '../Internals/Functions/Operator/Function_AND.js';
import { Function_OR } from '../Internals/Functions/Operator/Function_OR.js';

import { Function_EQ } from '../Internals/Functions/Compare/Function_EQ.js';
import { Function_NE } from '../Internals/Functions/Compare/Function_NE.js';
import { Function_LT } from '../Internals/Functions/Compare/Function_LT.js';
import { Function_LE } from '../Internals/Functions/Compare/Function_LE.js';
import { Function_GT } from '../Internals/Functions/Compare/Function_GT.js';
import { Function_GE } from '../Internals/Functions/Compare/Function_GE.js';

import { Function_NOT } from '../Internals/Functions/Flow/Function_NOT.js';
import { Function_IF } from '../Internals/Functions/Flow/Function_IF.js';
import { Function_Percentage } from '../Internals/Functions/MathBase/Function_Percentage.js';
import { Function_RAND } from '../Internals/Functions/MathBase/Function_RAND.js';
import { Function_NOW } from '../Internals/Functions/DateTimes/Function_NOW.js';
import { Function_TODAY } from '../Internals/Functions/DateTimes/Function_TODAY.js';
import { Function_GUID } from '../Internals/Functions/Csharp/Function_GUID.js';

const KEY_STRIP = /^["'\s\t\r\n\f]+|["'\s\t\r\n\f]+$/g;

function isTrueName(t) { return t === 'TRUE' || t === 'YES' || t === '是' || t === '有'; }
function isFalseName(t) { return t === 'FALSE' || t === 'NO' || t === '不是' || t === '否' || t === '没有' || t === '无'; }
function isVersionName(t) { return t === 'ALGORITHMVERSION' || t === 'ENGINEVERSION'; }

/** 与 Antlr 版 visitSTRING_fun 完全一致的转义处理 */
function unescapeString(raw) {
    const sb = [];
    let index = 1;
    while (index < raw.length - 1) {
        let c = raw[index++];
        if (c === '\\') {
            const c2 = raw[index++];
            if (c2 === 'n') { sb.push('\n'); }
            else if (c2 === 'r') { sb.push('\r'); }
            else if (c2 === 't') { sb.push('\t'); }
            else if (c2 === '0') { sb.push('\0'); }
            else if (c2 === 'v') { sb.push('\v'); }
            else if (c2 === 'a') { sb.push('\a'); }
            else if (c2 === 'b') { sb.push('\b'); }
            else if (c2 === 'f') { sb.push('\f'); }
            else { sb.push(c2); }
        } else {
            sb.push(c);
        }
    }
    return sb.join('');
}

/** 拆出数值后缀单位：优先匹配最长的 1~3 字符单位，且前缀必须是合法数字 */
function splitUnit(numText) {
    const dict = NumberUnitTypeHelper.getUnitTypedict();
    for (let n = 3; n >= 1; n--) {
        if (numText.length <= n) { continue; }
        const suffix = numText.substring(numText.length - n);
        const head = numText.substring(0, numText.length - n);
        if (dict[suffix.toUpperCase()] !== undefined && isPureNumber(head)) {
            return { d: parseFloat(head), unit: suffix };
        }
    }
    return null;
}

function isPureNumber(s) {
    if (s.length === 0) { return false; }
    return !isNaN(Number(s));
}

export class FastAstBuilder {
    /**
     * @param {object} text 原始公式
     * @param {object} ast FastParser 产出的语法树
     * @returns {FunctionBase}
     */
    static build(text, ast) {
        return this._build(text, ast);
    }

    static _build(text, node) {
        switch (node.k) {
            case 'bracket':
                return this._build(text, node.a);

            case 'num': {
                const numText = text.substring(node.s, node.e + 1);
                const split = splitUnit(numText);
                if (split !== null) { return new Function_NUM(split.d, split.unit); }
                return new Function_Value(Operand.Create(parseFloat(numText)), numText);
            }

            case 'str':
                return new Function_Value(Operand.Create(unescapeString(text.substring(node.s, node.e + 1))));

            case 'null':
                return new Function_Value(Operand.CreateNull(), 'NULL');

            case 'id': {
                const t = CharUtil.standardString(node.name);
                if (isTrueName(t)) { return new Function_Value(Operand.True, t); }
                if (isFalseName(t)) { return new Function_Value(Operand.False, t); }
                if (isVersionName(t)) { return new Function_Value(Operand.Version, t); }
                return new Function_PARAMETER(node.name);
            }

            case 'call':
                return this._buildCall(text, node);

            case 'prop': {
                const keyName = node.name.trim().replace(KEY_STRIP, '');
                return new Function_GetJsonValue(this._build(text, node.a), new Function_Value(Operand.Create(keyName), keyName));
            }

            case 'idx':
                return new Function_GetJsonValue(this._build(text, node.a), this._build(text, node.b));

            case 'not':
                return new Function_NOT(this._build(text, node.a));

            case 'pct':
                return new Function_Percentage(this._build(text, node.a));

            case 'if':
                return new Function_IF([this._build(text, node.a), this._build(text, node.b), this._build(text, node.c)]);

            case 'array': {
                const args = new Array(node.items.length);
                for (let i = 0; i < node.items.length; i++) { args[i] = this._build(text, node.items[i]); }
                return new Function_Array(args);
            }

            case 'json': {
                const args = new Array(node.items.length);
                for (let i = 0; i < node.items.length; i++) { args[i] = this._build(text, node.items[i]); }
                return new Function_ArrayJson(args);
            }

            case 'jsonitem': {
                const keyText = node.key;
                const keyName = keyText.replace(KEY_STRIP, '');
                return new Function_ArrayJsonItem(keyName, this._build(text, node.a));
            }

            case 'bin':
                return this._buildBinary(text, node);

            default:
                throw new Error(`Node '${node.k}' is not implemented.`);
        }
    }

    static _buildCall(text, node) {
        const rawName = node.name;
        const t = CharUtil.standardString(rawName);

        // 无参常量/函数：与 Antlr 版一致，忽略括号内参数
        if (t === 'E') { return new Function_Value(Operand.Create(Math.E), 'E'); }
        if (t === 'PI') { return new Function_Value(Operand.Create(Math.PI), 'PI'); }
        if (isTrueName(t)) { return new Function_Value(Operand.True, t); }
        if (isFalseName(t)) { return new Function_Value(Operand.False, t); }
        if (isVersionName(t)) { return new Function_Value(Operand.Version, t); }
        if (t === 'RAND') { return new Function_RAND(); }
        if (t === 'GUID') { return new Function_GUID(); }
        if (t === 'NOW') { return new Function_NOW(); }
        if (t === 'TODAY') { return new Function_TODAY(); }

        const args = new Array(node.args.length);
        for (let i = 0; i < node.args.length; i++) { args[i] = this._build(text, node.args[i]); }

        const factory = findFunction(t);
        if (factory) { return factory(args); }
        // 传入原文，对齐 Antlr 版 new Function_DiyFunction(context.f.text, funcs)
        return new Function_DiyFunction(rawName, args);
    }

    static _buildBinary(text, node) {
        const a = this._build(text, node.a);
        const b = this._build(text, node.b);
        switch (node.op) {
            case '*': return new Function_Mul([a, b]);
            case '/': return new Function_Div([a, b]);
            case '%': return new Function_Mod([a, b]);
            case '+': return new Function_Add([a, b]);
            case '-': return new Function_Sub([a, b]);
            case '&': return new Function_Connect([a, b]);
            case '>': return new Function_GT([a, b]);
            case '>=': return new Function_GE([a, b]);
            case '<': return new Function_LT([a, b]);
            case '<=': return new Function_LE([a, b]);
            case '=': return new Function_EQ([a, b]);
            case '!=': return new Function_NE([a, b]);
            case '&&': return new Function_AND([a, b]);
            case '||': return new Function_OR([a, b]);
            default: throw new Error(`Operator '${node.op}' is not implemented.`);
        }
    }
}

export { functionDict };
