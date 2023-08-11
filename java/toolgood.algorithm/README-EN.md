ToolGood.Algorithm (JAVA)
===================
[中文文档](README.md)

ToolGood.Algorithm is a powerful, lightweight, `Excel formula` compatible algorithm library aimed at improving developers’ productivity in different business scenes. 

**Applicable scenarios:** Code and algorithm are separated to avoid forced project upgrade 

1）Uncertain algorithm at the beginning of the project; 

2）Algorithms that are frequently changed during project maintenance; 

3）Algorithms in financial data and statistical data (Note: Some formulas use the `double` type, and it is recommended to use `fen` as the unit);

4）The report is exported, the data source uses the stored procedure, and the algorithm is set in the Word document. Example https://github.com/toolgood/ToolGood.WordTemplate 


### pom.xml
``` 
    <dependency>
        <groupId>io.github.toolgood</groupId>
        <artifactId>toolgood-algorithm</artifactId>
        <version>3.0.3.0</version>
    </dependency>
```

## Quick start
``` java
    AlgorithmEngine engine = new AlgorithmEngine();
    double a=0.0;
    if (engine.Parse("1+2")) {
        var o = engine.Evaluate();
        a=o.NumberValue;
    }
    double b = engine.TryEvaluate("1=1 && 1<2 and 7-8>1", 0.0);// 支持 && || and or 
    double c = engine.TryEvaluate("2+3", 0);
    double q = engine.TryEvaluate("-7 < -2 ?1 : 2", 0);
    double d = engine.TryEvaluate("count(array(1, 2, 3, 4))", 0.0);//{}代表数组, 返回:4
    String s = engine.TryEvaluate("'aa'&'bb'", ""); //字符串连接, 返回:aabb
    int r = engine.TryEvaluate("(1=1)*9+2", 0); //返回:11
    DateTime d = engine.TryEvaluate("'2016-1-1'+1", DateTime.now()); //返回日期:2016-1-2
    DateTime t = engine.TryEvaluate("'2016-1-1'+9*'1:0'", DateTime.now());//返回日期:2016-1-1 9:0
    String j = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\", \"Age\":51, \"Birthday\":\"04/26/1564 00:00:00\"}').Age", "");//返回51
    String k = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare   \", \"Age\":51, \"Birthday\":\"04/26/1564 00:00:00\"}')[Name].Trim()", "");//返回"William Shakespeare" (不带空格)
    String l = engine.TryEvaluate("json('{\"Name1\":\"William Shakespeare \", \"Age\":51, \"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2, 3)", ""); ;//返回"ill"

```
Constants`pi`, `e`, `true`, `false`are supported.

The value is converted to bool, non-zero is true and zero is false.
String to bool, ` 0`and`FALSE` is false, `1`and`TRUE` is true. Case insensitive.

Bool to value, false is`0`, true is`1`.
Bool to string, false to`FALSE`, true to`TRUE`.

The default index is`excel index`. If you want to use c# index, please set`UseExcelIndex`to`false`.


Chinese symbols are automatically converted to English symbols: `brackets`, `square brackets`, `commas`, `quotation marks`, `double quotation marks`.

Note: Use `&` for string concatenation. 

Note: `find` is an Excel formula , find (the string to be searched, the string to be searched [, start position]) 

## Custom parameters
``` java
    //定义圆柱信息
public class Cylinder extends AlgorithmEngine {
    private int _radius;
    private int _height;

    public Cylinder(int radius, int height) {
        _radius = radius;
        _height = height;
    }

    @Override
    protected Operand GetParameter(String parameter) throws Exception {
        if (parameter.equals("半径")) {
            return Operand.Create(_radius);
        }
        if (parameter.equals("直径"))
        {
            return Operand.Create(_radius * 2);
        }
        if (parameter.equals("高"))
        {
            return Operand.Create(_height);
        }
        return super.GetParameter(parameter);
    }
}
    //调用方法
    Cylinder c = new Cylinder(3, 10);
    c.TryEvaluate("[半径]*[半径]*pi()", 0.0);      //圆底面积
    c.TryEvaluate("[直径]*pi()", 0.0);            //圆的长
    c.TryEvaluate("[半径]*[半径]*pi()*[高]", 0.0); //圆的体积
    c.TryEvaluate("['半径']*[半径]*pi()*[高]", 0.0); //圆的体积
    c.EvaluateFormula("'圆'-[半径]-高", '-'); // Return: 圆-3-10
    c.GetSimplifiedFormula("半径*if(半径>2, 1+4, 3)"); // Return: 3 * 5
```
Parameters are defined in square brackets, such as `[parameter name]`. 

Note: You can also use `AddParameter`, `AddParameterFromJson` to add methods, and use `DiyFunction` to customize functions. 

Note 2: use `AlgorithmEngineHelper.GetDiyNames` get `parameter name` and `custom function name`.

## Multi formula
``` java
    ConditionCache multiConditionCache = new ConditionCache();
    multiConditionCache.LazyLoad = true;
    multiConditionCache.AddFormula("桌面积", "[圆桌]", "[半径]*[半径]*pi()");
    multiConditionCache.AddFormula("桌面积", "[方桌]", "[长]*[宽]");
    multiConditionCache.AddFormula("价格", "[圆桌]&& [半径]<2.5", "[桌面积]*1.3");
    multiConditionCache.AddFormula("价格", "[圆桌]&& [半径]<5", "[桌面积]*1.5");
    multiConditionCache.AddFormula("价格", "[圆桌]&& [半径]<7", "[桌面积]*2");
    multiConditionCache.AddFormula("价格", "[圆桌]", "[桌面积]*2.5");
    multiConditionCache.AddFormula("价格", "[方桌]&& [长]<1.3", "[桌面积]*1.3+[高]*1.1");
    multiConditionCache.AddFormula("价格", "[方桌]&& [长]<2", "[桌面积]*1.5+[高]*1.1");
    multiConditionCache.AddFormula("价格", "[方桌]&& [长]<5", "[桌面积]*2+[高]*1.1");
    multiConditionCache.AddFormula("价格", "[方桌]&& [长]<7", "[桌面积]*2.5");

    toolgood.algorithm.AlgorithmEngineEx algoEngine = new toolgood.algorithm.AlgorithmEngineEx(multiConditionCache);
    algoEngine.JumpConditionError = true;
    algoEngine.AddParameter("方桌", true);
    algoEngine.AddParameter("长", 3);
    algoEngine.AddParameter("宽", 1.3);
    algoEngine.AddParameter("高", 1);

    Double p2 = algoEngine.TryEvaluate("价格", 0.0);
    assertEquals(3 * 1.3 * 2 + 1 * 1.1, p2, 0.0001);
```
See unit testing for more features.

