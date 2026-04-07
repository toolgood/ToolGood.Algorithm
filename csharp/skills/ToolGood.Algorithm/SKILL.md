***

name: "ToolGood.Algorithm"
description: "一个功能强大的C#数学表达式计算库。当用户需要计算Excel类公式、执行数学运算、处理日期时间操作、字符串操作、金融计算或实现自定义公式引擎时调用。"
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

# ToolGood.Algorithm 库文档

## 概述

ToolGood.Algorithm 是一个功能强大的 C# 库，用于计算数学表达式和 Excel 风格的公式。它全面支持数学函数、日期时间操作、字符串处理、金融计算、统计函数等。该库使用 ANTLR4 解析表达式，支持自定义参数和函数。

## 核心组件

### AlgorithmEngine 类

表达式计算的主要入口点。

**命名空间:** `ToolGood.Algorithm`

**主要属性:**

- `UseLocalTime` (bool): 使用本地时间进行时间戳转换（默认: true）
- `DistanceUnit` (DistanceUnitType): 距离单位类型（默认: M）
- `AreaUnit` (AreaUnitType): 面积单位类型（默认: M2）
- `VolumeUnit` (VolumeUnitType): 体积单位类型（默认: M3）
- `MassUnit` (MassUnitType): 质量单位类型（默认: KG）
- `LastError` (string): 最后的错误信息
- `UseExcelIndex` (bool): 使用 Excel 风格的索引
- `UseParseCache` (bool): 启用解析结果缓存

**主要方法:**

#### Parse

```csharp
public virtual FunctionBase Parse(string exp)
```

将公式表达式编译为可重用的函数。

**参数:**

- `exp` (string): 要解析的公式表达式

**返回值:** `FunctionBase` - 可执行的已编译函数

**异常:** 表达式无效时抛出 `Exception`

#### Evaluate

```csharp
public virtual Operand Evaluate(FunctionBase function)
```

执行已编译的函数。

**参数:**

- `function` (FunctionBase): 要执行的已编译函数

**返回值:** `Operand` - 计算结果

#### TryEvaluate (重载)

```csharp
public virtual int TryEvaluate(string exp, int def)
public virtual long TryEvaluate(string exp, long def)
public virtual double TryEvaluate(string exp, double def)
public virtual decimal TryEvaluate(string exp, decimal def)
public virtual string TryEvaluate(string exp, string def)
public virtual bool TryEvaluate(string exp, bool def)
public virtual DateTime TryEvaluate(string exp, DateTime def)
public virtual TimeSpan TryEvaluate(string exp, TimeSpan def)
```

计算表达式，如果出错则返回默认值。

**参数:**

- `exp` (string): 公式表达式
- `def` (T): 出错时返回的默认值

**返回值:** 计算结果或默认值

#### GetParameter (虚方法)

```csharp
public virtual Operand GetParameter(string parameter)
```

重写此方法以提供自定义参数值。

**参数:**

- `parameter` (string): 参数名称

**返回值:** `Operand` - 参数值

#### ExecuteDiyFunction (虚方法)

```csharp
public virtual Operand ExecuteDiyFunction(string parameter, List<Operand> args)
```

重写此方法以实现自定义函数。

**参数:**

- `parameter` (string): 函数名称
- `args` (List<Operand>): 函数参数

**返回值:** `Operand` - 函数结果

***

### AlgorithmEngineEx 类

带有参数缓存功能的扩展算法引擎。

**命名空间:** `ToolGood.Algorithm`

**额外属性:**

- `IgnoreCase` (bool): 是否忽略参数名大小写
- `UseTempDict` (bool): 将参数保存到临时字典

**额外方法:**

#### ClearParameters

```csharp
public virtual void ClearParameters()
```

清除所有缓存的参数。

#### AddParameter (重载)

```csharp
public virtual void AddParameter(string key, Operand obj)
public virtual void AddParameter(string key, bool obj)
public virtual void AddParameter(string key, short obj)
public virtual void AddParameter(string key, int obj)
public virtual void AddParameter(string key, long obj)
public virtual void AddParameter(string key, ushort obj)
public virtual void AddParameter(string key, uint obj)
public virtual void AddParameter(string key, ulong obj)
public virtual void AddParameter(string key, float obj)
public virtual void AddParameter(string key, double obj)
public virtual void AddParameter(string key, decimal obj)
public virtual void AddParameter(string key, string obj)
public virtual void AddParameter(string key, MyDate obj)
public virtual void AddParameter(string key, DateTime obj)
public virtual void AddParameter(string key, TimeSpan obj)
public virtual void AddParameter(string key, List<Operand> obj)
public virtual void AddParameter(string key, ICollection<string> obj)
public virtual void AddParameter(string key, ICollection<double> obj)
public virtual void AddParameter(string key, ICollection<int> obj)
public virtual void AddParameter(string key, ICollection<bool> obj)
```

向引擎添加自定义参数。

#### AddParameterFromJson

```csharp
public virtual void AddParameterFromJson(string json)
```

从 JSON 对象字符串添加参数。

**参数:**

- `json` (string): 包含参数键值对的 JSON 字符串

***

### AlgorithmEngineHelper 类

公式解析和操作的静态辅助类。

**命名空间:** `ToolGood.Algorithm`

**静态方法:**

#### IsParameter

```csharp
public static bool IsParameter(string parameter)
```

检查字符串是否为有效的参数名。

#### GetDiyNames

```csharp
public static DiyNameInfo GetDiyNames(string exp)
```

从表达式中提取自定义参数和函数。

**返回值:** 包含参数和函数列表的 `DiyNameInfo`

#### ParseFormula

```csharp
public static FunctionBase ParseFormula(string exp)
```

解析公式表达式。

#### CheckFormula

```csharp
public static bool CheckFormula(string exp)
```

验证公式表达式是否有效。

#### ParseCondition

```csharp
public static ConditionTree ParseCondition(string condition)
```

将条件表达式解析为树结构。

**返回值:** 表示解析后条件的 `ConditionTree`

#### ParseCalculate

```csharp
public static CalculateTree ParseCalculate(string exp)
```

