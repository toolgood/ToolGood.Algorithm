# ToolGood.Algorithm — Python 版

`ToolGood.Algorithm` 表达式引擎的 Python 移植，零第三方依赖（仅标准库），
可直接在 CPython 3.9+ 运行。

Python 包的目录结构**按 `csharp/ToolGood.Algorithm` 一一对应**，方便从 C# 源码
直接定位到 Python 实现。

```python
from toolgood_algorithm import AlgorithmEngine

engine = AlgorithmEngine()
engine.try_evaluate("1+2*3", 0)                    # 7
engine.try_evaluate("SUM(1,2,3)", 0)               # 6
engine.try_evaluate("'abc'.UPPER()", "")           # 'ABC'
engine.try_evaluate("json('{\"a\":1}')['a']", 0)   # 1
```

## 目录对照表

| C#（`csharp/ToolGood.Algorithm/`） | Python（`python/toolgood_algorithm/`） |
| --- | --- |
| `AlgorithmEngine.cs` | `algorithm_engine.py`（含 `NoneEngine`） |
| `AlgorithmEngineEx.cs` | `algorithm_engine_ex.py` |
| `AlgorithmEngineHelper.cs` | `algorithm_engine_helper.py` |
| `FunctionCache.cs` / `IFunctionCache.cs` | `function_cache.py` |
| `Operand.cs` + `Operands/Operand*.cs` | `operands/operand.py`（合并为一个模块，与 Java 版一致） |
| `Operands/MyDate.cs` | `operands/my_date.py` |
| `Operands/KeyValue.cs` | `operands/operand.py`（`KeyValue`） |
| `CalculationLogic/*.cs` | `calculation_logic/`（`engine.py` / `info.py` / `calculation_logic_type.py`） |
| `Enums/*.cs` | `enums/__init__.py` |
| `Internals/CalculateTree.cs` | `internals/calculate_tree.py` |
| `Internals/ConditionTree.cs` | `internals/condition_tree.py` |
| `Internals/DiyNameInfo.cs` / `DiyNameKeyInfo.cs` | `internals/diy_name_info.py` |
| `Internals/ParameterType.cs` | `internals/parameter_type.py` |
| `Internals/Functions/FunctionBase.cs` / `Function_0..N.cs` | `internals/functions/function_base.py` |
| `Internals/Functions/FunctionUtil.cs` | `internals/functions/function_util.py` |
| `Internals/Functions/NoneEngine.cs` | `algorithm_engine.py`（`NoneEngine`） |
| `Internals/Functions/Compare/` | `internals/functions/compare.py` |
| `Internals/Functions/Csharp/` | `internals/functions/csharp.py` |
| `Internals/Functions/CsharpSecurity/` | `internals/functions/csharp_security.py` |
| `Internals/Functions/CsharpWeb/` | `internals/functions/csharp_web.py` |
| `Internals/Functions/DateTimes/` | `internals/functions/date_times.py` |
| `Internals/Functions/Financial/` | `internals/functions/financial.py` |
| `Internals/Functions/Flow/` | `internals/functions/flow.py` |
| `Internals/Functions/MathBase/` | `internals/functions/math_base.py` |
| `Internals/Functions/MathSum/` | `internals/functions/math_sum.py` |
| `Internals/Functions/MathSum2/` | `internals/functions/math_sum2.py` |
| `Internals/Functions/MathTransformation/` | `internals/functions/math_transformation.py` |
| `Internals/Functions/MathTrigonometric/` | `internals/functions/math_trigonometric.py` |
| `Internals/Functions/Operator/` | `internals/functions/operator.py` |
| `Internals/Functions/String/` | `internals/functions/string.py` |
| `Internals/Functions/Value/` | `internals/functions/value.py` |
| `Internals/Visitors/CharUtil.cs` | `internals/visitors/char_util.py` |
| `Internals/Visitors/DiyNameVisitor.cs` | `internals/visitors/diy_name_visitor.py` |
| `Internals/Visitors/MathSplitVisitor.cs` / `MathSplitVisitor2.cs` | `internals/visitors/math_split_visitor.py` |
| `Internals/Visitors/MathFunctionVisitor.cs` | `math/parser.py`（`AstNode.to_function`） |
| `LitJson/JsonData.cs` | `lit_json/json_data.py` |
| `LitJson/JsonMapper.cs` | `lit_json/json_mapper.py` |
| `MathNet/Constants.cs` | `math_net/constants.py` |
| `MathNet/ExcelFunctions.cs` | `math_net/excel_functions.py` |
| `MathNet/Precision.cs` | `math_net/precision.py` |
| `MathNet/Distributions/*.cs` | `math_net/distributions/__init__.py` |
| `MathNet/RootFinding/Brent.cs` | `math_net/root_finding/__init__.py` |
| `MathNet/SpecialFunctions/*.cs` | `math_net/special_functions/`（`bessel_coefficients.py` 为 Chebyshev 系数表） |
| `MathNet/Statistics/*.cs` | `math_net/statistics/__init__.py` |
| `UnitConversion/*.cs` | `unit_conversion/`（四个 converter + `__init__.py`） |
| `math/mathLexer.cs` + `mathParser.cs`（ANTLR 生成） | `math/lexer.py` + `math/parser.py`（手写等价实现） |