## Custom parameters
``` csharp
    AlgorithmEngineHelper helper = new AlgorithmEngineHelper();
    helper.IsKeywords("false"); // return true
    helper.IsKeywords("true"); // return true
    helper.IsKeywords("mysql"); // return false

    DiyNameInfo p5 = helper.GetDiyNames("ddd(d1, 22)");
    assertEquals("ddd", p5.Functions.get(0));
    assertEquals("d1", p5.Parameters.get(0));

```

## Excel Formula

Functions: `logical functions`, `mathematics and trigonometric functions`, `text functions`, `statistical functions`, `date and time functions` 

Note: Function names are not case sensitive. Parameters with square brackets can be omitted. The return value of the example is approximate. 

Note 2: The function name with ★ indicates that the first parameter can be prefixed, such as `(-1).ISTEXT()` 

Note 3: The function name with ▲ means that it is affected by `Excel Index`, 

#### Logical function
<table>
    <tr><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>IF</td><td>if(测试条件, 真值[, 假值])<br>执行真假值判断, 根据逻辑计算的真假值, 返回不同结果。</td>
        <td>if(1=1, 1, 2) <br>>>1</td>
    </tr>
    <tr>
        <td>ifError</td><td>ifError(测试条件, 真值[, 假值])<br>如果公式计算出错误则返回您指定的值；否则返回公式结果。</td>
        <td>ifError(1/0, 1, 2) <br>>>1</td>
    </tr>
    <tr>
        <td>isError ★</td>
        <td>
        isError(值)<br>判断是否出错, 返回 TRUE 或 FALSE
        isError(值, 替换值)<br>如果出错, 返回替换值，否则返回原值
        </td>
        <td>isError(1) <br>>>false</td>
    </tr>
    <tr>
        <td>isNull ★</td>
        <td>
        isNull(值)<br>判断是否为空, 返回 TRUE 或 FALSE
        isNull(值, 替换值)<br>如果为空, 返回替换值，否则返回原值
        </td>
        <td>isNull(null) <br>>>true</td>
    </tr>
    <tr>
        <td>isNullOrError ★</td>
        <td>
        isNullOrError(值)<br>判断是否为空或错误, 返回 TRUE 或 FALSE
        isNullOrError(值, 替换值)<br>如果为空或错误, 返回替换值，否则返回原值
        </td>
        <td>isNullOrError(null) <br>>>true</td>
    </tr>
    <tr>
        <td>isNumber ★</td><td>isNumber(值)<br>判断是否数值, 返回 TRUE 或 FALSE</td>
        <td>isNumber(1) <br>>>true</td>
    </tr>
    <tr>
        <td>isText ★</td><td>isText(值)<br>判断是否文字, 返回 TRUE 或 FALSE</td>
        <td>isText('1') <br>>>true </td>
    </tr>
    <tr>
        <td>IsNonText ★</td><td>IsNonText(值)<br>判断是否为非文字, 返回 TRUE 或 FALSE</td>
        <td>IsNonText('1') <br>>>false </td>
    </tr>
    <tr>
        <td>IsLogical ★</td><td>IsLogical(值)<br>判断是否为逻辑值, 返回 TRUE 或 FALSE</td>
        <td>IsLogical('1') <br>>>false </td>
    </tr>
    <tr>
        <td>IsEven ★</td><td>IsEven(值)<br>如果数值是偶数, 返回 TRUE 或 FALSE</td>
        <td>IsEven('1') <br>>>false </td>
    </tr>
    <tr>
        <td>IsOdd ★</td><td>IsOdd(值)<br>如果数值是奇数, 返回 TRUE 或 FALSE</td>
        <td>IsOdd('1') <br>>>true </td>
    </tr>
    <tr>
        <td>AND</td><td>and(逻辑值1, ...)<br>如果所有参数均为TRUE, 则返回TRUE, 如有错误先报错</td>
        <td>and(1, 2=2) <br>>>true</td>
    </tr>
    <tr>
        <td>OR</td><td>or(逻辑值1, ...)<br>如果任一参数为TRUE, 则返回TRUE, 如有错误先报错</td>
        <td>or(1, 2=3) <br>>>true</td>
    </tr>
    <tr>
        <td>NOT</td><td>not(逻辑值)<br>对参数的逻辑值求反</td>
        <td>NOT(true()) <br>>>false</td>
    </tr>
    <tr>
        <td>TRUE</td><td>true()<br>返回逻辑值TRUE</td>
        <td>true() <br>>>true</td>
    </tr>
    <tr>
        <td>FALSE</td><td>false()<br>返回逻辑值FALSE</td>
        <td>false() <br>>>false</td>
    </tr>
</table>

