# ToolGood.Algorithm C++ 移植

本目录是 [`../csharp/ToolGood.Algorithm`](../csharp/ToolGood.Algorithm) 的 C++17 移植版，
目录结构、类型命名与语义均对着 C# 源码逐个对应（`../java`、`../go`、`../python`
的可读性对照仅供辅助）。

## 快速开始

```cpp
#include "toolgood/AlgorithmEngine.h"

using namespace toolgood;

int main() {
    AlgorithmEngine engine;
    Operand r = engine.Calculate("SUM(1,2,3) * 2");
    // r.ToText().TextValue() == "12"
}
```

自定义参数与函数：继承 `AlgorithmEngine` 并重写 `GetParameter` / `ExecuteDiyFunction`。

```cpp
class MyEngine : public AlgorithmEngine {
public:
    Operand GetParameter(const std::string& name) override {
        if (name == "单价") return Operand::Create(Decimal::FromString("12.5"));
        return AlgorithmEngine::GetParameter(name);
    }
    Operand ExecuteDiyFunction(const std::string& name,
                               const std::vector<Operand>& args) override {
        if (name == "myadd") {
            Decimal s;
            for (const auto& a : args) s = s.Add(a.NumberValue());
            return Operand::Create(s);
        }
        return AlgorithmEngine::ExecuteDiyFunction(name, args);
    }
};
```

## 目录

| 路径 | 对应 C# |
| --- | --- |
| `toolgood/BigInt.*` | `System.Numerics`（Decimal 的底层大整数） |
| `toolgood/Decimal.*` | `System.Decimal` |
| `toolgood/MathEx.*` | `System/MathEx.cs`（decimal 级数学函数） |
| `toolgood/Enums.h` | `Enums/` |
| `toolgood/MyDate.*` | `Operands/MyDate.cs` |
| `toolgood/LitJson/` | `LitJson/` |
| `toolgood/Operands/` | `Operand.cs` + `Operands/` |
| `toolgood/Internals/FunctionBase.*` | `Internals/Functions/FunctionBase.cs` + `Function_1..N.cs` |
| `toolgood/Internals/FunctionUtil.*` | `Internals/Functions/FunctionUtil.cs` + `Internals/RegexHelper.cs` |
| `toolgood/Internals/FunctionNames.*` | `g4/math.g4` 里的词法别名（`COVARIANCE.S` 等） |
| `toolgood/Internals/Functions/<组>/` | `Internals/Functions/<组>/` |
| `toolgood/Internals/Registry.cpp` | 由 `MathFunctionVisitor` 的分派表自动生成 |
| `toolgood/Lexer.*`、`toolgood/Parser.*` | `math/mathLexer.cs`、`math/mathParser.cs`、`Internals/Visitors/` |
| `toolgood/AlgorithmEngine.*` | `AlgorithmEngine.cs` |
| `toolgood/MathNet/` | `MathNet/` |
| `toolgood/UnitConversion/` | `UnitConversion/` |
| `toolgood/CalculationLogic/` | `CalculationLogic/` |

## 设计差异（相对 C#）

1. **解析器**：C# 用 ANTLR4 生成的 `mathLexer/mathParser` + 访问者；C++ 用等价的
   手写 `Lexer` + 递归下降 `Parser`，直接生成 `Function` 树，**不依赖 ANTLR 运行时**。
   字符标准化（小写转大写、全角转半角、中文标识符转 `_`）与 C# `CharUtil` 一致。
2. **`Operand`**：C# 是抽象类 + 子类；C++ 是「句柄 + 多态实现」（`std::shared_ptr<OperandBase>`），
   值语义赋值即共享实现。
3. **`Decimal`**：`BigInt` + 标度实现，外部行为对齐 C# `System.Decimal`
   （96 位尾数、标度 ≤ 28、HALF_EVEN 舍入、溢出交由调用方判定）。
4. **`Function`**：`std::shared_ptr<FunctionBase>`；`FunctionN` 合并了 C# 的
   `Function_1..Function_6/Function_N`，参数下标从 1 开始与 C# 一致。
5. **异常**：C# 的 `FormatException/ArgumentException` → `std::runtime_error`；
   运算类失败仍返回错误 `Operand`（与 C# 相同）。

## 构建

无第三方依赖，只需 C++17 编译器（已用 GCC 16.2 / MinGW-w64 验证）。

```bash
g++ -std=c++17 -O2 -Icpp -c cpp/toolgood/*.cpp ...
```

或用 CMake：

```bash
cmake -S cpp -B build && cmake --build build
```

完整构建与测试脚本见 `../temp/cpp-test/build.sh`（测试产物放在 `temp/`，
避免安全软件对仓库目录频繁告警）。

## 已实现的函数

`Internals/Functions/` 下 15 个组共 262 个注册名（301 个类），与 C# 的
`MathFunctionVisitor` 分派表一一对应，另含：
MathNet 统计分布（Normal/StudentT/F/Beta/Gamma/ChiSquare/Binomial/…）、
单位换算、计算逻辑引擎（`CalculationLogicEngine`）、
`AlgorithmEngineEx`、`AlgorithmEngineHelper`。

## 与 C# 的已知差异

差分测试（313 条表达式，见 `../temp/cpp-test/exprs.txt`）当前 **290 条与 C# 完全一致**。

剩余差异集中在两类：

1. **统计分布函数（约 20 条）**：`MathNet` 层用 `double` 实现（`NORMDIST`、`BETADIST`、
   `TDIST`、`GAMMALN`、`ERF`、`BESSELJ` 等），与 C# 的 decimal 版相比在大约第 15~16 位
   有效数字后不同。Excel 本身只保证 15 位，实用上无差别。
2. **错误文案（3 条）**：`ABS()`、`SUM()`、`2^-1` 这类非法输入的报错文字不同
   （C# 是 ANTLR 的 "no viable alternative at input" / "Lexer error"，这里是
   手写解析器的等价报错）。行为一致——都会抛异常。

另有一处结构差异：C# 的 `decimal` 自动截断到 96 位尾数，C++ 的 `Decimal` 在
加减乘除后同样收敛到 96 位/标度 28，因此超大数运算会像 C# 一样返回 `Num error!`。
