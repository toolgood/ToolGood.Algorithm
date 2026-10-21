# ToolGood.Algorithm — Go 版

`ToolGood.Algorithm` 表达式引擎的 Go 移植，**零第三方依赖**（仅标准库），Go 1.21+。

目录结构按 `../csharp/ToolGood.Algorithm/` 一一对应，方便从 C# 源码直接定位到 Go 实现。

```go
package main

import (
	"fmt"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

func main() {
	engine := alg.NewAlgorithmEngine(false)

	fmt.Println(engine.TryEvaluateInt("1+2*3", 0))          // 7
	fmt.Println(engine.TryEvaluateInt("SUM(1,2,3)", 0))     // 6
	fmt.Println(engine.TryEvaluateText("'abc'.UPPER()", "")) // ABC
	fmt.Println(engine.TryEvaluateInt(`json('{"a":1}')['a']`, 0)) // 1
}
```

## 目录对照表

| C#（`csharp/ToolGood.Algorithm/`） | Go（`go/algorithm/`） |
| --- | --- |
| `AlgorithmEngine.cs` | `internals/engine.go`（引擎本体）＋根包 `alias.go`（对外别名） |
| `AlgorithmEngineEx.cs` | `algorithm_engine_ex.go` |
| `AlgorithmEngineHelper.cs` | `algorithm_engine_helper.go` |
| `FunctionCache.cs` / `IFunctionCache.cs` | `function_cache.go` |
| `Operand.cs` + `Operands/Operand*.cs` | `operands/operand.go` |
| `Operands/MyDate.cs` | `operands/mydate.go` |
| `Operands/KeyValue.cs` | `operands/operand.go`（`KeyValue`） |
| `Enums/*.cs` | `enums/enums.go` |
| `LitJson/*.cs` | `litjson/json.go`（`JsonData` / `JsonMapper`） |
| `Internals/CalculateTree.cs` / `ConditionTree.cs` | `internals/tree.go` |
| `Internals/ParameterType.cs` / `DiyNameInfo.cs` | `internals/tree.go` |
| `Internals/Functions/FunctionBase.cs` / `Function_0..N.cs` | `internals/function_base.go` |
| `Internals/Functions/FunctionUtil.cs` | `internals/function_util.go` |
| `Internals/Visitors/CharUtil.cs` | `internals/char_util.go` |
| `Internals/Visitors/DiyNameVisitor.cs` 等 | `algorithm_engine_helper.go` |
| `Internals/Functions/Compare/` | `internals/functions/compare/` |
| `Internals/Functions/Csharp/` | `internals/functions/csharp/` |
| `Internals/Functions/CsharpSecurity/` | `internals/functions/csharpsecurity/` |
| `Internals/Functions/CsharpWeb/` | `internals/functions/csharpweb/` |
| `Internals/Functions/DateTimes/` | `internals/functions/datetimes/` |
| `Internals/Functions/Financial/` | `internals/functions/financial/` |
| `Internals/Functions/Flow/` | `internals/functions/flow/` |
| `Internals/Functions/MathBase/` | `internals/functions/mathbase/` |
| `Internals/Functions/MathSum/` | `internals/functions/mathsum/` |
| `Internals/Functions/MathSum2/` | `internals/functions/mathsum2/` |
| `Internals/Functions/MathTransformation/` | `internals/functions/mathtransformation/` |
| `Internals/Functions/MathTrigonometric/` | `internals/functions/mathtrigonometric/` |
| `Internals/Functions/Operator/` | `internals/functions/operator/` |
| `Internals/Functions/String/` | `internals/functions/string/` |
| `Internals/Functions/Value/` | `internals/functions/value/` |
| `MathNet/` | `mathnet/`（`distributions/`、`specialfunctions/`、`statistics/`、`rootfinding/`） |
| `UnitConversion/` | `unitconversion/` |
| `CalculationLogic/` | `calculationlogic/` |
| `System/MathEx.cs` | 标准库 `math` + `decimal`（见下） |
| `decimal` / `BigDecimal`（语言内建） | `decimal/decimal.go`（自实现，Go 无内建十进制） |
| `math/math.g4` + ANTLR 生成代码 | `lexer.go` + `parser.go`（手写词法/递归下降，与 `math.g4` 等价） |

### 包依赖方向

```
enums / decimal  →  litjson  →  operands  →  internals  →  internals/functions/*
                                                   ↓
                                          algorithm（根包：语法 + 公开 API）
```

`internals` 不反向依赖解析器：`internals.ParseExpression` 由根包在 `init()` 注入，
因此函数子包、计算逻辑、单位换算都能独立编译、独立测试。

## 已实现能力

- 完整表达式文法：运算符优先级、`?:` 三元、`&&` / `||`、`%` 后缀、数组 `[..]`、对象 `{..}`、
  链式调用 `'abc'.UPPER()`、下标/属性 `json['a'].b`、带单位数字 `1KM` / `1M2`。
- 283 个注册函数，覆盖 C# `Internals/Functions/` 下 277 个 `Function_*.cs`（差集仅为字面量节点，
  如 `VALUETEXT`、`ARRAYJSONITEM`，它们由解析器直接构造，不注册）。
- 自定义参数（`GetParameterFunc`）、自定义函数（`ExecuteDiyFunctionFunc`）、
  解析缓存（`FunctionCache`）、参数类型推导（`GetParameterTypes`）、DIY 名称提取（`GetDiyNames`）、
  `AlgorithmEngineEx`（`AddParameterFromJson` 等）、`CalculationLogicEngine`、单位换算。
- 十进制使用自实现的任意精度定点数（加减乘精确、除法按 34 位有效数字 HALF_EVEN）。

## 构建与测试