#### Mathematics and Trigonometric Functions
<table>
    <tr><td>分类</td><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td rowspan="14">基<br><br>础<br><br>数<br><br>学</td>
        <td>E</td><td>e()<br>返回 e 值</td>
        <td>E() <br>>> </td>
    </tr>
    <tr>
        <td>PI</td><td>pi()<br>返回 PI 值</td>
        <td>pi() <br>>>3.141592654</td>
    </tr>
    <tr>
        <td>abs</td><td>abs(数值)<br>返回数值的绝对值</td>
        <td>abs(-1) <br>>>1</td>
    </tr>
    <tr>
        <td>QUOTIENT</td><td>quotient(除数, 被除数)<br>返回商的整数部分, 该函数可用于舍掉商的小数部分。</td>
        <td>QUOTIENT(7, 3) <br>>>2</td>
    </tr>
    <tr>
        <td>mod</td><td>mod(除数, 被除数)<br>返回两数相除的余数</td>
        <td>MOD(7, 3) <br>>>1</td>
    </tr>
    <tr>
        <td>SIGN</td><td>sign(数值)<br>返回数值的符号。当数值为正数时返回 1, 为零时返回 0, 为负数时返回 -1。</td>
        <td>SIGN(-9) <br>>>-1</td>
    </tr>
   <tr>
        <td>SQRT</td><td>sqrt(数值)<br>返回正平方根</td>
        <td>SQRT(9) <br>>>3</td>
    </tr>
    <tr>
        <td>TRUNC</td><td>trunc(数值)<br>将数值截尾取整</td>
        <td>TRUNC(9.222) <br>>>9</td>
    </tr>
    <tr>
        <td>int ★</td><td>int(数值)<br>将数值向下舍入到最接近的整数。</td>
        <td>int(9.222) <br>>>9</td>
    </tr>
    <tr>
        <td>gcd</td><td>gcd(数值, ...)<br>返回最大公约数</td>
        <td>GCD(3, 5, 7) <br>>>1</td>
    </tr>
    <tr>
        <td>LCM</td><td>lcm(数值, ...)<br>返回整数参数的最小公倍数</td>
        <td>LCM(3, 5, 7) <br>>>105</td>
    </tr>
    <tr>
        <td>combin</td><td>combin(总数, 排列数)<br>计算从给定数目的对象集合中提取若干对象的组合数</td>
        <td>combin(10, 2) <br>>>45</td>
    </tr>
    <tr>
        <td>PERMUT</td><td>permut(总数, 排列数)<br>返回从给定数目的对象集合中选取的若干对象的排列数</td>
        <td>PERMUT(10, 2) <br>>>990</td>
    </tr>
    <tr>
        <td>FIXED</td><td>fixed(数值[, 小数位数[, 有无逗号分隔符]])<br>将数值设置为具有固定小数位的文本格式</td>
        <td>FIXED(4567.89, 1) <br>>>4, 567.9</td>
    </tr>
    <tr>
    <td rowspan="15">三<br><br>角<br><br>函<br><br>数</td>
        <td>degrees</td><td>degrees(弧度)<br>将弧度转换为度</td>
        <td>degrees(pi()) <br>>>180</td>
    </tr>
    <tr>
        <td>RADIANS</td><td>radians(度)<br>将度转换为弧度</td>
        <td>RADIANS(180) <br>>>3.141592654</td>
    </tr>
    <tr>
        <td>cos</td><td>cos(弧度)<br>返回数值的余弦值</td>
        <td>cos(1) <br>>>0.540302305868</td>
    </tr>
    <tr>
        <td>cosh</td><td>cosh(弧度)<br>返回数值的双曲余弦值</td>
        <td>cosh(1) <br>>>1.54308063481</td>
    </tr>
    <tr>
        <td>SIN</td><td>sin(弧度)<br>返回给定角度的正弦值</td>
        <td>sin(1) <br>>>0.84147098480</td>
    </tr>
    <tr>
        <td>SINH</td><td>sinh(弧度)<br>返回数值的双曲正弦值</td>
        <td>sinh(1) <br>>>1.1752011936</td>
    </tr>
    <tr>
        <td>TAN</td><td>tan(弧度)<br>返回数值的正切值</td>
        <td>tan(1) <br>>>1.55740772465</td>
    </tr>
    <tr>
        <td>TANH</td><td>tanh(弧度)<br>返回数值的双曲正切值</td>
        <td>tanh(1) <br>>>0.761594155955</td>
    </tr>
    <tr>
        <td>acos</td><td>acos(数值)<br>返回数值的反余弦值</td>
        <td>acos(0.5) <br>>>1.04719755119</td>
    </tr>
    <tr>
        <td>acosh</td><td>acosh(数值)<br>返回数值的反双曲余弦值</td>
        <td>acosh(1.5) <br>>>0.962423650119</td>
    </tr>
    <tr>
        <td>asin</td><td>asin(数值)<br>返回数值的反正弦值</td>
        <td>asin(0.5) <br>>>0.523598775598</td>
    </tr>
    <tr>
        <td>asinh</td><td>asinh(数值)<br>返回数值的反双曲正弦值。</td>
        <td>asinh(1.5) <br>>>1.1947632172</td>
    </tr>
    <tr>
        <td>atan</td><td>atan(数值)<br>返回数值的反正切值</td>
        <td>atan(1) <br>>>0.785398163397</td>
    </tr>
   <tr>
        <td>atanh</td><td>atanh(数值)<br>返回参数的反双曲正切值</td>
        <td>atanh(1) <br>>>0.549306144334</td>
    </tr>
    <tr>
        <td>atan2</td><td>atan2(数值, 数值)<br>从X和Y坐标返回反正切</td>
        <td>atan2(1, 2) <br>>>1.10714871779</td>
    </tr>
    <tr>
        <td rowspan="8">四<br><br>舍<br><br>五<br><br>入</td>
        <td>ROUND</td><td>round(数值, 小数位数)<br>返回某个数值按指定位数取整后的数值。</td>
        <td>ROUND(4.333, 2) <br>>>4.33</td>
    </tr>
    <tr>
        <td>roundDown</td><td>roundDown(数值, 小数位数)<br>靠近零值, 向下（绝对值减小的方向）舍入数值。</td>
        <td>roundDown(4.333, 2) <br>>>4.33</td>
    </tr>
    <tr>
        <td>roundUp</td><td>roundUp(数值, 小数位数)<br>远离零值, 向上（绝对值增长的方向）舍入数值。</td>
        <td>roundUp(4.333, 2) <br>>>4.34</td>
    </tr>
    <tr>
        <td>CEILING</td><td>ceiling(数值, 舍入基数)<br>向上舍入（沿绝对值增大的方向）为最接近的 舍入基数 的倍数。</td>
        <td>CEILING(4.333, 0.1) <br>>>4.4</td>
    </tr>
    <tr>
        <td>floor</td><td>floor(数值, 舍入基数)<br>向下舍入, 使其等于最接近的 Significance 的倍数。</td>
        <td>FLOOR(4.333, 0.1) <br>>>4.3</td>
    </tr>
    <tr>
        <td>even</td><td>even(数值)<br>返回沿绝对值增大方向取整后最接近的偶数。</td>
        <td>EVEN(3) <br>>>4</td>
    </tr>
    <tr>
        <td>ODD</td><td>odd(数值)<br>将数值向上舍入为最接近的奇型整数</td>
        <td>ODD(3.1) <br>>>5</td>
    </tr>
    <tr>
        <td>MROUND</td><td>mround(数值, 舍入基数)<br>返回一个舍入到所需倍数的数值</td>
        <td>MROUND(13, 5) <br>>>15</td>
    </tr>
    <tr>
        <td rowspan="2">随<br><br>机<br><br>数</td>
        <td>RAND</td><td>rand()<br>返回 0 到 1 之间的随机数 </td>
        <td>RAND() <br>>>0.2</td>
    </tr>
    <tr>
        <td>randBetween</td><td>randBetween(最小整数, 最大整数)<br>返回大于等于指定的最小值, 小于指定最大值之间的一个随机整数。</td>
        <td>randBetween(2, 44) <br>>>9</td>
    </tr>
    <tr>
        <td rowspan="11">幂<br><br>/<br><br>对<br><br>数<br><br>/<br><br>阶<br><br>乘</td>
        <td>fact</td><td>fact(数值)<br>返回数的阶乘, 一个数的阶乘等于 1*2*3*…* 该数。</td>
        <td>FACT(3) <br>>>6</td>
    </tr>
    <tr>
        <td>factdouble</td><td>factDouble(数值)<br>返回数值的双倍阶乘</td>
        <td>factDouble(10) <br>>>3840</td>
    </tr>
    <tr>
        <td>POWER</td><td>power(数值, 幂)<br>返回数的乘幂结果</td>
        <td>POWER(10, 2) <br>>>100</td>
    </tr>
    <tr>
        <td>exp</td><td>exp(幂)<br>返回e的指定数乘幂</td>
        <td>exp(2) <br>>>7.389056099</td>
    </tr>
    <tr>
        <td>ln</td><td>ln(数值)<br>返回数值的自然对数</td>
        <td>LN(4) <br>>>1.386294361</td>
    </tr>
    <tr>
        <td>log</td><td>log(数值[, 底数])<br>返回数值的常用对数, 如省略底数, 默认为10</td>
        <td>LOG(100, 10) <br>>>2</td>
    </tr>
    <tr>
        <td>LOG10</td><td>log10(数值)<br>返回数值的10对数</td>
        <td>LOG10(100) <br>>>2</td>
    </tr>
    <tr>
        <td>MULTINOMIAL</td><td>multinomial(数值, ...)<br>返回参数和的阶乘与各参数阶乘乘积的比值</td>
        <td>MULTINOMIAL(1, 2, 3) <br>>>60</td>
    </tr>
    <tr>
        <td>PRODUCT</td><td>product(数值, ...)<br>将所有以参数形式给出的数值相乘, 并返回乘积值。</td>
        <td>PRODUCT(1, 2, 3, 4) <br>>>24</td>
    </tr>
    <tr>
        <td>SqrtPi</td><td>SqrtPi(数值)<br>返回某数与 PI 的乘积的平方根</td>
        <td>SqrtPi(3) <br>>>3.069980124</td>
    </tr>
    <tr>
        <td>SUMSQ</td><td>sumQq(数值, ...)<br>返回参数的平方和</td>
        <td>SUMSQ(1, 2) <br>>>5</td>
    </tr>
    <tr>
        <td rowspan="12">转<br><br>化</td>
        <td>DEC2BIN ★</td><td>DEC2BIN(数值[, 位数])<br>十进制转二进制 </td>
        <td>DEC2BIN(100) <br>>> </td>
    </tr>
    <tr>
        <td>DEC2OCT ★</td><td>DEC2OCT(数值[, 位数])<br>十进制转八进制 </td>
        <td>DEC2OCT(100) <br>>> </td>
    </tr>
    <tr>
        <td>DEC2HEX ★</td><td>DEC2HEX(数值[, 位数])<br>十进制转十六进制 </td>
        <td>DEC2HEX(100) <br>>> </td>
    </tr>
    <tr>
        <td>HEX2BIN ★</td><td>HEX2BIN(数值[, 位数])<br>十六进制转二进制 </td>
        <td>HEX2BIN(100) <br>>> </td>
    </tr>
    <tr>
        <td>HEX2OCT ★</td><td>HEX2OCT(数值[, 位数])<br>十六进制转八进制 </td>
        <td>HEX2OCT(100) <br>>> </td>
    </tr>
    <tr>
        <td>HEX2DEC ★</td><td>HEX2DEC(数值)<br>十六进制转十进制 </td>
        <td>HEX2DEC(100) <br>>> </td>
    </tr>
    <tr>
        <td>OCT2BIN ★</td><td>OCT2BIN(数值[, 位数])<br>八进制转二进制 </td>
        <td>OCT2BIN(100) <br>>> </td>
    </tr>
    <tr>
        <td>OCT2DEC ★</td><td>OCT2DEC(数值)<br>八进制转十进制 </td>
        <td>OCT2DEC(100) <br>>> </td>
    </tr>
    <tr>
        <td>OCT2HEX ★</td><td>OCT2HEX(数值[, 位数])<br>八进制转十六进制 </td>
        <td>OCT2HEX(100) <br>>> </td>
    </tr>
    <tr>
        <td>BIN2OCT ★</td><td>BIN2OCT(数值[, 位数])<br>二进制转八进制 </td>
        <td>BIN2OCT(100) <br>>> </td>
    </tr>
    <tr>
        <td>BIN2DEC ★</td><td>BIN2DEC(数值)<br>二进制转十进制 </td>
        <td>BIN2DEC(100) <br>>> </td>
    </tr>
    <tr>
        <td>BIN2HEX ★</td><td>BIN2HEX(数值[, 位数])<br>二进制转十六进制 </td>
        <td>BIN2HEX(100) <br>>> </td>
    </tr>
