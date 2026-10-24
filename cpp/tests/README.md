# C++ 单元测试（移植自 csharp/ToolGood.Algorithm.Test）

C# 测试工程有 **854 个 `[Test]`**，这里是对应的 C++ 版本，**逐方法 1:1 翻译**，
当前 **854/854 全部通过**。

源码随仓库提交（`cpp/tests/`）；编译产物（`.o` / `.exe`）全部落到 `temp/cpp-test/`，
避免安全软件反复扫描仓库内的可执行文件。

## 运行

```bash
# 编译库 + 全部测试并运行（约 2 分钟）
bash cpp/tests/build.sh

# 只跑某个套件 / 某个用例（子串匹配）
bash cpp/tests/build.sh --suite=MathBase
bash cpp/tests/build.sh --filter=round

# 列出全部用例
temp/cpp-test/unit/tg_tests.exe --list
```

输出格式：

```
[FAIL] MathBase.ROUND_test
       MathBaseTest.cpp:123 Objects are not equal
  lhs: 3
  rhs: 2
==== 854/854 tests passed, 0 failed ====
```

## 文件

| 文件 | 说明 |
| --- | --- |
| `cpptest.h` | 测试框架（PetaTest 的 C++ 等价物：`TEST` 宏 + `Assert::*`） |
| `main.cpp` | 入口，支持 `--suite= / --filter= / --list` |
| `build.sh` | 增量编译库与测试、链接并运行（产物写入 `temp/cpp-test/`） |
| `XxxTest.cpp` | 28 个测试文件，对应 C# 的 28 个测试类 |
| `PORTING-GUIDE.md` | 翻译对照表（C# API → C++ API） |
| `tools/ccdiff.cpp` | 与 C# 原库的差分执行器（配 `tools/exprs.txt`） |
| `tools/smoke.cpp` | 端到端冒烟（60 条） |

## 与 C# PetaTest 的对应

| C# | C++ |
| --- | --- |
| `[TestFixture] class XxxTest` | 文件内匿名命名空间 |
| `[Test] void foo()` | `TEST(Xxx, foo) { ... }` |
| `Assert.AreEqual(a, b)` | `Assert::AreEqual(a, b)` |
| `Assert.AreEqual(a, b, within)` | `Assert::AreEqual(a, b, within)`（int 容差也行） |
| `Assert.IsTrue / IsFalse` | `Assert::IsTrue / IsFalse` |
| `Assert.IsNotNull(x)` | `Assert::IsNotNull(x)`（`std::string` 版本以空串代表 C# 的 null） |
| `Assert.Throws<T>(() => ...)` | `Assert::Throws([&]{ ... })` |

断言失败不会中断用例（收集后统一打印），一条用例失败不影响其它用例。

## 覆盖范围

| 套件 | 用例数 | 对应 C# |
| --- | --- | --- |
| Operands | 60 | `Operands/OperandsTest.cs` |
| MyDate | 29 | `Helpers/MyDateTest.cs` |
| ParameterType | 97 | `ParameterTypes/ParameterTypeTest.cs` |
| MathBase | 54 | `AlgorithmEngine/MathBaseTest.cs` |
| MathTrig | 24 | `MathTrigonometricTest.cs` |
| MathTrans | 25 | `MathTransformationTest.cs` |
| MathSum | 41 | `MathSumTest.cs` |
| MathSum2 | 33 | `MathSum2Test.cs` |
| Financial | 21 | `FinancialTest.cs` |
| String | 59 | `StringTest.cs` |
| Csharp | 54 | `CsharpTest.cs` |
| CsharpWeb | 17 | `CsharpWebTest.cs` |
| CsharpSecurity | 13 | `CsharpSecurityTest.cs` |
| DateTimes | 58 | `DateTimesTest.cs` |
| Flow | 28 | `FlowTest.cs` |
| Operator | 7 | `OperatorTest.cs` |
| Value | 40 | `ValueTest.cs` |
| Compare | 5 | `CompareTest.cs` |
| Engine | 5 | `EngineTest.cs`（含 `Cylinder` 自定义引擎） |
| StrictMode | 27 | `StrictModeTest.cs` |
| EngineEx | 27 | `AlgorithmEngineEx/AlgorithmEngineExTest.cs` |
| Helper | 5 | `AlgorithmEngineHelper/AlgorithmEngineHelperTest.cs` |
| CalcLogic | 47 | `CalculationLogic/CalculationLogicTest.cs` |
| ConditionTree | 13 | `ConditionTrees/ConditionTreeTest.cs` |
| CalculateTree | 4 | `ConditionTrees/CalculateTreeTest.cs` |
| FunctionCache | 10 | `FunctionCacheTest.cs` |
| UnitConversion | 38 | `UnitConversion/UnitConversionTest.cs` |
| UnitConversionPrecision | 10 | `UnitConversion/UnitConversionPrecisionTest.cs` |
| Issues | 3 | `Tests/IssuesTest.cs` |

合计 **854**，与 C# 的 `[Test]` 数量一致。

## 翻译时的等价改写（不影响断言语义）

1. **`DateTime` → `MyDate`**：`DateTime.MinValue` → `MyDate()`，`new DateTime(y,m,d)` →
   `MyDate(y,m,d,0,0,0)`；两者都用 `ToLong()` 做相等比较。
2. **`TimeSpan` 无对应类型**：改为 `Calculate(exp)` 后断言 `IsDate()` 与
   `DateValue().ToTimeSpanSeconds()`（共 3 个用例，已在注释中标明）。
3. **`TryEvaluate(exp, null)`**：C# 里返回 `null` 表示失败；C++ 用空串表示，断言相应调整。
4. **`CultureInfo` / `Thread.CurrentCulture`**：C++ 无区域概念，相关代码删除
   （`StringTest.ProperCulture_Test` 只保留 Invariant 语义的断言）。
5. **`decimal` 字面量**：`1.5m` → `Decimal::FromString("1.5")`。
6. **`Assert.AreEqual(x, y, 6)`**：框架用 `Src{line,file}` 结构注入调用点信息，
   避免把整型容差误当成行号参数。

## 这批测试暴露并修复的库缺陷

移植过程中修掉的问题（都已回到库代码里）：

| 问题 | 影响 |
| --- | --- |
| `TRUE()` / `GUID()` 等带括号常量被当成自定义函数 | 3 个用例 |
| 数字单位后缀大小写敏感（`1m` 解析失败）、缺 `L`/`ML` | 22 个用例 |
| `json()` 不支持单引号字符串（LitJson 支持） | 3 个用例 |
| JSON 数字超出 decimal 范围未报错 | 1 个用例 |
| `PARAMETER(...)` 别名未注册 | 1 个用例 |
| 除法结果超出 decimal 范围未返回 `Num error!` | 1 个用例 |
| 比较运算符 `ToString` 丢括号，往返求值语义改变 | 1 个用例 |
| `!=` 在「一侧为 null」时返回 false（C# 返回 true） | 1 个用例 |
| 多数函数组缺 `GetParameterTypes`（参数类型推导全为 NONE） | 60 个用例 |
