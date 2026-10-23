# ToolGood.Algorithm C++ 移植约定（内部契约）

本目录按 `../csharp/ToolGood.Algorithm` 的目录结构组织 C++ 代码。**C# 是主参照**，
`../java/toolgood/algorithm` 与 `../go/algorithm`（已验证的 Go 移植）可作语义对照。

## 1. 目录映射

| C# 目录 | C++ 文件 |
| --- | --- |
| `Enums/` | `toolgood/Enums.h` |
| `System.Decimal` + `MathNet` 依赖 | `toolgood/Decimal.*`、`toolgood/BigInt.*` |
| `Operands/MyDate.cs` | `toolgood/MyDate.*` |
| `LitJson/` | `toolgood/LitJson/JsonData.*` |
| `Operand.cs` + `Operands/` | `toolgood/Operands/Operand.*` |
| `Internals/Functions/FunctionBase.cs`、`Function_1..N.cs` | `toolgood/Internals/FunctionBase.*` |
| `Internals/Functions/FunctionUtil.cs`、`Internals/RegexHelper.cs` | `toolgood/Internals/FunctionUtil.*` |
| `Internals/Functions/<组>/` | `toolgood/Internals/Functions/<组>/<组>.h` + `.cpp` |
| `Internals/Visitors/`（ANTLR） | 由 `toolgood/Lexer.*` + `toolgood/Parser.*` 手写替代 |
| `AlgorithmEngine.cs` | `toolgood/AlgorithmEngine.*` |
| `MathNet/` | `toolgood/MathNet/` |
| `UnitConversion/` | `toolgood/UnitConversion/` |
| `CalculationLogic/` | `toolgood/CalculationLogic/` |

命名空间统一为 `toolgood`。

## 2. 函数组的写法

每个组**只有一个 .h + 一个 .cpp**，头文件已自动生成（见
`toolgood/Internals/Functions/<组>/<组>.h`），签名为：

```cpp
Function NewFunction_ABS(std::vector<Function> args);
```

对应 C++ 实现（`toolgood/Internals/Functions/MathBase/MathBase.cpp`）：

```cpp
#include "MathBase.h"

#include <cmath>

#include "../../../Operands/Operand.h"

namespace toolgood {

namespace {

class Function_ABS : public FunctionN {
public:
    explicit Function_ABS(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "ABS", 1, 1);
    }

    std::string Name() const override { return "ABS"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto a = GetNumber(1, engine, tp);
        if (a.IsErrorOrNone()) return a;
        return Operand::Create(a.NumberValue().Abs());
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

}  // namespace

Function NewFunction_ABS(std::vector<Function> args) {
    return std::make_shared<Function_ABS>(std::move(args));
}

}  // namespace toolgood
```

规则：

- 类名与 C# 完全一致（`Function_ABS`、`Function_Add`），工厂名 `NewFunction_<类名后缀>`。
- 构造函数里用 `RequireArgs(args_, "<Name>", min, max)` 做参数个数校验，
  `max = -1` 表示不限；校验失败抛 `std::runtime_error`（引擎会捕获转成 LastError）。
- **只实现头文件里已声明的工厂**；不要改 `<组>.h`，不要改 `Internals/Registry.cpp`
  （该文件由脚本从 C# `MathFunctionVisitor` 的分派表生成）。
- 辅助方法（下标**从 1 开始**，与 C# 一致）：

| 方法 | 说明 |
| --- | --- |
| `EvalArg(i, engine, tp)` | 求值第 i 个参数 |
| `GetNumber(i, engine, tp)` | 求值并转数字（失败返回错误操作数） |
| `GetText(i, ...)` / `GetBoolean` / `GetDate` / `GetArray` | 同上 |
| `EvaluateAll(engine, tp, out)` | 依次求值全部参数，返回首个错误（无错误返回 `Operand::Null()`） |
| `ParameterError(i)` | `Function 'X' parameter N is error!` |
| `FunctionError()` / `CompareError()` / `Div0Error()` / `NumError()` | 固定错误 |
| `ConvertToNumber(op, i)` 等 | 带参数错误信息的转换 |
| `Arg(i)` / `ArgCount()` | 取原始子节点（0 基）/ 个数 |

- `Name()` 与 C# 的 `Name` 属性保持一致（注意大小写，如 `"Json"`、`"ABS"`）。
- `GetResultType()` 照抄 C# 对应实现。
- `GetParameterTypes(...)` 默认实现已遍历全部子节点（`OperandType::NONE`），
  C# 里有特殊逻辑的再重写。
- 需要自定义表达式还原时重写 `void ToString(std::string& sb, bool addBrackets)`。