约定：

* **一个 C# 目录 = 一个 Python 模块**，模块内仍是「一个函数一个类」，
  类名与 C# 完全一致（`Function_SUM`、`Function_LEFT` …）。
* 函数注册用 `@register("SUM")`，键名与 `g4/math.g4` 的 ANTLR token 一致。
* 公共 API 以 snake_case 为主（`try_evaluate`、`parse`、`add_parameter`），
  同时保留 CamelCase 别名（`TryEvaluate`、`Parse`、`AddParameter`），
  别名通过包装方法实现，子类覆写依然生效。

## 与 C#/Java 的差异

1. **解析器**：不依赖 ANTLR 运行时，`math/lexer.py` 手写词法分析（含大小写/
   全角归一化、关键字最长匹配、数字单位后缀），`math/parser.py` 为递归下降 +
   优先级爬升，语义与 `g4/math.g4` 对齐。
2. **数值**：统一 `decimal.Decimal`，除法按 Java 版使用 34 位精度（`DECIMAL128`）。
3. **字符串索引**：Python 按 Unicode 码点，Java 按 UTF-16 code unit；仅影响
   emoji 等星平面字符的 `LEN/LEFT/MID` 等。
4. **日期**：`MyDate` 保留 Java 版的「日期/时间混用」模型；`AddMonths` 采用 C#
   的月末夹取语义（1-31 + 1 月 = 2-29）。
5. **JSON**：自实现宽松解析（单引号、裸键、尾逗号），与 LitJson 行为一致。

## 测试

Java 的 29 个 JUnit 测试类（802 个 `@Test`）已**逐用例移植**为 pytest：

```
tests/
  test_compare.py              ← AlgorithmEngine/CompareTest
  test_csharp.py               ← AlgorithmEngine/CsharpTest
  test_csharp_security.py      ← AlgorithmEngine/CsharpSecurityTest
  test_csharp_web.py           ← AlgorithmEngine/CsharpWebTest
  test_date_times.py           ← AlgorithmEngine/DateTimesTest
  test_engine_basic.py         ← AlgorithmEngine/EngineTest + Cylinder
  test_financial.py            ← AlgorithmEngine/FinancialTest
  test_flow.py                 ← AlgorithmEngine/FlowTest
  test_math_base.py            ← AlgorithmEngine/MathBaseTest
  test_math_sum.py             ← AlgorithmEngine/MathSumTest
  test_math_sum2.py            ← AlgorithmEngine/MathSum2Test
  test_math_transformation.py  ← AlgorithmEngine/MathTransformationTest
  test_math_trigonometric.py   ← AlgorithmEngine/MathTrigonometricTest
  test_operator.py             ← AlgorithmEngine/OperatorTest
  test_strict_mode.py          ← AlgorithmEngine/StrictModeTest
  test_string.py               ← AlgorithmEngine/StringTest
  test_value.py                ← AlgorithmEngine/ValueTest
  test_engine_ex.py            ← algorithmEngineEx/AlgorithmEngineExTest
  test_helper.py               ← algorithmEngineHelper/AlgorithmEngineHelperTest
  test_calculation_logic.py    ← calculationlogic/CalculationLogicTest
  test_calculate_tree.py       ← conditionTrees/CalculateTreeTest
  test_condition_tree.py       ← conditionTrees/ConditionTreeTest
  test_my_date.py              ← helpers/MyDateTest
  test_operands.py             ← operands/OperandsTest
  test_parameter_types.py      ← parameterTypes/ParameterTypeTest
  test_issues.py               ← tests/IssuesTest
  test_unit_conversion.py      ← unitConversion/UnitConversionTest + UnitConversionPrecisionTest
  test_function_cache.py       ← FunctionCacheTest
  # 另有若干面向公共 API 的补充用例
  test_engine.py、test_cache_and_logic.py
```

运行：

```bash
python -m pytest tests -q        # 845 passed, 1 skipped
```

唯一 skip 是 `MyDateTest.ToDateTime_Kind_Test`：Java 版可指定 `DateTimeZone`，
Python 的 `MyDate.to_datetime()` 返回 naive `datetime`，无时区参数，无法等价表达。

跨实现校验（可选）：

```bash
python tests/java_conformance.py
# 直接读取 java/ 下的 JUnit 源码并回放 TryEvaluate/assertEquals
# 941 条断言通过，0 失败
```

> 说明：`java_conformance.py` 是「回放器」，用于防止两份实现漂移；
> 常规回归仍以 `pytest` 为准。两者当前均全绿。