```bash
cd go
./build.sh build ./...
./build.sh test ./...
./build.sh vet ./...
```

`build.sh` 会把 `GOCACHE` / `GOTMPDIR` 指向仓库的 `temp/` 目录，编译缓存与临时产物不会落在源码树内。

## 与 C# 的已知差异

1. **十进制精度**：C# `decimal` 为 28–29 位且范围有限；Go 版 `Decimal` 为任意精度，
   除法按 DECIMAL128（34 位有效数字）舍入，因此极少数「恰好落在末位」的结果末几位可能不同。
2. **超越函数**：C# 用 `MathEx` 的 decimal 级数（约 28 位），Go 版 `SIN/COS/EXP/LN/LOG/POW` 等走
   `float64`（15–16 位）；`SQRT` 使用 Decimal 开方（34 位）。
3. **正则**：Go `regexp` 是 RE2，不支持反向引用、环视、命名捕获语法；编译失败按 C# 的
   `ArgumentException` 分支返回参数错误。C# 的 1 秒超时无对应分支（RE2 无回溯爆炸）。
4. **HTML 实体**：`HTMLENCODE/HTMLDECODE` 覆盖 `& < > " '`、数字实体与常用 Latin-1 命名实体，
   未收录 HTML4 全量实体表。
5. **哈希输出**：`MD5/SHA*/HMAC*` 输出**大写**十六进制，与 C# `Convert.ToHexString` 一致
   （Java/Python 版为小写）。
6. **UTF-16 vs rune**：`LEN/LEFT/MID/RIGHT/FIND/INDEXOF/SUBSTRING` 等按 UTF-16 码元计数，
   与 C# `string.Length` 一致；`SEARCH` 的通配符匹配按 rune 进行。
7. **下标基准**：`ExcelIndex` 默认 1（`json('[1,2,3]')[1]` 取第一个元素），与 C# 默认值一致。

## 测试

`go/test/` 是从 `csharp/ToolGood.Algorithm.Test` 逐类移植的单元测试（**854 个子用例**，
对应 32 个 C# 测试类），文件名与测试类对应：

| C# 测试类 | Go 文件 |
| --- | --- |
| `AlgorithmEngine/MathBaseTest` | `test/math_base_test.go` |
| `AlgorithmEngine/MathTrigonometricTest` | `test/math_trigonometric_test.go` |
| `AlgorithmEngine/MathTransformationTest` | `test/math_transformation_test.go` |
| `AlgorithmEngine/MathSumTest` / `MathSum2Test` | `test/math_sum_test.go` / `math_sum2_test.go` |
| `AlgorithmEngine/StringTest` | `test/string_test.go` |
| `AlgorithmEngine/DateTimesTest` | `test/date_times_test.go` |
| `AlgorithmEngine/FinancialTest` | `test/financial_test.go` |
| `AlgorithmEngine/FlowTest` / `ValueTest` | `test/flow_test.go` / `value_test.go` |
| `AlgorithmEngine/OperatorTest` / `CompareTest` | `test/operator_test.go` / `compare_test.go` |
| `AlgorithmEngine/CsharpTest` / `CsharpSecurityTest` / `CsharpWebTest` | `test/csharp_test.go` / `csharp_security_test.go` / `csharp_web_test.go` |
| `AlgorithmEngine/StrictModeTest` / `EngineTest` | `test/strict_mode_test.go` / `engine_test.go` |
| `Operands/OperandsTest` | `test/operands_test.go` |
| `ParameterTypes/ParameterTypeTest` | `test/parameter_type_test.go` |
| `AlgorithmEngineEx/AlgorithmEngineExTest` | `test/algorithm_engine_ex_test.go` |
| `AlgorithmEngineHelper/AlgorithmEngineHelperTest` | `test/algorithm_engine_helper_test.go` |
| `CalculationLogic/CalculationLogicTest` | `test/calculation_logic_test.go` |
| `ConditionTrees/CalculateTreeTest` / `ConditionTreeTest` | `test/calculate_tree_test.go` / `condition_tree_test.go` |
| `FunctionCacheTest` | `test/function_cache_test.go` |
| `Helpers/MyDateTest` | `test/my_date_test.go` |
| `UnitConversion/UnitConversionTest` / `UnitConversionPrecisionTest` | `test/unit_conversion_test.go` / `unit_conversion_precision_test.go` |
| `Tests/IssuesTest` | `test/issues_test.go` |

另有 `algorithm/engine_test.go`（求值链路冒烟）与 `datetimes/port_check_test.go`、`financial/port_check_test.go`。

```bash
./build.sh test ./test/          # 854 个子用例
./build.sh test ./...            # 全部包
```

### 移植测试时发现并修复的库缺陷

1. `LastIndexOf` 的 `startIndex` 语义：.NET 要求**匹配结束位置**不超过 `startIndex`（`LastIndexOf('ababa','aba',2)` → 0），原实现按起始位置搜索。
2. 数字字面量与 JSON 数字缺少 `decimal` 范围校验（C# 抛 `Number '...' is invalid or out of range.`）。
3. 字符串里的 `\uXXXX` 转义未还原。
4. `COVARIANCES` 只作为别名值存在，未进入关键字表。
5. 错误文案占位符 `%s`/`%d` 与 `Errorf` 的 `{0}`/`{1}` 不一致（参数缺失、DiyFunction 缺失等）。
6. 运算符文本输出未按 C# 给子节点强制加括号（影响 `ToString` 往返）。
7. 字符串/日期/财务函数缺少 `GetParameterTypes` 的参数类型声明，导致 `LEFT/TEXT` 等推导为 `NONE`；
   已用 `internals.RegisterParameterTypes` 声明表补齐。