</table>

#### Text function
<table>
    <tr><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>ASC ★</td><td>asc(字符串)<br>将字符串内的全角英文字母更改为半角字符</td>
        <td>asc('ａｂｃＡＢＣ１２３') <br>>>abcABC123</td>
    </tr>
    <tr>
        <td>JIS ★<br> WIDECHAR ★</td><td>jis(字符串)<br>将字符串中的半角英文字符更改为全角字符</td>
        <td>jis('abcABC123') <br>>>ａｂｃＡＢＣ１２３</td>
    </tr>
    <tr>
        <td>CHAR ★</td><td>CHAR(数值)<br>返回由代码数值指定的字符</td>
        <td>char(49) <br>>>1</td>
    </tr>
    <tr>
        <td>CLEAN ★</td><td>clean(字符串)<br>删除文本中所有打印不出的字符</td>
        <td>clean('\r112\t') <br>>>112</td>
    </tr>
    <tr>
        <td>CODE ★</td><td>code(字符串)<br>返回文本字符串中第一个字符的数值代码</td>
        <td>CODE("1") <br>>>49</td>
    </tr>
    <tr>
        <td>CONCATENATE ★</td><td>concatenate(字符串1, ...)<br>将若干文本项合并到一个文本项中</td>
        <td>CONCATENATE('tt', '11') <br>>>tt11</td>
    </tr>
    <tr>
        <td>EXACT ★</td><td>exact(字符串1, 字符串2)<br>检查两个文本值是否完全相同</td>
        <td>EXACT("11", "22") <br>>>false</td>
    </tr>
    <tr style="color:red">
        <td>FIND ★ ▲</td><td>find(要查找的字符串, 被查找的字符串[, 开始位置])<br>在一文本值内查找另一文本值（区分大小写） </td>
        <td>FIND("11", "12221122") <br>>>5</td>
    </tr>
    <tr>
        <td>LEFT ★</td><td>left(字符串[, 字符个数])<br>返回文本值最左边的字符</td>
        <td>LEFT('123222', 3) <br>>>123</td>
    </tr>
    <tr>
        <td>LEN ★</td><td>len(字符串)<br>返回文本字符串中的字符个数</td>
        <td>LEN('123222') <br>>>6</td>
    </tr>
    <tr>
        <td>MID ★  ▲</td><td>mid(字符串, 开始位置, 字符个数)<br>从文本字符串中的指定位置起返回特定个数的字符</td>
        <td>MID('ABCDEF', 2, 3) <br>>>BCD</td>
    </tr>
    <tr>
        <td>PROPER ★</td><td>proper(字符串)<br>将文本值中每一个单词的首字母设置为大写</td>
        <td>PROPER('abc abc') <br>>>Abc Abc</td>
    </tr>
    <tr>
        <td>REPLACE ★  ▲</td>
        <td>replace(原字符串, 开始位置, 字符个数, 新字符串)<br>
        replace(原字符串, 要替换的字符串, 新字符串)<br>
        替换文本内的字符</td>
        <td>REPLACE("abccd", 2, 3, "2") <br>>>a2d<br>
        REPLACE("abccd", "bc", "2") <br>>>a2cd
        </td>
    </tr>
    <tr>
        <td>REPT ★</td><td>rept(字符串, 重复次数)<br>按给定次数重复文本</td>
        <td>REPT("q", 3) <br>>>qqq</td>
    </tr>
    <tr>
        <td>RIGHT ★</td><td>right(字符串[, 字符个数])<br>返回文本值最右边的字符</td>
        <td>RIGHT("123q", 3) <br>>>23q</td>
    </tr>
    <tr>
        <td>RMB ★</td><td>RMB(数值)<br>将数值转换为大写数值文本</td>
        <td>RMB(12.3) <br>>>壹拾贰元叁角</td>
    </tr>
    <tr>
        <td>SEARCH ★ ▲</td><td>search(要找的字符串, 被查找的字符串[, 开始位置])<br>在一文本值中查找另一文本值（不区分大小写）</td>
        <td>SEARCH("aa", "abbAaddd") <br>>>4</td>
    </tr>
    <tr>
        <td>SUBSTITUTE ★</td><td>substitute(字符串, 原字符串, 新字符串[, 替换序号])<br>在文本字符串中以新文本替换旧文本</td>
        <td>SUBSTITUTE("ababcc", "ab", "12") <br>>>1212cc</td>
    </tr>
    <tr>
        <td>T ★</td><td>t(数值)<br>将参数转换为文本</td>
        <td>T('123') <br>>>123</td>
    </tr>
    <tr>
        <td>TEXT ★</td><td>text(数值, 数值格式)<br>设置数值的格式并将数值转换为文本</td>
        <td>TEXT(123, "0.00") <br>>>123.00</td>
    </tr>
    <tr>
        <td>TRIM ★</td><td>trim(字符串)<br>删除文本中的空格</td>
        <td>TRIM(" 123 123 ")<br>>>123 123</td>
    </tr>
    <tr>
        <td>LOWER ★<br>TOLOWER ★</td><td>lower(字符串)<br>tolower(字符串)<br>将文本转换为小写形式</td>
        <td>LOWER('ABC') <br>>>abc</td>
    </tr>
    <tr>
        <td>UPPER ★<br>TOUPPER ★</td><td>upper(字符串)<br>toupper(字符串)<br>将文本转换为大写形式</td>
        <td>UPPER("abc") <br>>>ABC</td>
    </tr>
    <tr>
        <td>VALUE ★</td><td>value(字符串)<br>将文本参数转换为数值</td>
        <td>VALUE("123") <br>>>123</td>
    </tr>
