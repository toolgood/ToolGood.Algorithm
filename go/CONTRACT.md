# ToolGood.Algorithm Go 移植约定（内部契约）

本目录按 `../csharp/ToolGood.Algorithm` 的文件夹结构组织 Go 包，并参照 `../java`、
`../python`（Python 移植）中的语义。**C# 是主参照**。

## 目录 / 包映射

| C# 目录 | Go 包 |
| --- | --- |
| `Enums/` | `go/algorithm/enums` |
| （System.Decimal / BigDecimal） | `go/algorithm/decimal` |
| `LitJson/` | `go/algorithm/litjson` |
| `Operands/` + `Operand.cs` | `go/algorithm/operands` |
| `Internals/` + `AlgorithmEngine.cs` | `go/algorithm/internals` |
| `Internals/Functions/<组>/` | `go/algorithm/internals/functions/<组>` |
| `MathNet/` | `go/algorithm/mathnet`（`distributions/`、`specialfunctions/`、`statistics/`、`rootfinding/`） |
| `UnitConversion/` | `go/algorithm/unitconversion` |
| `CalculationLogic/` | `go/algorithm/calculationlogic` |
| `math/`（ANTLR 生成） + 解析入口 | `go/algorithm`（根包：词法、语法、公开 API） |

依赖方向（禁止反向 import，避免循环）：

```
enums, decimal  ->  litjson  ->  operands  ->  internals  ->  internals/functions/*  ->  algorithm(根包)
mathnet, unitconversion 只依赖 enums/decimal/operands
```

## 函数子包的写法

每个子包都有 `alias.go`，把 `internals` / `operands` / `decimal` / `enums` 的公共名字引入本包，
所以代码里可以直接写 `Operand`、`Decimal`、`Function`、`BaseFunction`、`CreateDecimal`、`Error` 等。

```go
package mathbase

type FunctionABS struct{ BaseFunction }

func newFunctionABS(args []Function) Function {
	return &FunctionABS{BaseFunction: NewBase("ABS", OperandTypeNumber, args)}
}

func (f *FunctionABS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	return CreateFloat(math.Abs(a.DoubleValue()))
}

func init() { RegisterFunction("ABS", newFunctionABS) }
```

- 构造函数统一：`func newFunctionXXX(args []Function) Function`。
- 参数个数校验：`RequireArgs(args, "ABS", 1, 1)`（`max = -1` 表示不限）。
- 注册：`init()` 中 `RegisterFunction("KEY", newFunctionXXX)`，KEY 与 C# 注册名一致（大写）。
  不要修改其它文件的 `init()`。
- 需要自定义文本输出时实现 `WriteTo(sb *strings.Builder, addBrackets bool)`；
  需要自定义结果类型时实现 `ResultType() OperandType`；
  需要传播参数类型时实现
  `GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string)`。

## BaseFunction 辅助方法（index 从 1 开始）

| 方法 | 说明 |
| --- | --- |
| `f.Arg(i)` | 第 i 个参数节点（0 基） |
| `f.ArgCount()` | 参数个数 |
| `f.EvalArg(i, engine, tp)` | 求值第 i 个参数（0 基） |
| `f.GetNumber(index, engine, tp)` | 求值并转数字 |
| `f.GetText(index, engine, tp)` | 求值并转文本 |
| `f.GetDate(index, engine, tp)` | 求值并转日期 |
| `f.GetBoolean(index, engine, tp)` | 求值并转布尔 |
| `f.GetArray(index, engine, tp)` | 求值并转数组 |
| `f.ConvertToNumber(op, index)` 等 | 转换（带参数错误信息） |
| `f.ParameterError(index)` | `Function 'X' parameter N is error!` |
| `f.FunctionError()` / `f.CompareError()` / `f.Div0Error()` / `f.NumError()` | 固定错误 |
| `f.EvaluateAll(engine, tp, &args)` | 依次求值全部参数，返回首个错误（无错误返回 nil） |

## Operand（`operands` 包）