将计算表达式解析为树结构。

**返回值:** 表示解析后计算的 `CalculateTree`

#### UnitConversion

```csharp
public static decimal UnitConversion(decimal src, string oldSrcUnit, string oldTarUnit, string name = null)
```

在单位之间转换值。

#### Condition_And / Condition_Or

```csharp
public static FunctionBase Condition_And(FunctionBase left, FunctionBase right)
public static FunctionBase Condition_Or(FunctionBase left, FunctionBase right)
```

使用 AND/OR 逻辑组合条件。

#### Calculate_Add / Calculate_Subtract / Calculate_Multiply / Calculate_Divide / Calculate_Mod / Calculate_Connect

```csharp
public static FunctionBase Calculate_Add(FunctionBase left, FunctionBase right)
public static FunctionBase Calculate_Subtract(FunctionBase left, FunctionBase right)
public static FunctionBase Calculate_Multiply(FunctionBase left, FunctionBase right)
public static FunctionBase Calculate_Divide(FunctionBase left, FunctionBase right)
public static FunctionBase Calculate_Mod(FunctionBase left, FunctionBase right)
public static FunctionBase Calculate_Connect(FunctionBase left, FunctionBase right)
```

创建算术运算函数。

***

### Operand 类

表示所有可能操作数类型的抽象基类。

**命名空间:** `ToolGood.Algorithm`

**静态属性:**

- `Version`: 库版本字符串
- `True`: 布尔真操作数
- `False`: 布尔假操作数
- `One`: 数值 1 操作数
- `Zero`: 数值 0 操作数
- `Null`: 空值操作数
- `None`: 未定义操作数

**类型检查属性:**

- `IsNone`: 操作数是否未定义
- `IsNull`: 操作数是否为空值
- `IsNumber`: 操作数是否为数值
- `IsText`: 操作数是否为字符串
- `IsBoolean`: 操作数是否为布尔值
- `IsArray`: 操作数是否为数组
- `IsDate`: 操作数是否为日期
- `IsJson`: 操作数是否为 JSON 对象
- `IsArrayJson`: 操作数是否为 JSON 数组
- `IsError`: 操作数是否表示错误
- `ErrorMsg`: 如果 IsError 为 true 时的错误信息
- `Type`: `OperandType` 枚举值

**值属性:**

- `NumberValue` (decimal): 数值
- `DoubleValue` (double): 双精度值
- `IntValue` (int): 整数值
- `LongValue` (long): 长整数值
- `TextValue` (string): 字符串值
- `BooleanValue` (bool): 布尔值
- `ArrayValue` (List<Operand>): 数组值
- `DateValue` (MyDate): 日期值

**静态工厂方法:**

```csharp
public static Operand Create(bool obj)
public static Operand Create(short obj)
public static Operand Create(int obj)
public static Operand Create(long obj)
public static Operand Create(ushort obj)
public static Operand Create(uint obj)
public static Operand Create(ulong obj)
public static Operand Create(float obj)
public static Operand Create(double obj)
public static Operand Create(decimal obj)
public static Operand Create(string obj)
public static Operand Create(MyDate obj)
public static Operand Create(DateTime obj)
public static Operand Create(TimeSpan obj)
public static Operand Create(List<Operand> obj)
public static Operand Create(ICollection<string> obj)
public static Operand Create(ICollection<double> obj)
public static Operand Create(ICollection<int> obj)
public static Operand Create(ICollection<bool> obj)
public static Operand CreateJson(string txt)
public static Operand Error(string msg)
public static Operand Error(string msg, params object[] args)
```

**转换方法:**

```csharp
public virtual Operand ToNumber(string errorMessage = null)
public virtual Operand ToBoolean(string errorMessage = null)
public virtual Operand ToText(string errorMessage = null)
public virtual Operand ToMyDate(string errorMessage = null)
public virtual Operand ToArray(string errorMessage = null)
public virtual Operand ToJson(string errorMessage = null)
```

***

### FunctionBase 类

所有函数实现的抽象基类。

**命名空间:** `ToolGood.Algorithm.Internals.Functions`

**主要属性:**

- `Name` (string): 函数名称

**主要方法:**

```csharp
public abstract Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter = null)
public abstract OperandType GetResultType()
public List<ParameterType> GetParameterTypes(AlgorithmEngine engine)
```

**TryEvaluate 方法:**

```csharp
public int TryEvaluate(AlgorithmEngine engine, int def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
public decimal TryEvaluate(AlgorithmEngine engine, decimal def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
public string TryEvaluate(AlgorithmEngine engine, string def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
public bool TryEvaluate(AlgorithmEngine engine, bool def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
public DateTime TryEvaluate(AlgorithmEngine engine, DateTime def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
public TimeSpan TryEvaluate(AlgorithmEngine engine, TimeSpan def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
```

***

## 枚举类型

### OperandType

```csharp
public enum OperandType : byte
{
    NONE,      // 未定义
    NULL,      // 空值
    ERROR,     // 错误
    DATE,      // 日期时间
    ARRAY,     // 数组
    NUMBER,    // 数值
    BOOLEAN,   // 布尔值
    TEXT,      // 字符串
    JSON,      // JSON 对象
    ARRAYJSON  // JSON 数组
}
```

### ConditionTreeType

```csharp
public enum ConditionTreeType
{
    String,  // 文本/叶节点
    And,     // AND 运算
    Or,      // OR 运算
    Error    // 错误状态
}
```

### CalculateTreeType

```csharp
public enum CalculateTreeType
{
    String,    // 文本/叶节点
    Add,       // 加法
    Sub,       // 减法
    Mul,       // 乘法
    Div,       // 除法
    Mod,       // 取模
    Connect,   // 字符串连接
    Error      // 错误状态
}
```

### 单位类型

```csharp
public enum DistanceUnitType { M, KM, DM, CM, MM }
public enum AreaUnitType { M2, KM2, DM2, CM2, MM2 }
public enum VolumeUnitType { M3, KM3, DM3, CM3, MM3, L, ML }
public enum MassUnitType { G, KG, T }
```

***

## 支持的函数

### 数学函数 (MathBase)

