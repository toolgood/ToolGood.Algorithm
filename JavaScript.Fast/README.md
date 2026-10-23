# ToolGood.Algorithm.Fast (JavaScript)

`toolgood.algorithm` 的 **无 antlr4 运行时** 版本：词法分析 + 递归下降语法分析全部手写，
导出的 API、函数库、求值结果与 ANTLR 版**完全一致**（同一套测试全量通过）。

## 与 ANTLR 版的差异

| | toolgood.algorithm（ANTLR 版） | 本包（Fast） |
|---|---|---|
| 解析实现 | `math.g4` 生成 `mathLexer/mathParser`（ANTLR4 运行时 + ATN 模拟） | `src/fast/FastLexer.js` + `FastParser.js`（手写） |
| 运行时依赖 | 内置 `src/antlr4/**`（120 个文件，约 685 KB 源码） | 无 |
| 函数分派 | `MathFunctionVisitor`（Antlr Visitor + `funcDict`） | 直接查 `src/fast/FunctionDict.js` 的 `funcDict` |
| 首次解析 | 需反序列化 ATN、构建 DFA | 无额外初始化 |
| 解析速度 | 13.3 µs/次 | **3.1 µs/次**（≈4.3×） |
| 解析+求值 | 14.5 µs/次 | **3.9 µs/次**（≈3.7×） |

## 目录

```
src/
  fast/
    FastLexer.js        手写词法器（数字/字符串/标识符/运算符，含 COVARIANCE.P 这类带点函数名）
    FastParser.js       递归下降 + 优先级爬升，产出轻量 AST
    FastAstBuilder.js   AST -> FunctionBase（等价于 Antlr 版 MathFunctionVisitor）
    FastTreeBuilder.js  AST -> ConditionTree / CalculateTree / DIY 名称收集
    FunctionDict.js     函数名 -> 工厂 的映射（从 Antlr 版提取，301 个函数）
  AlgorithmEngine.js    仅把 parseInternal 换成手写解析入口
  AlgorithmEngineHelper.js  仅把 GetDiyNames/ParseFormula/CheckFormula/ParseCondition/ParseCalculate 换掉
  ...（其余文件与 ANTLR 版完全相同，直接复用）
tests/                  与 ANTLR 版相同的测试集合，Node 直接跑
```

## 使用

```js
import { AlgorithmEngine } from './src/index.js';

const engine = new AlgorithmEngine();
const value = engine.Evaluate(engine.Parse("sum(1,2,3) * 2 + 1cm")); // 13.01
```

## 构建 / 测试

```bash
npm install        # 运行时只有 crypto-js，devDependencies 用于 webpack/babel 打包
npm test           # node tests/TestAll.js
npm run build      # webpack 打包到 dist/toolgood.algorithm.fast.js
```

## 产物（dist）

| | toolgood.algorithm.js（ANTLR 版） | toolgood.algorithm.fast.js |
|---|---|---|
| 原始大小 | 346,521 B | **250,805 B**（-27.6%） |
| gzip 后 | 87,224 B | **60,101 B**（-31.1%） |

打包方式与 ANTLR 版完全相同（webpack 5 + babel preset-env + Terser，UMD）：
- CommonJS：`const { AlgorithmEngine } = require('toolgood.algorithm.fast');`
- 浏览器 `<script>`：全局变量 `ToolGood.Algorithm`
- 导出项与 ANTLR 版**逐一对齐**（`AlgorithmEngine`、`AlgorithmEngineEx`、`AlgorithmEngineHelper`、
  `CalculationLogicEngine`、`Operand`、`Enums` 等 10 项），无缺失、无多余

## 已知限制

- **每层嵌套的栈帧数**与 C# 版一致优化过，但由于 JS 引擎没有 `TryEnsureSufficientExecutionStack`，
  超深嵌套（> 约 3000 层）仍可能抛 `RangeError: Maximum call stack size exceeded`；
  `FastParser.MAX_DEPTH = 5000` 只做逻辑兜底。
- 修改**语法结构**（而不是新增函数）时需要手写改 `FastParser.js`；新增/改名函数只需动 `FunctionDict.js`
  （对应 ANTLR 版改 `.g4` 再重新生成）。