- 判定：`IsErrorOrNone() IsNone() IsNull() IsNumber() IsText() IsBoolean() IsArray() IsDate() IsJson() IsArrayJson() IsError()`
- 取值：`NumberValue() Decimal`、`DoubleValue() float64`、`IntValue() int`、`LongValue() int64`、
  `TextValue() string`、`BooleanValue() bool`、`ArrayValue() []Operand`、`DateValue() *MyDate`、
  `JsonValue() *JsonData`、`ErrorMsg() string`、`Type() OperandType`
- 转换：`ToNumber(msg string)`、`ToBoolean`、`ToText`、`ToMyDate`、`ToArray`、`ToJson`（`msg` 传 `""` 用默认信息）
- 工厂（alias 已引入）：`Create`、`CreateInt`、`CreateInt64`、`CreateFloat`、`CreateDecimal`、
  `CreateString`、`CreateBool`、`CreateMyDate`、`CreateArray`、`CreateJsonData`、`CreateJson`、
  `Error(msg)`、`Errorf(msg, args...)`
- 常量：`True`、`False`、`Null`、`None`、`Zero`、`One`、`Version`

## Decimal（`decimal` 包，alias 已引入）

值类型。方法：`Add Sub Mul Div Div32 Mod FloorMod FloorDiv Neg Abs Cmp Equal Less LessOrEqual
Greater GreaterOrEqual Sign IsZero IsInteger Trunc Floor Ceil RoundHalfUp(digits) RoundHalfEven(digits)
Pow(n) Sqrt Float64 Int64 Int String PlainStrip StringFixed(digits)`。

`Div` 按 34 位有效数字（DECIMAL128）舍入；`Mod` 符号随被除数（BigDecimal.remainder）；
`FloorMod` 符号随除数（Excel MOD / Python `%`）。

## MyDate（`operands` 包，alias 已引入）

字段 `Year Month Day Hour Minute Second int`，`Year/Month/Day` 为 0 表示未设置。
`ParseMyDate NewMyDate NewMyDateFromTime NewMyDateFromDate NewMyDateFromNumber MyDateNow
ToDateTime() time.Time DayOfWeek() int DayOfYear() int AddYears AddMonths AddDays AddHours
AddMinutes AddSeconds AddDate SubDate AddNumber SubNumber MulNumber DivNumber ToNumber() Decimal
ToLong() int64 String() ToString(format string) FormatDateTime(t, pattern)`。

## JsonData（`litjson` 包，alias 已引入）

`JsonMapperToObject(text) (*JsonData, error)`；方法 `IsObject IsArray IsString IsDouble IsBoolean
IsNull InstArray() []*JsonData InstObject() map[string]*JsonData Keys() []string BooleanValue
NumberValue StringValue Count GetChild(key interface{}) ContainsKey String() Set Add`。

## FunctionUtil（`internals` 包，alias 已引入）

`EstimateCount`、`FlattenToList`、`FlattenToBigDecimalList`、`FlattenToNumberList`、
`FlattenToStringList`、`GetCountIf`、`GetCountIfOp`、`GetSumIf`、`GetSumIfOp`、`CompareValues`、
`GetGCDTwo`、`GetGCD`、`GetLCM`、`GetFactorial`、`ParseSumIfMatch`、`TryParseBoolean`、
`QuickSelect`、`GetRank`。

## 移植原则

1. 以 `../csharp/ToolGood.Algorithm/Internals/Functions/<组>/Function_XXX.cs` 为主线逐个实现，
   `../python/toolgood_algorithm/internals/functions/<组>.py` 可作文本对照（它已把 C# 逻辑压缩成单文件）。
2. 保持语义、错误文案、舍入方式一致；不要"顺手优化"分支或提示信息。
3. Python 的 `None` 参数在 Go 中为 `""`（字符串）/ `0`（数字）/ `nil`（指针）。
4. Python 的异常在 Go 中用 `panic` 表示（引擎在公开入口 recover）；普通失败返回 `OperandError`。
5. 完成后 `gofmt -w`，并用下列命令确认本包可编译：

```bash
export GOROOT="C:\\Users\\ASUS\\.workbuddy\\binaries\\go\\go"
"/c/Users/ASUS/.workbuddy/binaries/go/go/bin/go.exe" build ./algorithm/internals/functions/<组>
```