| 函数          | 签名                                | 描述               |
| ------------- | ----------------------------------- | ------------------ |
| `Abs`         | `Abs(number)`                       | 绝对值             |
| `Ceiling`     | `Ceiling(number)`                   | 向上取整           |
| `Floor`       | `Floor(number)`                     | 向下取整           |
| `Round`       | `Round(number, digits)`             | 四舍五入到指定位数 |
| `RoundDown`   | `RoundDown(number, digits)`         | 向下舍入到指定位数 |
| `RoundUp`     | `RoundUp(number, digits)`           | 向上舍入到指定位数 |
| `Trunc`       | `Trunc(number)`                     | 截断为整数         |
| `Int`         | `Int(number)`                       | 整数部分           |
| `Sign`        | `Sign(number)`                      | 数字符号 (-1, 0, 1)|
| `Sqrt`        | `Sqrt(number)`                      | 平方根             |
| `SqrtPi`      | `SqrtPi(number)`                    | (number * PI) 的平方根 |
| `Power`       | `Power(base, exponent)`             | 幂运算             |
| `Exp`         | `Exp(number)`                       | e 的幂次方         |
| `Ln`          | `Ln(number)`                        | 自然对数           |
| `Log`         | `Log(number, base)`                 | 指定底数的对数     |
| `Log10`       | `Log10(number)`                     | 以 10 为底的对数   |
| `Fact`        | `Fact(number)`                      | 阶乘               |
| `FactDouble`  | `FactDouble(number)`                | 双阶乘             |
| `Combin`      | `Combin(n, k)`                      | 组合数             |
| `Permut`      | `Permut(n, k)`                      | 排列数             |
| `GCD`         | `GCD(number1, number2, ...)`        | 最大公约数         |
| `LCM`         | `LCM(number1, number2, ...)`        | 最小公倍数         |
| `Mod`         | `Mod(number, divisor)`              | 取模               |
| `Quotient`    | `Quotient(numerator, denominator)`  | 整除               |
| `Product`     | `Product(number1, number2, ...)`    | 乘积               |
| `Even`        | `Even(number)`                      | 向上舍入到最近的偶数 |
| `Odd`         | `Odd(number)`                       | 向上舍入到最近的奇数 |
| `MRound`      | `MRound(number, multiple)`          | 舍入到指定倍数     |
| `Multinomial` | `Multinomial(number1, number2, ...)`| 多项式系数         |
| `Percentage`  | `Percentage(number)`                | 转换为百分比       |
| `Rand`        | `Rand()`                            | 随机数 [0, 1)      |
| `RandBetween` | `RandBetween(bottom, top)`          | 指定范围内的随机整数 |
| `Delta`       | `Delta(number1, number2)`           | 克罗内克 delta     |
| `GeStep`      | `GeStep(number, step)`              | 测试是否 >= step   |

### 三角函数 (MathTrigonometric)

| 函数      | 签名            | 描述               |
| --------- | --------------- | ------------------ |
| `Sin`     | `Sin(angle)`    | 正弦               |
| `Cos`     | `Cos(angle)`    | 余弦               |
| `Tan`     | `Tan(angle)`    | 正切               |
| `Asin`    | `Asin(number)`  | 反正弦             |
| `Acos`    | `Acos(number)`  | 反余弦             |
| `Atan`    | `Atan(number)`  | 反正切             |
| `Atan2`   | `Atan2(y, x)`   | y/x 的反正切       |
| `Sinh`    | `Sinh(number)`  | 双曲正弦           |
| `Cosh`    | `Cosh(number)`  | 双曲余弦           |
| `Tanh`    | `Tanh(number)`  | 双曲正切           |
| `Asinh`   | `Asinh(number)` | 反双曲正弦         |
| `Acosh`   | `Acosh(number)` | 反双曲余弦         |
| `Atanh`   | `Atanh(number)` | 反双曲正切         |
| `Cot`     | `Cot(angle)`    | 余切               |
| `Coth`    | `Coth(number)`  | 双曲余切           |
| `Acot`    | `Acot(number)`  | 反余切             |
| `Acoth`   | `Acoth(number)` | 反双曲余切         |
| `Csc`     | `Csc(angle)`    | 余割               |
| `Csch`    | `Csch(number)`  | 双曲余割           |
| `Sec`     | `Sec(angle)`    | 正割               |
| `Sech`    | `Sech(number)`  | 双曲正割           |
| `Degrees` | `Degrees(angle)`| 弧度转角度         |
| `Radians` | `Radians(angle)`| 角度转弧度         |

### 统计函数 (MathSum)

| 函数          | 签名                                    | 描述             |
| ------------- | --------------------------------------- | ---------------- |
| `Sum`         | `Sum(number1, number2, ...)`            | 求和             |
| `SumIf`       | `SumIf(range, criteria, sumRange)`      | 条件求和         |
| `SumProduct`  | `SumProduct(array1, array2, ...)`       | 乘积之和         |
| `SumSq`       | `SumSq(number1, number2, ...)`          | 平方和           |
| `Average`     | `Average(number1, number2, ...)`        | 算术平均值       |
| `AverageIf`   | `AverageIf(range, criteria, avgRange)`  | 条件平均值       |
| `Count`       | `Count(value1, value2, ...)`            | 计数             |
| `CountIf`     | `CountIf(range, criteria)`              | 条件计数         |
| `Max`         | `Max(number1, number2, ...)`            | 最大值           |
| `Min`         | `Min(number1, number2, ...)`            | 最小值           |
| `Median`      | `Median(number1, number2, ...)`         | 中位数           |
| `Mode`        | `Mode(number1, number2, ...)`           | 众数             |
| `StDev`       | `StDev(number1, number2, ...)`          | 样本标准差       |
| `StDevP`      | `StDevP(number1, number2, ...)`         | 总体标准差       |
| `Var`         | `Var(number1, number2, ...)`            | 样本方差         |
| `VarP`        | `VarP(number1, number2, ...)`           | 总体方差         |
| `Large`       | `Large(array, k)`                       | 第 k 大的值      |
| `Small`       | `Small(array, k)`                       | 第 k 小的值      |
| `Rank`        | `Rank(number, array, order)`            | 数值的排名       |
| `Percentile`  | `Percentile(array, k)`                  | 第 k 百分位数    |
| `PercentRank` | `PercentRank(array, value)`             | 百分比排名       |
| `Quartile`    | `Quartile(array, quart)`                | 四分位数         |
| `Correl`      | `Correl(array1, array2)`                | 相关系数         |
| `Covar`       | `Covar(array1, array2)`                 | 协方差           |
| `Forecast`    | `Forecast(x, knownY, knownX)`           | 线性预测         |
| `Intercept`   | `Intercept(knownY, knownX)`             | 线性回归截距     |
| `Slope`       | `Slope(knownY, knownX)`                 | 线性回归斜率     |
| `Pearson`     | `Pearson(array1, array2)`               | 皮尔逊相关系数   |
| `GeoMean`     | `GeoMean(number1, number2, ...)`        | 几何平均值       |
| `HarMean`     | `HarMean(number1, number2, ...)`        | 调和平均值       |
| `AveDev`      | `AveDev(number1, number2, ...)`         | 平均偏差         |
| `DevSq`       | `DevSq(number1, number2, ...)`          | 偏差平方和       |
| `Poisson`     | `Poisson(x, mean, cumulative)`          | 泊松分布         |
| `SeriesSum`   | `SeriesSum(x, n, m, coefficients)`      | 幂级数求和       |