</table>

#### Date and time functions
<table>
    <tr><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>NOW</td><td>now()<br>返回当前日期和时间的序列号</td>
        <td>NOW() <br>>>2017-01-07 11:00:00</td>
    </tr>
    <tr>
        <td>TODAY</td><td>today()<br>返回今天日期的序列号</td>
        <td>TODAY() <br>>>2017-01-07</td>
    </tr>
    <tr>
        <td>DateValue ★</td><td>DateValue(字符串)<br>将文本格式的日期转换为序列号</td>
        <td>DateValue("2017-01-02") <br>>>2017-01-02</td>
    </tr>
    <tr>
        <td>TimeValue ★</td><td>TimeValue(字符串)<br>将文本格式的时间转换为序列号</td>
        <td>TimeValue("12:12:12") <br>>>12:12:12</td>
    </tr>
    <tr>
        <td>DATE ★</td><td>date(年, 月, 日[, 时[, 分[, 秒]]])<br>返回特定日期的序列号</td>
        <td>DATE(2016, 1, 1) <br>>>2016-01-01</td>
    </tr>
    <tr>
        <td>TIME ★</td><td>time(时, 分, 秒)<br>返回特定时间的序列号</td>
        <td>TIME(12, 13, 14) <br>>>12:13:14</td>
    </tr>
    <tr>
        <td>YEAR ★</td><td>year(日期)<br>将序列号转换为年</td>
        <td>YEAR(NOW()) <br>>>2017</td>
    </tr>
    <tr>
        <td>MONTH ★</td><td>month(日期)<br>将序列号转换为月</td>
        <td>MONTH(NOW()) <br>>>1</td>
    </tr>
    <tr>
        <td>DAY ★</td><td>day(日期)<br>将序列号转换为月份中的日</td>
        <td>DAY(NOW()) <br>>>7</td>
    </tr>
    <tr>
        <td>HOUR ★</td><td>hour(日期)<br>将序列号转换为小时</td>
        <td>HOUR(NOW()) <br>>>11</td>
    </tr>
    <tr>
        <td>MINUTE ★</td><td>minute(日期)<br>将序列号转换为分钟</td>
        <td>MINUTE(NOW()) <br>>>12</td>
    </tr>
    <tr>
        <td>SECOND ★</td><td>second(日期)<br>将序列号转换为秒</td>
        <td>SECOND(NOW()) <br>>>34</td>
    </tr>
    <tr>
        <td>WEEKDAY ★</td><td>second(日期)<br>将序列号转换为星期几</td>
        <td>WEEKDAY(date(2017, 1, 7)) <br>>>7</td>
    </tr>
    <tr>
        <td>dateDIF</td><td>dateDif(开始日期, 结束日期, 类型Y/M/D/YD/MD/YM)<br>返回两个日期之间的相隔天数</td>
        <td>dateDIF("1975-1-30", "2017-1-7", "Y") <br>>>41</td>
    </tr>
    <tr>
        <td>DAYS360</td><td>days360(开始日期, 结束日期[, 选项0/1])<br>以一年 360 天为基准计算两个日期间的天数</td>
        <td>DAYS360('1975-1-30', '2017-1-7') <br>>>15097</td>
    </tr>
    <tr>
        <td>EDATE</td><td>eDate(开始日期, 月数)<br>返回用于表示开始日期之前或之后月数的日期的序列号</td>
        <td>EDATE("2012-1-31", 32) <br>>>2014-09-30</td>
    </tr>
    <tr>
        <td>EOMONTH</td><td>eoMonth(开始日期, 月数)<br>返回指定月数之前或之后的月份的最后一天的序列号</td>
        <td>EOMONTH("2012-2-1", 32) <br>>>2014-10-31</td>
    </tr>
    <tr>
        <td>netWorkdays</td><td>netWorkdays(开始日期, 结束日期[, 假日])<br>返回两个日期之间的全部工作日数</td>
        <td>netWorkdays("2012-1-1", "2013-1-1") <br>>>262</td>
    </tr>
    <tr>
        <td>workDay</td><td>workday(开始日期, 天数[, 假日])<br>返回指定的若干个工作日之前或之后的日期的序列号</td>
        <td>workDay("2012-1-2", 145) <br>>>2012-07-23</td>
    </tr>
    <tr>
        <td>WEEKNUM</td><td>weekNum(日期[, 类型：1/2])<br>将序列号转换为一年中相应的周数</td>
        <td>WEEKNUM("2016-1-3") <br>>>2</td>
    </tr>