## 3. Operand

- 判定：`IsErrorOrNone() IsNone() IsNull() IsNumber() IsText() IsBoolean() IsArray()
  IsDate() IsJson() IsArrayJson() IsError()`
- 取值：`NumberValue() -> Decimal`、`DoubleValue()`、`IntValue()`、`LongValue()`、
  `TextValue()`、`BooleanValue()`、`ArrayValue()`、`DateValue() -> MyDate`、
  `JsonValue() -> const JsonData*`、`ErrorMsg()`
- 转换：`ToNumber(text)`、`ToBoolean`、`ToText`、`ToMyDate`、`ToArray`、`ToJson`（失败返回错误操作数）
- 工厂：`Operand::Create(int / int64_t / double / Decimal / bool / std::string / MyDate / JsonData /
  std::vector<Operand>)`、`Operand::CreateJson(text)`、`Operand::Error(msg)`、
  `Operand::ErrorFormat("Function '{0}' parameter {1} is error!", {Name(), std::to_string(i)})`
- 常量：`Operand::True() / False() / One() / Zero() / Null() / None() / Version()`
- JSON 风格输出：`op.ToString()`

## 4. Decimal

值类型，方法：`Add Sub Mul Div Div32 Mod FloorDiv FloorMod Neg Abs Pow(n) Sqrt Cmp Equal Less
LessOrEqual Greater GreaterOrEqual Sign IsZero IsInteger Trunc Floor Ceil RoundHalfUp(digits)
RoundHalfEven(digits) Float64 Int64 Int String PlainStrip StringFixed(digits) Scale Unscaled`
构造：`Decimal::FromInt/FromUInt/FromString/TryParse/FromDouble`；常量 `Decimal::Zero/One/Max`。

- `Div` 内部按 34 位工作精度计算，结果统一收敛到 C# `System.Decimal` 的表示范围
  （96 位尾数、标度 ≤ 28、HALF_EVEN），与 C# 输出对齐；
  `Mod` 符号随被除数；`FloorMod` 符号随除数（Excel MOD）。
- 加减乘同样收敛到 96 位；超出 `Decimal::Max` 时调用方应返回 `NumError()`（对应 C# OverflowException）。
- C# 的 `Math.Truncate(decimal)` → `Decimal::Trunc()`；`Math.Round(d, n)` → `RoundHalfEven(n)`
  （C# 默认银行家舍入，Excel 函数多数用 RoundHalfUp，请按 C# 原逻辑判断）。
- 三角函数/超越函数用 `std::` + `DoubleValue()`，再 `Operand::Create(double)`。

## 5. MyDate

字段 `Year Month Day Hour Minute Second`（`Year/Month/Day` 为 0 表示未设置）。
`MyDate::TryParse Parse FromTimestamp(timestamp, useLocalTime) ToTimestamp(useLocalTime) Now
Today ToString() ToString(format) AddYears AddMonths AddDays AddHours AddMinutes AddSeconds
DayOfWeek DayOfYear DaysInMonth ToLong ToTimeSpanSeconds HasDate`。

## 6. 引擎与临时参数

```cpp
Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override;
```

- `tp` 可能为 `nullptr`；取参数统一用
  `AlgorithmEngine::GetParameterValue(engine, tp, name)`，或 `PARAM` 的既有实现。
- 引擎设置：`engine.UseLocalTime`、`engine.ExcelIndex`、`engine.DistanceUnit / AreaUnit /
  VolumeUnit / MassUnit`、`engine.UseStrictMode`、`engine.LastError`。

## 7. 移植原则

1. 以 `csharp/ToolGood.Algorithm/Internals/Functions/<组>/Function_XXX.cs` 为主线逐类实现，
   保持语义、错误文案、舍入方式一致，不要"顺手优化"。
2. C# 的 `null` 参数在 C++ 中用空串/0；异常用 `std::runtime_error`，普通失败返回错误操作数。
3. C# 属性 → C++ `Name()`；C# 的 `Operand` 是类，C++ 是值句柄，赋值即共享实现。
4. 中文注释保留；代码里的引号必须是 ASCII 直引号。
5. 完成后必须用下列命令确认本组可编译（`-Icpp` 为工程根）：

```bash
export PATH="/d/git/ToolGood.Algorithm2/temp/w64devkit/bin:$PATH"
cd /d/git/ToolGood.Algorithm2
g++ -std=c++17 -O1 -Wall -Wextra -Wno-unused-parameter -Icpp \
    -c cpp/toolgood/Internals/Functions/<组>/<组>.cpp -o /tmp/<组>.o
```