### 统计分布函数 (MathSum2)

| 函数            | 签名                                           | 描述               |
| --------------- | ---------------------------------------------- | ------------------ |
| `NormDist`      | `NormDist(x, mean, sd, cumulative)`            | 正态分布           |
| `NormInv`       | `NormInv(probability, mean, sd)`               | 反正态分布         |
| `NormSDist`     | `NormSDist(z)`                                 | 标准正态分布       |
| `NormSInv`      | `NormSInv(probability)`                        | 反标准正态分布     |
| `TDist`         | `TDist(x, degrees, tails)`                     | 学生 t 分布        |
| `TInv`          | `TInv(probability, degrees)`                   | 反 t 分布          |
| `FDist`         | `FDist(x, df1, df2)`                           | F 分布             |
| `FInv`          | `FInv(probability, df1, df2)`                  | 反 F 分布          |
| `BetaDist`      | `BetaDist(x, alpha, beta, A, B)`               | Beta 分布          |
| `BetaInv`       | `BetaInv(probability, alpha, beta, A, B)`      | 反 Beta 分布       |
| `GammaDist`     | `GammaDist(x, alpha, beta, cumulative)`        | Gamma 分布         |
| `GammaInv`      | `GammaInv(probability, alpha, beta)`           | 反 Gamma 分布      |
| `GammaLn`       | `GammaLn(x)`                                   | Gamma 函数的自然对数 |
| `ChiDist`       | `ChiDist(x, degrees)`                          | 卡方分布           |
| `ChiInv`        | `ChiInv(probability, degrees)`                 | 反卡方分布         |
| `ExponDist`     | `ExponDist(x, lambda, cumulative)`             | 指数分布           |
| `LogNormDist`   | `LogNormDist(x, mean, sd)`                     | 对数正态分布       |
| `LogInv`        | `LogInv(probability, mean, sd)`                | 反对数正态分布     |
| `BinomDist`     | `BinomDist(s, trials, probability, cumulative)`| 二项分布           |
| `NegBinomDist`  | `NegBinomDist(f, s, probability)`              | 负二项分布         |
| `HyperGeomDist` | `HyperGeomDist(s, sample, population, successes)` | 超几何分布      |
| `Weibull`       | `Weibull(x, alpha, beta, cumulative)`          | 威布尔分布         |
| `Fisher`        | `Fisher(x)`                                    | Fisher 变换        |
| `FisherInv`     | `FisherInv(y)`                                 | 反 Fisher 变换     |
| `BesselI`       | `BesselI(x, n)`                                | 修正贝塞尔函数 I   |
| `BesselJ`       | `BesselJ(x, n)`                                | 贝塞尔函数 J       |
| `BesselK`       | `BesselK(x, n)`                                | 修正贝塞尔函数 K   |
| `BesselY`       | `BesselY(x, n)`                                | 贝塞尔函数 Y       |
| `Erf`           | `Erf(x)`                                       | 误差函数           |
| `ErfC`          | `ErfC(x)`                                      | 互补误差函数       |

### 日期时间函数 (DateTimes)

| 函数          | 签名                                     | 描述               |
| ------------- | ---------------------------------------- | ------------------ |
| `Now`         | `Now()`                                  | 当前日期时间       |
| `Today`       | `Today()`                                | 当前日期           |
| `Date`        | `Date(year, month, day)`                 | 创建日期           |
| `Time`        | `Time(hour, minute, second)`             | 创建时间           |
| `Year`        | `Year(date)`                             | 提取年份           |
| `Month`       | `Month(date)`                            | 提取月份           |
| `Day`         | `Day(date)`                              | 提取日             |
| `Hour`        | `Hour(time)`                             | 提取小时           |
| `Minute`      | `Minute(time)`                           | 提取分钟           |
| `Second`      | `Second(time)`                           | 提取秒             |
| `Weekday`     | `Weekday(date, returnType)`              | 星期几             |
| `WeekNum`     | `WeekNum(date, returnType)`              | 周数               |
| `DateValue`   | `DateValue(dateText)`                    | 文本转日期         |
| `TimeValue`   | `TimeValue(timeText)`                    | 文本转时间         |
| `TimeStamp`   | `TimeStamp(date)`                        | Unix 时间戳        |
| `AddYears`    | `AddYears(date, years)`                  | 添加年份           |
| `AddMonths`   | `AddMonths(date, months)`                | 添加月份           |
| `AddDays`     | `AddDays(date, days)`                    | 添加天数           |
| `AddHours`    | `AddHours(date, hours)`                  | 添加小时           |
| `AddMinutes`  | `AddMinutes(date, minutes)`              | 添加分钟           |
| `AddSeconds`  | `AddSeconds(date, seconds)`              | 添加秒数           |
| `Days`        | `Days(endDate, startDate)`               | 日期间隔天数       |
| `Days360`     | `Days360(startDate, endDate, method)`    | 按 360 天计算天数  |
| `DateDif`     | `DateDif(startDate, endDate, unit)`      | 日期差             |
| `EDate`       | `EDate(startDate, months)`               | 日期加月数         |
| `EOMonth`     | `EOMonth(startDate, months)`             | 月末日期           |
| `NetworkDays` | `NetworkDays(startDate, endDate, holidays)` | 工作日数        |
| `WorkDay`     | `WorkDay(startDate, days, holidays)`     | 工作日             |
| `YearFrac`    | `YearFrac(startDate, endDate, basis)`    | 年份分数           |