</table>

##### Extension function
<table>
    <tr><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>AddYears ★</td><td>AddYears(时间, 数值)<br>增加年。</td> <td></td>
    </tr>
	<tr>
        <td>AddMonths ★</td><td>AddMonths(时间, 数值)<br>增加月。</td> <td></td>
    </tr>
	<tr>
        <td>AddDays ★</td><td>AddDays(时间, 数值)<br>增加日。</td> <td></td>
    </tr>
    <tr>
        <td>AddHours ★</td><td>AddHours(时间, 数值)<br>增加小时。</td> <td></td>
    </tr>
    <tr>
        <td>AddMinutes ★</td><td>AddMinutes(时间, 数值)<br>增加分钟。</td> <td></td>
    </tr>
    <tr>
        <td>AddSeconds ★</td><td>AddSeconds(时间, 数值)<br>增加秒。</td> <td></td>
    </tr>
    <tr>
    <td>DateValue ★</td><td>DateValue(值, 数值)<br>转成时间<br>
        DateValue(文本/数值, 0) <br>解析，自动匹配到当前日期相近的日期<br>
        DateValue(文本, 1) <br>解析年月日文本<br>
        DateValue(数值, 2) <br>转成日期，Excel数值<br>
        DateValue(数值, 3) <br>转成日期，时间戳（毫秒）<br>
        DateValue(值数值, 4) <br>转成日期，时间戳（秒）<br>
        </td> <td></td>
    </tr>
    <tr>
        <td>Timestamp ★</td><td>DateValue(值[, 数值=0])<br>转成时间戳，默认为毫秒<br>
        Timestamp(时间, 0) <br>转成时间戳（毫秒）<br>
        Timestamp(时间, 1) <br>转成时间戳（秒）<br>
        </td> <td></td>
    </tr>
</table>

Note: The `UseLocalTime` attribute affects the conversion of `DateValue`/`Timestamp`. Set `true` to directly convert to local time.


