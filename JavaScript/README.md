ToolGood.Algorithm
===================

ToolGood.Algorithm是一个功能强大、轻量级、兼容`Excel公式`的算法类库，旨在提高开发人员在不同业务场景中的生产力。 


## 快速上手
``` js
    let engine = new ToolGood.Algorithm.AlgorithmEngine();
    let b = engine.TryEvaluate_Boolean("1=1 && 1<2 and 7-8>1", 0);// Support(支持) && || and or 
    let c = engine.TryEvaluate_Int("2+3", 0);
    let d = engine.TryEvaluate_String("'aa'&'bb'", ""); //String connection, return: AABB 字符串连接, 返回:aabb
 
    let engineEx = new ToolGood.Algorithm.AlgorithmEngineEx();

 
```

支持常量`pi`, `e`, `true`, `false`。

数值转bool，非零为真, 零为假。字符串转bool, `0`、`FALSE`、`NO`、`无`、`没有`、`不是`、`否`为假，`1`、`TRUE`、`YES`、`是`、`有`为真。不区分大小写。

bool转数值，假为`0`，真为`1`。bool转字符串，假为`FALSE`，真为`TRUE`。

索引默认为`Excel索引`，如果想用c#索引，请设置`UseExcelIndex`为`false`。

中文符号自动转成英文符号：`括号`, `逗号`, `引号`, `双引号`,`加`，`减`，`乘`,`除`,`等号`。

注：字符串拼接使用`&`。

注：`find`为Excel公式，find(要查找的字符串, 被查找的字符串[, 开始位置])
 
 
参数定义`参数名`，

注：`AlgorithmEngineEx` 可以使用`AddParameter`、`AddParameterFromJson`添加参数。

注：可以重写`GetParameter`、`ExecuteDiyFunction`方法，来自定义参数和自定义方法。

注：使用 `AlgorithmEngineHelper.GetDiyNames` 获取`参数名`、`自定义方法名`。
 

## 自定义参数
``` csharp
    ToolGood.Algorithm.AlgorithmEngineHelper.IsKeywords("false"); // return true
    ToolGood.Algorithm.AlgorithmEngineHelper.IsKeywords("true"); // return true
    ToolGood.Algorithm.AlgorithmEngineHelper.IsKeywords("mysql"); // return false

    let p5 = ToolGood.Algorithm.AlgorithmEngineHelper.GetDiyNames("ddd(d1, 22)");
```
## 支持单位

可设置标准单位：长度`DistanceUnit`(默认：`m`)、面积`AreaUnit`(默认：`m2`)、体积`VolumeUnit`(默认：`m3`)、重量`MassUnit`(默认：`kg`)。

注：公式计算时，先将带单位的数量转成标准单位，再进行数字计算。
 
``` csharp
    let engine = new ToolGood.Algorithm.AlgorithmEngine();
    bool a = engine.TryEvaluate_Boolean("1=1m", false); // return true
    bool b = engine.TryEvaluate_Boolean("1=1m2", false); // return true
    bool c = engine.TryEvaluate_Boolean("1=1m3", false); // return true
    bool d = engine.TryEvaluate_Boolean("1=1kg", false); // return true

    // 单位转化
    var num = ToolGood.Algorithm.AlgorithmEngineHelper.UnitConversion(1M,"米","千米"); 

    // 不抛错例子
    bool error = engine.TryEvaluate_Boolean("1m=1m2", false); // return true
```


## Excel公式

请参考首页公式