### 字符串函数 (String)

| 函数          | 签名                                        | 描述                 |
| ------------- | ------------------------------------------- | -------------------- |
| `Concatenate` | `Concatenate(text1, text2, ...)`            | 连接字符串           |
| `Left`        | `Left(text, numChars)`                      | 左侧字符             |
| `Right`       | `Right(text, numChars)`                     | 右侧字符             |
| `Mid`         | `Mid(text, start, numChars)`                | 中间字符             |
| `Len`         | `Len(text)`                                 | 字符串长度           |
| `Lower`       | `Lower(text)`                               | 转小写               |
| `Upper`       | `Upper(text)`                               | 转大写               |
| `Proper`      | `Proper(text)`                              | 首字母大写           |
| `Trim`        | `Trim(text)`                                | 去除首尾空格         |
| `TrimStart`   | `TrimStart(text)`                           | 去除前导空格         |
| `TrimEnd`     | `TrimEnd(text)`                             | 去除尾部空格         |
| `Find`        | `Find(findText, withinText, start)`         | 查找文本（区分大小写）|
| `Search`      | `Search(findText, withinText, start)`       | 搜索文本（不区分大小写）|
| `Replace`     | `Replace(text, start, numChars, newText)`   | 替换字符             |
| `Substitute`  | `Substitute(text, oldText, newText, instance)` | 替换文本          |
| `Rept`        | `Rept(text, times)`                         | 重复文本             |
| `Text`        | `Text(value, format)`                       | 格式化为文本         |
| `Value`       | `Value(text)`                               | 转换为数字           |
| `T`           | `T(value)`                                  | 转换为文本           |
| `Char`        | `Char(number)`                              | 从代码获取字符       |
| `UniChar`     | `UniChar(number)`                           | Unicode 字符         |
| `Code`        | `Code(text)`                                | 字符代码             |
| `Unicode`     | `Unicode(text)`                             | Unicode 代码         |
| `Exact`       | `Exact(text1, text2)`                       | 精确比较字符串       |
| `Clean`       | `Clean(text)`                               | 删除非打印字符       |
| `Asc`         | `Asc(text)`                                 | 转单字节             |
| `Jis`         | `Jis(text)`                                 | 转双字节             |
| `RMB`         | `RMB(number)`                               | 转换为人民币大写     |

### 流程控制函数 (Flow)

| 函数                 | 签名                                                            | 描述             |
| -------------------- | --------------------------------------------------------------- | ---------------- |
| `If`                 | `If(condition, trueValue, falseValue)`                          | 条件判断         |
| `Ifs`                | `Ifs(condition1, value1, condition2, value2, ...)`              | 多条件判断       |
| `IfError`            | `IfError(value, errorValue)`                                    | 错误处理         |
| `Switch`             | `Switch(expression, value1, result1, value2, result2, ..., default)` | Switch 语句 |
| `Not`                | `Not(logical)`                                                  | 逻辑非           |
| `IsNull`             | `IsNull(value)`                                                 | 检查是否为空     |
| `IsNullOrEmpty`      | `IsNullOrEmpty(value)`                                          | 检查是否为空或空字符串 |
| `IsNullOrWhiteSpace` | `IsNullOrWhiteSpace(value)`                                     | 检查是否为空或空白 |
| `IsNullOrError`      | `IsNullOrError(value)`                                          | 检查是否为空或错误 |
| `IsError`            | `IsError(value)`                                                | 检查是否为错误   |
| `IsNumber`           | `IsNumber(value)`                                               | 检查是否为数字   |
| `IsText`             | `IsText(value)`                                                 | 检查是否为文本   |
| `IsLogical`          | `IsLogical(value)`                                              | 检查是否为布尔值 |
| `IsNonText`          | `IsNonText(value)`                                              | 检查是否非文本   |
| `IsEven`             | `IsEven(number)`                                                | 检查是否为偶数   |
| `IsOdd`              | `IsOdd(number)`                                                 | 检查是否为奇数   |
| `HasValue`           | `HasValue(value)`                                               | 检查是否有值     |

### 金融函数 (Financial)

| 函数   | 签名                                        | 描述             |
| ------ | ------------------------------------------- | ---------------- |
| `PMT`  | `PMT(rate, nper, pv, fv, type)`             | 贷款每期付款额   |
| `PV`   | `PV(rate, nper, pmt, fv, type)`             | 现值             |
| `FV`   | `FV(rate, nper, pmt, pv, type)`             | 未来值           |
| `NPer` | `NPer(rate, pmt, pv, fv, type)`             | 期数             |
| `Rate` | `Rate(nper, pmt, pv, fv, type, guess)`      | 利率             |
| `IPmt` | `IPmt(rate, per, nper, pv, fv, type)`       | 利息付款         |
| `PPmt` | `PPmt(rate, per, nper, pv, fv, type)`       | 本金付款         |
| `NPV`  | `NPV(rate, value1, value2, ...)`            | 净现值           |
| `XNPV` | `XNPV(rate, values, dates)`                 | 带日期的净现值   |
| `IRR`  | `IRR(values, guess)`                        | 内部收益率       |
| `XIRR` | `XIRR(values, dates, guess)`                | 带日期的内部收益率 |
| `MIRR` | `MIRR(values, financeRate, reinvestRate)`   | 修正内部收益率   |
| `SLN`  | `SLN(cost, salvage, life)`                  | 直线折旧         |
| `DB`   | `DB(cost, salvage, life, period, month)`    | 余额递减折旧     |
| `DDB`  | `DDB(cost, salvage, life, period, factor)`  | 双倍余额递减折旧 |
| `SYD`  | `SYD(cost, salvage, life, period)`          | 年数总和折旧     |