#### Statistical function
<table>
    <tr><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>MAX</td><td>max(数值, ...)<br>返回参数列表中的最大值</td>
        <td>max(1, 2, 3, 4, 2, 2, 1, 4) <br>>>4</td>
    </tr>
    <tr>
        <td>MEDIAN</td><td>median(数值, ...)<br>返回给定数值的中值</td>
        <td>MEDIAN(1, 2, 3, 4, 2, 2, 1, 4) <br>>>2</td>
    </tr>
    <tr>
        <td>MIN</td><td>min(数值, ...)<br>返回参数列表中的最小值</td>
        <td>MIN(1, 2, 3, 4, 2, 2, 1, 4) <br>>>1</td>
    </tr>
    <tr>
        <td>QUARTILE</td><td>quartile(数值, 四分位：0-4)<br>返回数据集的四分位数</td>
        <td>QUARTILE({1, 2, 3, 4, 2, 2, 1, 4}, 0) <br>>>1</td>
    </tr>
    <tr>
        <td>MODE</td><td>mode(数值, ...)<br>返回在数组中出现频率最多的数值</td>
        <td>MODE(1, 2, 3, 4, 2, 2, 1, 4) <br>>>2</td>
    </tr>
    <tr>
        <td>LARGE ▲</td><td>large(数组, K)<br>返回数据集中第 k 个最大值</td>
        <td>LARGE({1, 2, 3, 4, 2, 2, 1, 4}, 3) <br>>>3</td>
    </tr>
    <tr>
        <td>SMALL ▲</td><td>small(数值, K)<br>返回数据集中第 k 个最小值</td>
        <td>SMALL({1, 2, 3, 4, 2, 2, 1, 4}, 3) <br>>>2</td>
    </tr>
    <tr>
        <td>PERCENTILE</td><td>percentile(数值, K)<br>返回区域中的第 k 个百分位值</td>
        <td>PERCENTILE({1, 2, 3, 4, 2, 2, 1, 4}, 0.4) <br>>>2</td>
    </tr>
    <tr>
        <td>PERCENTRANK</td><td>percentRank(数值, K)<br>返回数据集中值的百分比排位</td>
        <td>PERCENTRANK({1, 2, 3, 4, 2, 2, 1, 4}, 3) <br>>>0.714</td>
    </tr>
    <tr>
        <td>AVERAGE</td><td>average(数值, ...)<br>返回参数的平均值</td>
        <td>AVERAGE(1, 2, 3, 4, 2, 2, 1, 4) <br>>>2.375</td>
    </tr>
    <tr>
        <td>averageIf</td><td>averageIf({数值, ...}, 条件[, {值1, ...}])<br>返回参数的平均值</td>
        <td>averageIf({1, 2, 3, 4, 2, 2, 1, 4}, '>1') <br>>>2.833333333</td>
    </tr>
    <tr>
        <td>GEOMEAN</td><td>geoMean(数值, ...)<br>返回正数数组或区域的几何平均值</td>
        <td>GEOMEAN(1, 2, 3, 4) <br>>>2.213363839</td>
    </tr>
    <tr>
        <td>HARMEAN</td><td>harMean(数值, ...)<br>返回数据集合的调和平均值</td>
        <td>HARMEAN(1, 2, 3, 4) <br>>>1.92</td>
    </tr>
    <tr>
        <td>COUNT</td><td>count(数值, ...)<br>计算参数列表中数值的个数</td>
        <td>COUNT(1, 2, 3, 4, 2, 2, 1, 4) <br>>>8</td>
    </tr>
    <tr>
        <td>countIf</td><td>countIf({数值, ...}, 条件[, {值1, ...}])<br>计算参数列表中数值的个数</td>
        <td>countIf({1, 2, 3, 4, 2, 2, 1, 4}, '>1') <br>>>6</td>
    </tr>
    <tr>
        <td>SUM</td><td>sum(数值, ...)<br>返回所有数值之和。</td>
        <td>SUM(1, 2, 3, 4) <br>>>10</td>
    </tr>
    <tr>
        <td>sumIf</td><td>sumIf({数值, ...}, 条件[, {值1, ...}])<br>返回所有数值之和</td>
        <td>sumIf({1, 2, 3, 4, 2, 2, 1, 4}, '>1') <br>>>17</td>
    </tr>
    <tr>
        <td>AVEDEV</td><td>aveDev(数值, ...)<br>返回数据点与其平均值的绝对偏差的平均值</td>
        <td>AVEDEV(1, 2, 3, 4, 2, 2, 1, 4) <br>>>0.96875</td>
    </tr>
    <tr>
        <td>STDEV</td><td>stDev(数值, ...)<br>基于样本估算标准偏差</td>
        <td>STDEV(1, 2, 3, 4, 2, 2, 1, 4) <br>>>1.1877349391654208</td>
    </tr>
    <tr>
        <td>STDEVP</td><td>stDevp(数值, ...)<br>计算基于整个样本总体的标准偏差</td>
        <td>STDEVP(1, 2, 3, 4, 2, 2, 1, 4) <br>>>1.1110243021644486</td>
    </tr>
    <tr>
        <td>DEVSQ</td><td>devSq(数值, ...)<br>返回偏差的平方和</td>
        <td>DEVSQ(1, 2, 3, 4, 2, 2, 1, 4) <br>>>9.875</td>
    </tr>
    <tr>
        <td>VAR</td><td>var(数值, ...)<br>基于样本估算方差</td>
        <td>VAR(1, 2, 3, 4, 2, 2, 1, 4) <br>>>1.4107142857142858</td>
    </tr>
    <tr>
        <td>VARP</td><td>varp(数值, ...)<br>基于整个样本总体计算方差</td>
        <td>VARP(1, 2, 3, 4, 2, 2, 1, 4) <br>>>1.234375</td>
    </tr>
    <tr>
        <td>normDist</td><td>normDist(数值, 算术平均值, 标准偏差, 返回类型：0/1)<br>返回正态累积分布</td>
        <td>normDist(3, 8, 4, 1) <br>>>0.105649774</td>
    </tr>
    <tr>
        <td>normInv</td><td>normInv(分布概率, 算术平均值, 标准偏差)<br>返回反正态累积分布</td>
        <td>normInv(0.8, 8, 3) <br>>>10.5248637</td>
    </tr>
    <tr>
        <td>NormSDist</td><td>normSDist(数值)<br>返回标准正态累积分布函数, 该分布的平均值为 0, 标准偏差为 1。</td>
        <td>NORMSDist(1) <br>>>0.841344746</td>
    </tr>
    <tr>
        <td>normSInv</td><td>normSInv(数值)<br>返回反标准正态累积分布</td>
        <td>normSInv(0.3) <br>>>-0.524400513</td>
    </tr>
    <tr>
        <td>BetaDist</td><td>BetaDist(数值, 分布参数α, 分布参数β)<br>返回 Beta 累积分布函数</td>
        <td>BetaDist(0.5, 11, 22) <br>>>0.97494877</td>
    </tr>
    <tr>
        <td>BetaInv</td><td>BetaInv(数值, 分布参数α, 分布参数β)<br>返回指定 Beta 分布的累积分布函数的反函数</td>
        <td>BetaInv(0.5, 23, 45) <br>>>0.336640759</td>
    </tr>
    <tr>
        <td>binomDist</td><td>binomDist(试验成功次数, 试验次数, 成功概率, 返回类型：0/1)<br>返回一元二项式分布概率</td>
        <td>binomDist(12, 45, 0.5, 0) <br>>>0.000817409</td>
    </tr>
    <tr>
        <td>exponDist</td><td>exponDist(函数值, 参数值, 返回类型：0/1)<br>返回指数分布</td>
        <td>exponDist(3, 1, 0) <br>>>0.049787068</td>
    </tr>
    <tr>
        <td>FDist</td><td>fDist(数值X, 分子自由度, 分母自由度)<br>返回 F 概率分布</td>
        <td>FDist(0.4, 2, 3) <br>>>0.701465776</td>
    </tr>
    <tr>
        <td>FInv</td><td>fInv(分布概率, 分子自由度, 分母自由度)<br>返回 F 概率分布的反函数</td>
        <td>FInv(0.7, 2, 3) <br>>>0.402651432</td>
    </tr>
    <tr>
        <td>FISHER</td><td>fisher(数值)<br>返回点 x 的 Fisher 变换。该变换生成一个正态分布而非偏斜的函数</td>
        <td>FISHER(0.68) <br>>>0.8291140383</td>
    </tr>
    <tr>
        <td>fisherInv</td><td>fisherInv(数值)<br>返回 Fisher 变换的反函数值。</td>
        <td>fisherInv(0.6) <br>>>0.537049567</td>
    </tr>
    <tr>
        <td>gammaDist</td><td>gammaDist(数值, 分布参数α, 分布参数β, 返回类型：0/1)<br>返回 γ 分布</td>
        <td>gammaDist(0.5, 3, 4, 0) <br>>>0.001723627</td>
    </tr>
    <tr>
        <td>gammaInv</td><td>gammaInv(分布概率, 分布参数α, 分布参数β)<br>返回 γ 累积分布函数的反函数</td>
        <td>gammaInv(0.2, 3, 4) <br>>>6.140176811</td>
    </tr>
    <tr>
        <td>GAMMALN</td><td>gammaLn(数值)<br>返回 γ 累积分布函数的反函数</td>
        <td>GAMMALN(4) <br>>>1.791759469</td>
    </tr>
    <tr>
        <td>hypgeomDist</td><td>hypgeomDist(样本成功次数, 样本容量, 样本总体成功次数, 样本总体容量)<br>返回超几何分布</td>
        <td>hypgeomDist(23, 45, 45, 100) <br>>>0.08715016</td>
    </tr>
    <tr>
        <td>logInv</td><td>logInv(分布概率, 算法平均数, 标准偏差)<br>返回 x 的对数累积分布函数的反函数</td>
        <td>logInv(0.1, 45, 33) <br>>>15.01122624</td>
    </tr>
    <tr>
        <td>LognormDist</td><td>lognormDist(数值, 算法平均数, 标准偏差)<br>返回反对数正态分布</td>
        <td>lognormDist(15, 23, 45) <br>>>0.326019201</td>
    </tr>
    <tr>
        <td>negbinomDist</td><td>negbinomDist(失败次数, 成功极限次数, 成功概率)<br>返回负二项式分布</td>
        <td>negbinomDist(23, 45, 0.7) <br>>>0.053463314</td>
    </tr>
    <tr>
        <td>POISSON</td><td>poisson(数值, 算法平均数, 返回类型：0/1)<br>返回 Poisson 分布</td>
        <td>POISSON(23, 23, 0) <br>>>0.082884384</td>
    </tr>
    <tr>
        <td>TDist</td><td>tDist(数值, 自由度, 返回类型：1/2)<br>返回学生的 t 分布</td>
        <td>TDist(1.2, 24, 1) <br>>>0.120925677</td>
    </tr>
    <tr>
        <td>TInv</td><td>TInv(分布概率, 自由度)<br>返回学生的 t 分布的反分布</td>
        <td>TInv(0.12, 23) <br>>>1.614756561</td>
    </tr>
    <tr>
        <td>WEIBULL</td><td>weibull(数值, 分布参数α, 分布参数β, 返回类型：0/1)<br>返回 Weibull 分布</td>
        <td>WEIBULL(1, 2, 3, 1) <br>>>0.105160683</td>
    </tr>
