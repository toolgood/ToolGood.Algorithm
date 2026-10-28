# ToolGood.Algorithm.Fast (Java)

`toolgood.algorithm` 的 **无 antlr4 依赖**版本：词法分析 + 递归下降语法分析全部手写，
导出的 API、函数库、求值结果与 ANTLR 版**完全一致**（同一套测试全量通过）。

## 与 ANTLR 版的差异

| | toolgood.algorithm（ANTLR 版） | 本包（Fast） |
|---|---|---|
| 解析实现 | `math.g4` 生成 `mathLexer/mathParser`（ANTLR4 运行时 + ATN 模拟） | `toolgood.algorithm.fast.FastLexer` + `FastParser`（手写） |
| 运行时依赖 | `org.antlr:antlr4-runtime:4.13.2`（326 KB） | **无** |
| 解析产物 | `mathParser.ProgContext` 语法树 + 4 个 Visitor 分层转换 | 轻量 `FastNode` 树 + 一次遍历构建 `FunctionBase` |
| 解析速度 | 4.42–4.49 µs/次 | **1.26–1.29 µs/次**（≈3.5×） |
| 解析+求值 | 5.71–5.79 µs/次 | **2.36–2.39 µs/次**（≈2.4×） |
| jar 体积 | 798,626 B + 运行时 326,307 B | **741,305 B**（无额外依赖） |
| 深嵌套 | 约 2000 层内正常，5000 层 `StackOverflowError` | 默认栈约 4000 层，**逻辑上限 5000**；栈不足时抛可捕获的 `FastParseException` |
| 超长链 `1+1+…` | 5000 项即 `StackOverflowError` | **50000 项仍可解析**（构建用显式栈） |

## 目录

```
src/main/java/toolgood/algorithm/
  fast/
    FastToken.java          Token 常量 + IsFunctionHead/IsMethodHead/IsParameter2（自动生成）
    FastKeywords.java       归一化文本 → Token 的开地址哈希表（自动生成，334 条）
    FastFunctionFactory.java Token → 函数类 的工厂（自动生成，278 个分支）
    FastLexeme.java         词法单元（可变对象，扫描时复用）
    FastLexer.java          手写词法器（数字+单位、字符串、注释、带点函数名 BETA.DIST）
    FastNode.java           轻量语法树节点（下标互相引用）
    FastParser.java         递归下降 + 优先级爬升，直接产出 FastNode 树
    FastAstBuilder.java     FastNode → FunctionBase（等价 MathFunctionVisitor）
    FastTreeBuilder.java    FastNode → ConditionTree / CalculateTree / DIY 名称收集
    FastParseException.java 语法错误（与函数构造期异常区分）
  AlgorithmEngine.java       仅把 ParseInternal 换成手写解析入口
  AlgorithmEngineHelper.java 仅把 GetDiyNames/ParseFormula/CheckFormula/ParseCondition/ParseCalculate 换掉
  ...（其余文件与 ANTLR 版完全相同，直接复用）

tools/gen_fast_tokens.py     从 g4/math.g4、mathParser.java、MathFunctionVisitor.java 生成上面 3 个文件
```

## 使用

```java
import toolgood.algorithm.AlgorithmEngine;

AlgorithmEngine engine = new AlgorithmEngine();
int v = engine.TryEvaluate("sum(1,2,3) * 2 + 1cm", 0);
```

与 ANTLR 版**同一个包名、同一个类名**，替换 jar 即可，不用改一行业务代码。

## 构建 / 测试

```bash
mvn test     # 801 项测试
mvn package  # 产出 target/toolgood-algorithm-fast-6.3.1.jar
```

改动文法或新增函数后重新生成查表代码：

```bash
cd tools && python gen_fast_tokens.py
```

## 实现要点

1. **优先级爬升**：`math.g4` 里 `expr` 是左递归规则，这里按同样的优先级阶梯
   （`函数调用/下标` > `!` > `%` > `* / %` > `+ - &` > 比较 > 等值 > `&&` > `||` > `?:`）手写实现，
   并把括号、前缀、后缀内联进 `parseExpr`，使每层嵌套只压 1 个栈帧。
2. **三目左结合**：ANTLR 对 `expr '?' expr ':' expr` 未声明 `assoc`，生成的是左结合，
   因此 `a?b:c?d:e` 等价于 `If(If(a,b,c),d,e)`。这里刻意照抄，保证零漂移。
3. **异常分层**：语法错误抛 `FastParseException`（`ParseWithoutError` 会吞掉并写入 `LastError`），
   函数构造期的参数校验异常照旧向外抛 —— 与 ANTLR 版逐条对齐。
4. **栈安全**：`StackOverflowError` 在 Java 中可捕获，入口方法会把它转换成
   `FastParseException("... nested too deeply for the current thread stack.")`，不会让进程崩溃。

## 已知限制

- 逻辑嵌套上限 `MAX_DEPTH = 5000`；实际能到多深取决于线程栈（`-Xss`）：
  默认约 4000 层，`-Xss16m` 可到 4999 层，`-Xss256k` 约 1000 层，超限均为可捕获异常。
- 修改**语法结构**（而不是新增函数）时需要手写改 `FastParser`；新增/改名函数只需重跑
  `tools/gen_fast_tokens.py`（对应 ANTLR 版改 `.g4` 再重新生成）。
- 与 ANTLR 版一样不支持 `'\u0041'` 这类 `\uXXXX` 转义（ANTLR 版也只输出 `u0041`），
  以保证两个实现的行为完全一致。