### 比较函数 (Compare)

| 函数 | 签名              | 描述       |
| ---- | ----------------- | ---------- |
| `EQ` | `value1 == value2`| 等于       |
| `NE` | `value1 != value2`| 不等于     |
| `GT` | `value1 > value2` | 大于       |
| `GE` | `value1 >= value2`| 大于等于   |
| `LT` | `value1 < value2` | 小于       |
| `LE` | `value1 <= value2`| 小于等于   |

### C# 扩展函数 (Csharp)

| 函数          | 签名                                      | 描述             |
| ------------- | ----------------------------------------- | ---------------- |
| `StartsWith`  | `StartsWith(text, prefix)`                | 检查前缀         |
| `EndsWith`    | `EndsWith(text, suffix)`                  | 检查后缀         |
| `IndexOf`     | `IndexOf(text, value, start)`             | 子字符串索引     |
| `LastIndexOf` | `LastIndexOf(text, value, start)`         | 最后子字符串索引 |
| `Substring`   | `Substring(text, start, length)`          | 提取子字符串     |
| `Split`       | `Split(text, separator)`                  | 分割字符串       |
| `Join`        | `Join(separator, array)`                  | 连接数组         |
| `RemoveStart` | `RemoveStart(text, prefix)`               | 删除前缀         |
| `RemoveEnd`   | `RemoveEnd(text, suffix)`                 | 删除后缀         |
| `Has`         | `Has(text, value)`                        | 包含子字符串     |
| `Guid`        | `Guid()`                                  | 生成 GUID        |
| `Regex`       | `Regex(text, pattern)`                    | 正则匹配         |
| `IsRegex`     | `IsRegex(text, pattern)`                  | 检查正则匹配     |
| `RegexReplace`| `RegexReplace(text, pattern, replacement)`| 正则替换         |
| `LookCeiling` | `LookCeiling(text, value)`                | 向上查找         |
| `LookFloor`   | `LookFloor(text, value)`                  | 向下查找         |

### 安全函数 (CsharpSecurity)

| 函数         | 签名                 | 描述        |
| ------------ | -------------------- | ----------- |
| `MD5`        | `MD5(text)`          | MD5 哈希    |
| `SHA1`       | `SHA1(text)`         | SHA1 哈希   |
| `SHA256`     | `SHA256(text)`       | SHA256 哈希 |
| `SHA512`     | `SHA512(text)`       | SHA512 哈希 |
| `HMACMD5`    | `HMACMD5(text, key)` | HMAC-MD5    |
| `HMACSHA1`   | `HMACSHA1(text, key)`| HMAC-SHA1   |
| `HMACSHA256` | `HMACSHA256(text, key)` | HMAC-SHA256 |
| `HMACSHA512` | `HMACSHA512(text, key)` | HMAC-SHA512 |

### Web 函数 (CsharpWeb)

| 函数              | 签名                     | 描述                 |
| ----------------- | ------------------------ | -------------------- |
| `HtmlEncode`      | `HtmlEncode(text)`       | HTML 编码            |
| `HtmlDecode`      | `HtmlDecode(text)`       | HTML 解码            |
| `UrlEncode`       | `UrlEncode(text)`        | URL 编码             |
| `UrlDecode`       | `UrlDecode(text)`        | URL 解码             |
| `TextToBase64`    | `TextToBase64(text)`     | 转换为 Base64        |
| `Base64ToText`    | `Base64ToText(base64)`   | 从 Base64 转换       |
| `TextToBase64Url` | `TextToBase64Url(text)`  | 转换为 URL 安全 Base64 |
| `Base64UrlToText` | `Base64UrlToText(base64)`| 从 URL 安全 Base64 转换 |

### 数值转换函数 (MathTransformation)

| 函数      | 签名                     | 描述             |
| --------- | ------------------------ | ---------------- |
| `Bin2Dec` | `Bin2Dec(binary)`        | 二进制转十进制   |
| `Bin2Hex` | `Bin2Hex(binary, places)`| 二进制转十六进制 |
| `Bin2Oct` | `Bin2Oct(binary, places)`| 二进制转八进制   |
| `Dec2Bin` | `Dec2Bin(number, places)`| 十进制转二进制   |
| `Dec2Hex` | `Dec2Hex(number, places)`| 十进制转十六进制 |
| `Dec2Oct` | `Dec2Oct(number, places)`| 十进制转八进制   |
| `Hex2Bin` | `Hex2Bin(hex, places)`   | 十六进制转二进制 |
| `Hex2Dec` | `Hex2Dec(hex)`           | 十六进制转十进制 |
| `Hex2Oct` | `Hex2Oct(hex, places)`   | 十六进制转八进制 |
| `Oct2Bin` | `Oct2Bin(octal, places)` | 八进制转二进制   |
| `Oct2Dec` | `Oct2Dec(octal)`         | 八进制转十进制   |
| `Oct2Hex` | `Oct2Hex(octal, places)` | 八进制转十六进制 |
| `Roman`   | `Roman(number, form)`    | 转换为罗马数字   |
| `Arabic`  | `Arabic(roman)`          | 从罗马数字转换   |

### JSON 函数 (Value)

| 函数            | 签名                             | 描述             |
| --------------- | -------------------------------- | ---------------- |
| `JSON`          | `JSON(text)`                     | 解析 JSON        |
| `GetJsonValue`  | `GetJsonValue(json, path)`       | 按路径获取 JSON 值 |
| `ArrayJson`     | `ArrayJson(jsonArray)`           | 创建 JSON 数组   |
| `ArrayJsonItem` | `ArrayJsonItem(jsonArray, index)`| 获取 JSON 数组元素 |

### 数组函数 (Value)

| 函数    | 签名                        | 描述       |
| ------- | --------------------------- | ---------- |
| `Array` | `Array(value1, value2, ...)`| 创建数组   |