</table>

#### Find references
<table>
    <tr><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>VLookUp ★ ▲</td><td>VLookUp({数组, ...}, 值, {列索引}[, 模糊匹配:0/1])<br>纵向查找函数。模糊匹配默认1</td> <td></td>
    </tr>
    <tr>
        <td>VLookUp ★ ▲</td><td>VLookUp({Json, ...}, 公式字符串, 属性名)<br>JSON数组查找函数。</td> <td></td>
    </tr>
</table>

#### Add function similar to C# method
<table>
    <tr><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>UrlEncode ★</td><td>UrlEncode(文本)<br> 对 URL 字符串进行编码。</td> <td></td>
    </tr>
	<tr>
        <td>UrlDecode ★</td><td>UrlEncode(文本)<br> 将 URL 编码的字符串转换为已解码的字符串。</td> <td></td>
    </tr>
	<tr>
        <td>HtmlEncode ★</td><td>HtmlEncode(文本)<br> 将字符串转换为 HTML 编码的字符串。</td> <td></td>
    </tr>
	<tr>
        <td>HtmlDecode ★</td><td>HtmlDecode(文本)<br>  将HTML 编码的字符串转解码。</td> <td></td>
    </tr>
	<tr>
        <td>Base64ToText ★</td><td>Base64ToText(文本[, 编码类型])<br>   将Base64转换为字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Base64UrlToText ★</td><td>Base64UrlToText(文本[, 编码类型])<br>   将Url类型的Base64 转换为字符串。</td> <td></td>
    </tr>
	<tr>
        <td>TextToBase64 ★</td><td>TextToBase64(文本[, 编码类型])<br>   将字符串转换为Base64字符串。</td> <td></td>
    </tr>
	<tr>
        <td>TextToBase64Url ★</td><td>TextToBase64Url(文本[, 编码类型])<br>   将字符串 转换为Url类型的Base64 字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Regex ★ ▲</td><td>Regex(文本, 匹配文本])<br>   并返回匹配的字符串。</td> <td></td>
    </tr>
	<tr>
        <td>RegexRepalce ★</td><td>RegexRepalce(文本, 匹配文本, 替换文本)<br>  匹配替换字符串。</td> <td></td>
    </tr>
	<tr>
        <td>IsRegex ★<br>IsMatch ★</td><td>IsRegex(文本, 匹配文本)<br>IsMatch(文本, 匹配文本)<br>  判断是否匹配。</td> <td></td>
    </tr>
	<tr>
        <td>Guid</td><td>Guid()<br>  生成Guid字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Md5 ★</td><td>Md5(文本[, 编码类型])<br> 返回Md5的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Sha1 ★</td><td>Sha1(文本[, 编码类型])<br> 返回Sha1的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Sha256 ★</td><td>Sha256(文本[, 编码类型])<br> 返回Sha256的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Sha512 ★</td><td>Sha512(文本[, 编码类型])<br> 返回Sha512的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Crc32 ★</td><td>Crc32(文本[, 编码类型])<br> 返回Crc32的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>HmacMd5 ★</td><td>HmacMd5(文本, secret[, 编码类型])<br> 返回HmacMd5的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>HmacSha1 ★</td><td>HmacSha1(文本, secret[, 编码类型])<br> 返回HmacSha1的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>HmacSha256 ★</td><td>HmacSha256(文本, secret[, 编码类型])<br> 返回HmacSha256的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>HmacSha512 ★</td><td>HmacSha512(文本, secret[, 编码类型])<br> 返回HmacSha512的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>TrimStart ★<br>LTrim ★</td><td>TrimStart(文本)<br>LTrim(文本)<br>LTrim(文本[, 字符集])<br>   消空字符串左边。</td> <td></td>
    </tr>
	<tr>
        <td>TrimEnd ★<br>RTrim ★</td><td>TrimEnd(文本)<br>RTrim(文本)<br>RTrim(文本, 字符集)<br>   消空字符串右边。</td> <td></td>
    </tr>
	<tr>
        <td>IndexOf ★ ▲</td><td>IndexOf(文本, 查找文本[, 开始位置[, 索引]])<br>   查找字符串位置。</td> <td></td>
    </tr>
	<tr>
        <td>LastIndexOf ★ ▲</td><td>LastIndexOf(文本, 查找文本[, 开始位置[, 索引]])<br>   查找字符串位置。</td> <td></td>
    </tr>
	<tr>
        <td>Split ★</td><td>Split(文本, 分隔符)<br> 生成数组<br>Split(文本, 分隔符, 索引)<br>  返回分割后索引指向的字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Join ★</td><td>Join(文本1, 文本2....)<br>  合并字符串。</td> <td></td>
    </tr>
    <tr style="color:red">
        <td>Substring ★ ▲</td><td>Substring(文本, 位置)<br>Substring(文本, 位置, 数量)<br>  切割字符串。</td> <td></td>
    </tr>
	<tr>
        <td>StartsWith ★</td><td>StartsWith(文本, 开始文本[, 是否忽略大小写:1/0])<br>  确定此字符串实例的开头是否与指定的字符串匹配。</td> <td></td>
    </tr>
	<tr>
        <td>EndsWith ★</td><td>EndsWith(文本, 开始文本[, 是否忽略大小写:1/0])<br>  确定使用指定的比较选项进行比较时此字符串实例的结尾是否与指定的字符串匹配。</td> <td></td>
    </tr>
	<tr>
        <td>IsNullOrEmpty ★</td><td>IsNullOrEmpty(文本)<br>  指示指定的字符串是 null 还是 空字符串。</td> <td></td>
    </tr>	
	<tr>
        <td>IsNullOrWhiteSpace ★</td><td>IsNullOrWhiteSpace(文本)<br>  指示指定的字符串是 null、空还是仅由空白字符组成。</td> <td></td>
    </tr>
	<tr>
        <td>RemoveStart ★</td><td>RemoveStart(文本, 左边文本[, 忽略大小写])<br>匹配左边，成功则去除左边字符串。</td> <td></td>
    </tr>
	<tr>
        <td>RemoveEnd ★</td><td>RemoveEnd(文本, 右边文本[, 忽略大小写])<br>匹配右边，成功则去除右边字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Json ★</td><td>json(文本)<br>动态json查询。</td> <td></td>
    </tr>
</table>
 
