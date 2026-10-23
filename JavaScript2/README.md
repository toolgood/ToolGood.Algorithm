ToolGood.Algorithm
===================

用于 HtmlEditor 中：**只做公式语法校验，不求值**。

手写词法 / 递归下降解析，已完全不依赖 antlr4（同时移除了内置的 antlr4 运行时与生成代码）。

## 用法

```js
import { AlgorithmEngineHelper, AntlrErrorListener } from './src/index.js';

class MyListener extends AntlrErrorListener {
    constructor() { super(); this.errors = []; }
    syntaxError(recognizer, offendingSymbol, line, column, msg, e) {
        this.errors.push({ line, column, msg });
    }
}

const listener = new MyListener();
const ast = AlgorithmEngineHelper.ParseFormula('(2+3)*4/2mm', listener);
// ast      -> 语法树对象（有语法错误时也会返回，与 ANTLR 的错误恢复行为一致）
// listener.errors -> []（有错时每处错误一条）
```

约定：

| 输入 | 返回 |
|---|---|
| `null` / `undefined` / 空串 / 全空白 | `null` |
| 未传 `errorListener` | `null` |
| 正常公式 | 语法树对象，`errors` 为空 |
| 有语法错误 | **仍然返回**语法树对象，同时回调 `errorListener.syntaxError(...)` |

`ParseFormula` 的签名与 ANTLR 版完全一致，替换即可，调用方无需改动。

## 语法树结构

```js
{ type: 'Binary', start: 0, end: 14, op: '+', children: [ ... ] }
```

`type` 取值：`Number` / `String` / `Parameter` / `Bracket` / `Not` / `Percentage` /
`Binary`（带 `op` 原文）/ `If` / `Call`（带 `name`）/ `Member`（带 `name`）/
`Index` / `Array` / `ArrayJson` / `ArrayJsonItem`（带 `name`）/ `Error`。
`start` / `end` 是原文下标（闭区间）。

## 与 ANTLR 版的差异

| | ANTLR 版 | 现版（手写） |
|---|---|---|
| 解析实现 | `mathjs.g4` 生成 `mathjsLexer/mathjsParser` | `src/fast/FastLexer.js` + `FastParser.js` |
| 运行时依赖 | 内置 `src/antlr4/**`（120 个文件） | **无** |
| 解析速度 | 6.96 µs/次 | **2.65 µs/次**（≈2.6×） |
| dist 体积 | 86,787 B / gzip 26,369 B | **10,941 B / gzip 3,143 B**（−87%） |
| 源码文件数 | 143 | **6** |
| 错误恢复 | ANTLR 自适应预测 | 手写恢复：报错后跳过非法 token 继续解析 |

## 行为一致性验证

- 单元测试：`npm test` → 7/7 通过（测试文件未改动）
- 定向差分：167 条覆盖文法的表达式，逐条比对「是否返回结果 + 是否报错」→ **0 差异**
- 随机差分：2000 条随机 token 组合，比对「接受 / 拒绝 / 是否抛异常」→ **0 差异**
  （唯一一处不同是 `{a:1}%[1].(`：ANTLR 版自身抛 `TypeError` 崩溃，现版正常报语法错误）

## 构建

```bash
npm test        # jest
npm run build   # webpack → dist/toolgood.algorithm.js
```

> 注意：`webpack.config.js` 已关闭 Terser 的**属性名混淆**。
> 之前开启时 `listener.syntaxError` 会被改名，导致打包产物里错误回调失效
> （旧 dist 在 `1+` 这类非法输入上会直接抛 TypeError）。