### 运算符

| 运算符      | 描述               |
| ----------- | ------------------ |
| `+`         | 加法 / 字符串连接  |
| `-`         | 减法 / 负号        |
| `*`         | 乘法               |
| `/`         | 除法               |
| `%`         | 取模               |
| `&`         | 字符串连接         |
| `=` / `==`  | 等于               |
| `!=` / `<>` | 不等于             |
| `>`         | 大于               |
| `>=`        | 大于等于           |
| `<`         | 小于               |
| `<=`        | 小于等于           |
| `&&`        | 逻辑与             |
| `||`        | 逻辑或             |
| `!`         | 逻辑非             |
| `? :`       | 三元条件运算符     |

***

## 表达式语法

### 字面量

```
123              // 整数
123.45           // 小数
123.45E-10       // 科学计数法
123.45M          // 带单位的小数（米）
123.45KG         // 带单位的小数（千克）
'Hello'          // 字符串（单引号）
"Hello"          // 字符串（双引号）
`Hello`          // 字符串（反引号）
true / false     // 布尔值
null             // 空值
```

### 带单位的数字

```
100M             // 100 米
50KG             // 50 千克
25M2             // 25 平方米
10L              // 10 升
```

### JSON 字面量

```
{key1: value1, key2: value2}    // JSON 对象
[value1, value2, value3]        // JSON 数组
```

### 参数访问

```
ParameterName                    // 简单参数
ParameterName.SubProperty        // 嵌套属性
ParameterName[0]                 // 数组索引
ParameterName['key']             // 字典键
```

### 函数调用

```
FunctionName()                   // 无参数
FunctionName(arg1)               // 一个参数
FunctionName(arg1, arg2, arg3)   // 多个参数
```

### 注释

```
// 单行注释
/* 多行注释 */
```

***

## 使用示例

### 基本用法

```csharp
using ToolGood.Algorithm;

// 创建引擎实例
var engine = new AlgorithmEngine();

// 解析并计算
var result = engine.TryEvaluate("1 + 2 * 3", 0);
Console.WriteLine(result); // 输出: 7

// 解析一次，多次执行
var func = engine.Parse("Abs(-5) + Sqrt(16)");
var operand = engine.Evaluate(func);
Console.WriteLine(operand.NumberValue); // 输出: 9
```

### 自定义参数

```csharp
// 方法一：重写 GetParameter
public class MyEngine : AlgorithmEngine
{
    private Dictionary<string, object> _data;
    
    public MyEngine(Dictionary<string, object> data)
    {
        _data = data;
    }
    
    public override Operand GetParameter(string parameter)
    {
        if (_data.TryGetValue(parameter, out var value))
        {
            return value switch
            {
                int i => Operand.Create(i),
                double d => Operand.Create(d),
                string s => Operand.Create(s),
                bool b => Operand.Create(b),
                DateTime dt => Operand.Create(dt),
                _ => Operand.Null
            };
        }
        return base.GetParameter(parameter);
    }
}

// 方法二：使用 AlgorithmEngineEx
var engine = new AlgorithmEngineEx();
engine.AddParameter("Price", 100.0);
engine.AddParameter("Quantity", 5);
engine.AddParameter("Discount", 0.1);

var total = engine.TryEvaluate("Price * Quantity * (1 - Discount)", 0.0);
Console.WriteLine(total); // 输出: 450.0
```

### 自定义函数

```csharp
public class MyEngine : AlgorithmEngine
{
    public override Operand ExecuteDiyFunction(string functionName, List<Operand> args)
    {
        switch (functionName.ToUpper())
        {
            case "DOUBLE":
                if (args.Count == 1 && args[0].IsNumber)
                {
                    return Operand.Create(args[0].NumberValue * 2);
                }
                return Operand.Error("DOUBLE 需要一个数值参数");
                
            case "GREET":
                if (args.Count == 1 && args[0].IsText)
                {
                    return Operand.Create($"你好, {args[0].TextValue}!");
                }
                return Operand.Error("GREET 需要一个文本参数");
        }
        return base.ExecuteDiyFunction(functionName, args);
    }
}

var engine = new MyEngine();
var result = engine.TryEvaluate("DOUBLE(5) + 1", 0);
Console.WriteLine(result); // 输出: 11
```

### 日期时间操作

```csharp
var engine = new AlgorithmEngine();

// 当前日期
var today = engine.TryEvaluate("Today()", DateTime.MinValue);

// 日期运算
var futureDate = engine.TryEvaluate("AddDays(Today(), 30)", DateTime.MinValue);

// 日期组件
var year = engine.TryEvaluate("Year(Now())", 0);
var month = engine.TryEvaluate("Month(Now())", 0);
var day = engine.TryEvaluate("Day(Now())", 0);

// 日期差
var daysBetween = engine.TryEvaluate("Days('2024-12-31', Today())", 0);
```

### 字符串操作

```csharp
var engine = new AlgorithmEngine();

// 连接
var fullName = engine.TryEvaluate("Concatenate('张', ' ', '三')", "");

// 字符串处理
var upper = engine.TryEvaluate("Upper('hello')", "");
var trimmed = engine.TryEvaluate("Trim('  hello  ')", "");
var substring = engine.TryEvaluate("Mid('Hello World', 1, 5)", "");

// 搜索
var index = engine.TryEvaluate("Find('World', 'Hello World')", 0);
var contains = engine.TryEvaluate("Has('Hello World', 'World')", false);
```

### 条件逻辑

```csharp
var engine = new AlgorithmEngineEx();
engine.AddParameter("Score", 85);

var grade = engine.TryEvaluate(
    "If(Score >= 90, 'A', If(Score >= 80, 'B', If(Score >= 70, 'C', 'D')))", 
    "");

// 使用 IFS
var grade2 = engine.TryEvaluate(
    "IFS(Score >= 90, 'A', Score >= 80, 'B', Score >= 70, 'C', true, 'D')", 
    "");

// 使用 SWITCH
var category = engine.TryEvaluate(
    "Switch(Score, 100, '满分', 90, '优秀', '其他')", 
    "");
```

### 金融计算

