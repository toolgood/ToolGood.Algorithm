# C# 单元测试 → C++ 翻译指南

目标：把 `csharp/ToolGood.Algorithm.Test/**/*.cs` 逐方法翻译为
`cpp/tests/<同名>.cpp`，断言与语义保持 1:1。

## 1. 测试框架

`cpp/tests/cpptest.h` 提供 PetaTest 的等价物（参考样例
`cpp/tests/CompareTest.cpp`）：

```cpp
#include "../cpptest.h"
#include "../../../cpp/toolgood/AlgorithmEngine.h"

using namespace toolgood;

namespace {

TEST(SuiteName, method_name) {        // 对应 [Test] public void method_name()
    AlgorithmEngine engine;
    Assert::AreEqual(3, engine.TryEvaluate("1+2", 0));
}

}  // namespace
```

- `TEST(套件, 用例)`：套件名用 C# 文件名去掉 `Test` 后缀（如 `MathBase`）。
- **每个 .cpp 必须整体放在匿名 `namespace { }` 内**，避免同名用例冲突。
- 断言：`Assert::AreEqual / AreEqual(a,b,within) / IsTrue / IsFalse / IsNull /
  IsNotNull / IsEmpty / IsNotEmpty / Throws / GreaterOrEqual / LessOrEqual / Less / Greater`。
- `Assert::IsNotNull(engine.LastError)`：C++ 的 `LastError` 是 `std::string`，
  空串表示"没有错误"，框架已做对应处理。

## 2. 类型与 API 对照

| C# | C++ |
| --- | --- |
| `new AlgorithmEngine()` | `AlgorithmEngine engine;` |
| `engine.TryEvaluate(exp, 0)` | `engine.TryEvaluate(exp, 0)` → `int` |
| `engine.TryEvaluate(exp, 0L)` | `engine.TryEvaluate(exp, (int64_t)0)` |
| `engine.TryEvaluate(exp, 0.0)` | `engine.TryEvaluate(exp, 0.0)` → `double` |
| `engine.TryEvaluate(exp, 0m)` | `engine.TryEvaluate(exp, Decimal::FromInt(0))` → `Decimal` |
| `engine.TryEvaluate(exp, "")` | `engine.TryEvaluate(exp, "")` → `std::string` |
| `engine.TryEvaluate(exp, true)` | `engine.TryEvaluate(exp, true)` → `bool` |
| `engine.TryEvaluate(exp, DateTime.MinValue)` | `engine.TryEvaluate(exp, MyDate())` → `MyDate` |
| `new DateTime(2016,1,2)` | `MyDate(2016, 1, 2, 0, 0, 0)` |
| `DateTime` 比较 | `MyDate` 支持 `Assert::AreEqual`（按 `ToLong()` 比较） |
| `decimal` 字面量 `1.5m` | `Decimal::FromString("1.5")` |
| `Math.Round(x, n)` | `x.RoundHalfEven(n)`（C# 默认银行家舍入） |
| `Math.Truncate(x)` | `x.Trunc()` |
| `Math.Abs(x)` | `x.Abs()` |
| `operand.NumberValue` | `Decimal` |
| `operand.IsError` | `operand.IsError()`（其余同：`IsNumber()` 等） |
| `operand.ErrorMsg` | `operand.ErrorMsg()` |
| `Operand.True` | `Operand::True()`（`False/One/Zero/Null/None/Version` 同理） |
| `Operand.Create(x)` | `Operand::Create(x)`（同样有 int/int64/double/Decimal/string/bool/MyDate/JsonData/vector 重载） |
| `list.Count` | `list.size()` |
| `list[i].Name` | `list[i].Name`（`ParameterType` 的字段是 `Name/Type/Operator/Value`） |
| `Enums.OperandType.NUMBER` | `OperandType::NUMBER` |
| `Assert.Throws<Exception>(() => ...)` | `Assert::Throws([&]{ ... });` |
| `string.Format` | 用 `+` 拼接 |
| `$"{x}"` | `std::to_string(x)`（Decimal 用 `ToString()`） |

注意 `TryEvaluate` 的第二个参数类型决定返回类型，**务必按 C# 的字面量类型选**：
`0`→int、`0.0`→double、`0m`→Decimal、`""`→std::string、`false`→bool。

## 3. 常用库 API

- `AlgorithmEngine`：`Parse/ParseWithoutError/Evaluate/TryEvaluate/Calculate/
  LastError/UseLocalTime/UseStrictMode/ExcelIndex/SetUseExcelIndex(bool)/
  DistanceUnit/AreaUnit/VolumeUnit/MassUnit/GetParameter/ExecuteDiyFunction`
- `FunctionBase`：`Evaluate(engine, tp)`、`ToString()`、`GetParameterTypes(engine)`
  （`tp` 是 `const TempParameter*`，一般不传）
- `Operand`：`IsError/IsNull/IsNone/IsNumber/IsText/IsBoolean/IsArray/IsDate/IsJson/
  IsArrayJson`、`NumberValue/TextValue/BooleanValue/DateValue/ArrayValue/JsonValue/
  ErrorMsg/Type/ToString`
- `Decimal`：`Add/Sub/Mul/Div/Mod/Neg/Abs/Pow/Sqrt/Cmp/Equal/Trunc/Floor/Ceil/
  RoundHalfUp/RoundHalfEven/Float64/Int64/ToString/PlainStrip`
- `MyDate`：`Year/Month/Day/Hour/Minute/Second/ToString()/ToString(fmt)/ToLong()/
  AddDays(...)`

## 4. 编译验证（**不要**运行 build_tests.sh，多人并发会冲突）

只编译自己写的文件，确认语法与类型正确：

```bash
export PATH="/d/git/ToolGood.Algorithm2/temp/w64devkit/bin:$PATH"
cd /d/git/ToolGood.Algorithm2
g++ -std=c++17 -O1 -Wall -Wextra -Wno-unused-parameter -Icpp -Icpp/tests \
    -c cpp/tests/XxxTest.cpp -o /tmp/xxx.o
```

运行由主控统一执行。

## 5. 处理无法直接翻译的内容

- C# 的 `Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR")`：删除，
  或在报告里注明"区域相关"。
- LINQ（`Select/Where/OrderBy/Any`）：改写成循环。
- `List<T>` → `std::vector<T>`；`Dictionary<K,V>` → `std::map`。
- 依赖库中**尚未实现**的 API（例如某函数的 `GetParameterTypes` 覆写缺失）：
  **照 C# 原样翻译**，不要为了通过而改断言；在汇报里列出「因库缺失预计失败」的用例。
- 自定义测试引擎（如 `Cylinder`）照原样继承 `AlgorithmEngine` 并重写
  `GetParameter(string)`（注意签名是 `const std::string&`）。
- C# 的 `null` 字面量在表达式字符串里写作 `null`，不要改。

## 6. 汇报要求

- 已翻译的用例数、文件清单。
- 单文件编译是否 0 error / 0 warning。
- 预期会失败的用例（因库 API 缺失/精度差异），按原因归类。