```csharp
var engine = new AlgorithmEngine();

// 月供计算
var monthlyPayment = engine.TryEvaluate(
    "PMT(0.05/12, 360, -200000)", 0.0);

// 投资未来值
var futureValue = engine.TryEvaluate(
    "FV(0.07/12, 120, -1000, 0, 0)", 0.0);

// 净现值
var npv = engine.TryEvaluate(
    "NPV(0.1, -1000, 300, 300, 300, 300)", 0.0);
```

### 统计函数

```csharp
var engine = new AlgorithmEngine();

// 基本统计
var sum = engine.TryEvaluate("Sum(1, 2, 3, 4, 5)", 0.0);
var avg = engine.TryEvaluate("Average(1, 2, 3, 4, 5)", 0.0);
var max = engine.TryEvaluate("Max(1, 2, 3, 4, 5)", 0.0);
var min = engine.TryEvaluate("Min(1, 2, 3, 4, 5)", 0.0);

// 标准差
var stdev = engine.TryEvaluate("StDev(1, 2, 3, 4, 5)", 0.0);
```

### JSON 操作

```csharp
var engine = new AlgorithmEngineEx();
engine.AddParameter("JsonData", @"{name: '张三', age: 30, city: '北京'}");

// 解析并访问 JSON
var name = engine.TryEvaluate("GetJsonValue(JsonData, 'name')", "");
var age = engine.TryEvaluate("GetJsonValue(JsonData, 'age')", 0);
```

### 单位转换

```csharp
// 使用单位后缀时自动进行单位转换
var engine = new AlgorithmEngine();
engine.DistanceUnit = DistanceUnitType.M;

// 将 100 厘米转换为米
var meters = AlgorithmEngineHelper.UnitConversion(100, "CM", "M");
```

### 条件树解析

```csharp
// 解析复杂条件
var condition = "Age >= 18 && (Status == 'Active' || Status == 'Pending')";
var tree = AlgorithmEngineHelper.ParseCondition(condition);

if (tree.Type == ConditionTreeType.Error)
{
    Console.WriteLine($"错误: {tree.ErrorMessage}");
}
else
{
    // 处理条件树
    Console.WriteLine($"条件类型: {tree.Type}");
}
```

### 计算树解析

```csharp
// 解析计算表达式
var expression = "Price * Quantity - Discount";
var tree = AlgorithmEngineHelper.ParseCalculate(expression);

if (tree.Type == CalculateTreeType.Error)
{
    Console.WriteLine($"错误: {tree.ErrorMessage}");
}
else
{
    // 处理计算树
    Console.WriteLine($"表达式类型: {tree.Type}");
}
```

### 公式验证

```csharp
var formula = "Sum(A, B, C) * 0.1";
var isValid = AlgorithmEngineHelper.CheckFormula(formula);

if (isValid)
{
    var diyNames = AlgorithmEngineHelper.GetDiyNames(formula);
    Console.WriteLine("参数:");
    foreach (var p in diyNames.Parameters)
    {
        Console.WriteLine($"  - {p.Name}");
    }
}
```

***

## 性能优化建议

1. **解析缓存**: 对于重复计算相同表达式，启用 `UseParseCache = true`。
2. **预编译表达式**: 使用 `Parse()` 一次，`Evaluate()` 多次以获得更好性能。
3. **整数缓存**: -1000 到 1000 的整数会被缓存以减少内存分配。
4. **AlgorithmEngineEx**: 使用 `UseTempDict = true` 缓存参数查找。
5. **避免重复解析**: 对于高频场景，预解析所有表达式。

***

## 错误处理

1. **TryEvaluate**: 出错时返回默认值，设置 `LastError` 属性。
2. **Parse/Evaluate**: 出错时抛出异常。
3. **Operand.IsError**: 检查结果是否为错误。
4. **Operand.ErrorMsg**: 获取错误信息。
5. **LastError**: 出错后检查引擎的 `LastError` 属性。

***

## 线程安全

- `AlgorithmEngine` 和 `AlgorithmEngineEx` 实例不是线程安全的。
- `Parse()` 返回的 `FunctionBase` 对象在执行时是线程安全的。
- 每个线程使用单独的引擎实例或同步访问。

***

## 集成说明

### NuGet 包

```xml
<PackageReference Include="ToolGood.Algorithm" Version="6.2.0" />
```

### 命名空间引用

```csharp
using ToolGood.Algorithm;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;
```

### .NET 版本支持

- .NET Standard 2.1+
- .NET 8.0+

***

## 最佳实践

1. **重用引擎实例**: 创建一次，多次使用。
2. **预解析常用公式**: 存储解析后的函数以供重用。
3. **使用强类型**: 使用适当的 `TryEvaluate` 重载获取预期类型。
4. **优雅处理错误**: 检查 `IsError` 或使用 `TryEvaluate`。
5. **验证输入**: 在存储用户表达式前使用 `CheckFormula()` 验证。
6. **文档化自定义函数**: 清晰记录任何自定义函数实现。
7. **测试边界情况**: 测试空值、空字符串和边界条件。

***

## 相关文件

- [AlgorithmEngine.cs](file:///d:/git/ToolGood.Algorithm2/csharp/ToolGood.Algorithm/AlgorithmEngine.cs) - 主引擎类
- [AlgorithmEngineEx.cs](file:///d:/git/ToolGood.Algorithm2/csharp/ToolGood.Algorithm/AlgorithmEngineEx.cs) - 带缓存的扩展引擎
- [AlgorithmEngineHelper.cs](file:///d:/git/ToolGood.Algorithm2/csharp/ToolGood.Algorithm/AlgorithmEngineHelper.cs) - 辅助方法
- [Operand.cs](file:///d:/git/ToolGood.Algorithm2/csharp/ToolGood.Algorithm/Operand.cs) - 操作数类型
- [FunctionBase.cs](file:///d:/git/ToolGood.Algorithm2/csharp/ToolGood.Algorithm/Internals/Functions/FunctionBase.cs) - 函数基类
- [mathjs.g4](file:///d:/git/ToolGood.Algorithm2/g4/mathjs.g4) - ANTLR4 语法定义
