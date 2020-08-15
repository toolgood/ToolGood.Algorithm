ToolGood.Algorithm (JAVA版本)
===================
ToolGood.Algorithm支持`四则运算`、`Excel函数`,并支持`自定义参数`。

**适用场景：** 代码与算法分离，避免项目强制升级

1）项目初期，未确定的算法；

2）项目维护时，经常改动的算法；

3）财务数据、统计数据之中的算法，(注:本项目使用double类型，建议使用`分`为单位)；

4）报表导出，数据来源使用存储过程，Word文档内设置算法。(C#版本 例 https://github.com/toolgood/ToolGood.WordTemplate)

## 快速上手
``` java
    AlgorithmEngine engine = new AlgorithmEngine();
    double a=0.0;
    if (engine.Parse("1+2")) {
        var o = engine.Evaluate();
        a=o.NumberValue;
    }
    double b = engine.TryEvaluate("1=1 && 1<2 and 7-8>1", 0.0);// 支持 && || and or 
    double c = engine.TryEvaluate("2+3", 0);
    double d = engine.TryEvaluate("count({1,2,3,4})", 0.0);//{}代表数组,返回:4
    String s = engine.TryEvaluate("'aa'&'bb'", ""); //字符串连接,返回:aabb
    int r = engine.TryEvaluate("(1=1)*9+2", 0); //返回:11
    DateTime d = engine.TryEvaluate("'2016-1-1'+1", DateTime.now()); //返回日期:2016-1-2
    DateTime t = engine.TryEvaluate("'2016-1-1'+9*'1:0'", DateTime.now());//返回日期:2016-1-1 9:0
    String j = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age", "");//返回51
    String k = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare   \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')[Name].Trim()", "");//返回"William Shakespeare" (不带空格)
    String l = engine.TryEvaluate("json('{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", ""); ;//返回"ill"

```
支持常量`pi`,`e`,`true`,`false`。

数值转bool，非零为真,零为假。
字符串转bool,`0`、`FALSE`为假，`1`、`TRUE`为真。不区分大小写

bool转数值，假为`0`，真为`1`。
bool转字符串，假为`FALSE`，真为`TRUE`。

索引默认为`Excel索引`，如果想用c#索引，请设置`UseExcelIndex`为`false`。

中文符号自动转成英文符号：`括号`,`方括号`,`逗号`,`引号`,`双引号`

注：字符串拼接使用`&`。

注：`substring`为C#版本，Substring(文本,位置[,数量])

注：`find`为Excel函数，find(要查找的字符串,被查找的字符串[,开始位置])

## 自定义参数
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
```
参数以方括号定义，如 `[参数名]`。 

注：还可以使用`AddParameter`、`AddParameterFromJson`添加方法，使用`DiyFunction`来自定义函数。

## Excel函数
函数：`逻辑函数`、`数学与三角函数`、`文本函数`、`统计函数`、`日期与时间函数`

注：函数名不分大小写,带方括号的参数可省略,示例的返回值,为近似值。

注2：函数名带★，表示第一个参数可以前置，如`(-1).ISTEXT()`

注3：函数名带▲，表示受`Excel索引`影响，
#### 逻辑函数
<table>
    <tr><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>If</td><td>If(测试条件,真值[,假值])<br>执行真假值判断,根据逻辑计算的真假值,返回不同结果。</td>
        <td>If(1=1,1,2) <br>>>1</td>
    </tr>
    <tr>
        <td>IfError</td><td>IfError(测试条件,真值[,假值])<br>如果公式计算出错误则返回您指定的值；否则返回公式结果。</td>
        <td>IfError(1/0,1,2) <br>>>1</td>
    </tr>
    <tr>
        <td>IsError ★</td>
        <td>
        IsError(值)<br>判断是否出错,返回 TRUE 或 FALSE
        IsError(值,替换值)<br>如果出错,返回替换值，否则返回原值
        </td>
        <td>IsError(1) <br>>>false</td>
    </tr>
    <tr>
        <td>IsNull ★</td>
        <td>
        IsNull(值)<br>判断是否为空,返回 TRUE 或 FALSE
        IsNull(值,替换值)<br>如果为空,返回替换值，否则返回原值
        </td>
        <td>IsNull(null) <br>>>true</td>
    </tr>
    <tr>
        <td>IsNullOrError ★</td>
        <td>
        IsNullOrError(值)<br>判断是否为空或错误,返回 TRUE 或 FALSE
        IsNullOrError(值,替换值)<br>如果为空或错误,返回替换值，否则返回原值
        </td>
        <td>IsNullOrError(null) <br>>>true</td>
    </tr>
    <tr>
        <td>IsNumber ★</td><td>IsNumber(值)<br>判断是否数值,返回 TRUE 或 FALSE</td>
        <td>IsNumber(1) <br>>>true</td>
    </tr>
    <tr>
        <td>IsText ★</td><td>IsText(值)<br>判断是否文字,返回 TRUE 或 FALSE</td>
        <td>IsText('1') <br>>>true </td>
    </tr>
    <tr>
        <td>IsNonText ★</td><td>IsNonText(值)<br>判断是否为非文字,返回 TRUE 或 FALSE</td>
        <td>IsNonText('1') <br>>>false </td>
    </tr>
    <tr>
        <td>IsLogical ★</td><td>IsLogical(值)<br>判断是否为逻辑值,返回 TRUE 或 FALSE</td>
        <td>IsLogical('1') <br>>>false </td>
    </tr>
    <tr>
        <td>IsEven ★</td><td>IsEven(值)<br>如果数值是偶数,返回 TRUE 或 FALSE</td>
        <td>IsEven('1') <br>>>false </td>
    </tr>
    <tr>
        <td>IsOdd ★</td><td>IsOdd(值)<br>如果数值是奇数,返回 TRUE 或 FALSE</td>
        <td>IsOdd('1') <br>>>true </td>
    </tr>
    <tr>
        <td>And</td><td>And(逻辑值1,...)<br>如果所有参数均为TRUE,则返回TRUE</td>
        <td>And(1,2=2) <br>>>true</td>
    </tr>
    <tr>
        <td>Or</td><td>Or(逻辑值1,...)<br>如果任一参数为TRUE,则返回TRUE</td>
        <td>Or(1,2=3) <br>>>true</td>
    </tr>
    <tr>
        <td>Not</td><td>Not(逻辑值)<br>对参数的逻辑值求反</td>
        <td>Not(true()) <br>>>false</td>
    </tr>
    <tr>
        <td>True</td><td>True()<br>返回逻辑值TRUE</td>
        <td>True() <br>>>true</td>
    </tr>
    <tr>
        <td>False</td><td>False()<br>返回逻辑值FALSE</td>
        <td>False() <br>>>false</td>
    </tr>
</table>

#### 数学与三角函数
<table>
    <tr><td>分类</td><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td rowspan="14">基<br><br>础<br><br>数<br><br>学</td>
        <td>E</td><td>e()<br>返回 e 值</td>
        <td>E() <br>>> </td>
    </tr>
    <tr>
        <td>Pi</td><td>Pi()<br>返回 PI 值</td>
        <td>Pi() <br>>>3.141592654</td>
    </tr>
    <tr>
        <td>Abs</td><td>Abs(数值)<br>返回数值的绝对值</td>
        <td>Abs(-1) <br>>>1</td>
    </tr>
    <tr>
        <td>Quotient</td><td>Quotient(除数,被除数)<br>返回商的整数部分,该函数可用于舍掉商的小数部分。</td>
        <td>Quotient(7,3) <br>>>2</td>
    </tr>
    <tr>
        <td>Mod</td><td>Mod(除数,被除数)<br>返回两数相除的余数</td>
        <td>Mod(7,3) <br>>>1</td>
    </tr>
    <tr>
        <td>Sign</td><td>Sign(数值)<br>返回数值的符号。当数值为正数时返回 1,为零时返回 0,为负数时返回 -1。</td>
        <td>Sign(-9) <br>>>-1</td>
    </tr>
   <tr>
        <td>Sqrt</td><td>Sqrt(数值)<br>返回正平方根</td>
        <td>Sqrt(9) <br>>>3</td>
    </tr>
    <tr>
        <td>Trunc</td><td>Trunc(数值)<br>将数值截尾取整</td>
        <td>Trunc(9.222) <br>>>9</td>
    </tr>
    <tr>
        <td>Int ★</td><td>Int(数值)<br>将数值向下舍入到最接近的整数。</td>
        <td>Int(9.222) <br>>>9</td>
    </tr>
    <tr>
        <td>Gcd</td><td>Gcd(数值,...)<br>返回最大公约数</td>
        <td>Gcd(3,5,7) <br>>>1</td>
    </tr>
    <tr>
        <td>Lcm</td><td>Lcm(数值,...)<br>返回整数参数的最小公倍数</td>
        <td>Lcm(3,5,7) <br>>>105</td>
    </tr>
    <tr>
        <td>Combin</td><td>Combin(总数,排列数)<br>计算从给定数目的对象集合中提取若干对象的组合数</td>
        <td>Combin(10,2) <br>>>45</td>
    </tr>
    <tr>
        <td>Permut</td><td>Permut(总数,排列数)<br>返回从给定数目的对象集合中选取的若干对象的排列数</td>
        <td>Permut(10,2) <br>>>990</td>
    </tr>
    <tr>
        <td>Fixed</td><td>Fixed(数值[,小数位数[,有无逗号分隔符]])<br>将数值设置为具有固定小数位的文本格式</td>
        <td>Fixed(4567.89,1) <br>>>4,567.9</td>
    </tr>
    <tr>
    <td rowspan="15">三<br><br>角<br><br>函<br><br>数</td>
        <td>Degrees</td><td>Degrees(弧度)<br>将弧度转换为度</td>
        <td>Degrees(pi()) <br>>>180</td>
    </tr>
    <tr>
        <td>Radians</td><td>Radians(度)<br>将度转换为弧度</td>
        <td>Radians(180) <br>>>3.141592654</td>
    </tr>
    <tr>
        <td>Cos</td><td>Cos(弧度)<br>返回数值的余弦值</td>
        <td>Cos(1) <br>>>0.540302305868</td>
    </tr>
    <tr>
        <td>Cosh</td><td>Cosh(弧度)<br>返回数值的双曲余弦值</td>
        <td>Cosh(1) <br>>>1.54308063481</td>
    </tr>
    <tr>
        <td>Sin</td><td>Sin(弧度)<br>返回给定角度的正弦值</td>
        <td>Sin(1) <br>>>0.84147098480</td>
    </tr>
    <tr>
        <td>Sinh</td><td>Sinh(弧度)<br>返回数值的双曲正弦值</td>
        <td>Sinh(1) <br>>>1.1752011936</td>
    </tr>
    <tr>
        <td>Tan</td><td>Tan(弧度)<br>返回数值的正切值</td>
        <td>Tan(1) <br>>>1.55740772465</td>
    </tr>
    <tr>
        <td>Tanh</td><td>Tanh(弧度)<br>返回数值的双曲正切值</td>
        <td>Tanh(1) <br>>>0.761594155955</td>
    </tr>
    <tr>
        <td>Acos</td><td>Acos(数值)<br>返回数值的反余弦值</td>
        <td>Acos(0.5) <br>>>1.04719755119</td>
    </tr>
    <tr>
        <td>Acosh</td><td>Acosh(数值)<br>返回数值的反双曲余弦值</td>
        <td>Acosh(1.5) <br>>>0.962423650119</td>
    </tr>
    <tr>
        <td>Asin</td><td>Asin(数值)<br>返回数值的反正弦值</td>
        <td>Asin(0.5) <br>>>0.523598775598</td>
    </tr>
    <tr>
        <td>Asinh</td><td>Asinh(数值)<br>返回数值的反双曲正弦值。</td>
        <td>Asinh(1.5) <br>>>1.1947632172</td>
    </tr>
    <tr>
        <td>Atan</td><td>Atan(数值)<br>返回数值的反正切值</td>
        <td>Atan(1) <br>>>0.785398163397</td>
    </tr>
   <tr>
        <td>Atanh</td><td>Atanh(数值)<br>返回参数的反双曲正切值</td>
        <td>Atanh(1) <br>>>0.549306144334</td>
    </tr>
    <tr>
        <td>Atan2</td><td>Atan2(数值,数值)<br>从X和Y坐标返回反正切</td>
        <td>Atan2(1,2) <br>>>1.10714871779</td>
    </tr>
    <tr>
        <td rowspan="8">四<br><br>舍<br><br>五<br><br>入</td>
        <td>Round</td><td>Round(数值,小数位数)<br>返回某个数值按指定位数取整后的数值。</td>
        <td>Round(4.333,2) <br>>>4.33</td>
    </tr>
    <tr>
        <td>RoundDown</td><td>RoundDown(数值,小数位数)<br>靠近零值,向下（绝对值减小的方向）舍入数值。</td>
        <td>RoundDown(4.333,2) <br>>>4.33</td>
    </tr>
    <tr>
        <td>RoundUp</td><td>RoundUp(数值,小数位数)<br>远离零值,向上（绝对值增长的方向）舍入数值。</td>
        <td>RoundUp(4.333,2) <br>>>4.34</td>
    </tr>
    <tr>
        <td>Ceiling</td><td>Ceiling(数值,舍入基数)<br>向上舍入（沿绝对值增大的方向）为最接近的 舍入基数 的倍数。</td>
        <td>Ceiling(4.333,0.1) <br>>>4.4</td>
    </tr>
    <tr>
        <td>Floor</td><td>Floor(数值,舍入基数)<br>向下舍入,使其等于最接近的 Significance 的倍数。</td>
        <td>Floor(4.333,0.1) <br>>>4.3</td>
    </tr>
    <tr>
        <td>Even</td><td>Even(数值)<br>返回沿绝对值增大方向取整后最接近的偶数。</td>
        <td>Even(3) <br>>>4</td>
    </tr>
    <tr>
        <td>Odd</td><td>Odd(数值)<br>将数值向上舍入为最接近的奇型整数</td>
        <td>Odd(3.1) <br>>>5</td>
    </tr>
    <tr>
        <td>MRound</td><td>MRound(数值,舍入基数)<br>返回一个舍入到所需倍数的数值</td>
        <td>MRound(13,5) <br>>>15</td>
    </tr>
    <tr>
        <td rowspan="2">随<br><br>机<br><br>数</td>
        <td>Rand</td><td>Rand()<br>返回 0 到 1 之间的随机数 </td>
        <td>Rand() <br>>>0.2</td>
    </tr>
    <tr>
        <td>RandBetween</td><td>RandBetween(最小整数,最大整数)<br>返回大于等于指定的最小值,小于指定最大值之间的一个随机整数。</td>
        <td>RandBetween(2,44) <br>>>9</td>
    </tr>
    <tr>
        <td rowspan="11">幂<br><br>/<br><br>对<br><br>数<br><br>/<br><br>阶<br><br>乘</td>
        <td>Fact</td><td>Fact(数值)<br>返回数的阶乘,一个数的阶乘等于 1*2*3*…* 该数。</td>
        <td>Fact(3) <br>>>6</td>
    </tr>
    <tr>
        <td>FactDouble</td><td>FactDouble(数值)<br>返回数值的双倍阶乘</td>
        <td>FactDouble(10) <br>>>3840</td>
    </tr>
    <tr>
        <td>Power</td><td>Power(数值,幂)<br>返回数的乘幂结果</td>
        <td>Power(10,2) <br>>>100</td>
    </tr>
    <tr>
        <td>Exp</td><td>Exp(幂)<br>返回e的指定数乘幂</td>
        <td>Exp(2) <br>>>7.389056099</td>
    </tr>
    <tr>
        <td>Ln</td><td>Ln(数值)<br>返回数值的自然对数</td>
        <td>Ln(4) <br>>>1.386294361</td>
    </tr>
    <tr>
        <td>Log</td><td>Log(数值[,底数])<br>返回数值的常用对数,如省略底数,默认为10</td>
        <td>Log(100,10) <br>>>2</td>
    </tr>
    <tr>
        <td>Log10</td><td>Log10(数值)<br>返回数值的10对数</td>
        <td>Log10(100) <br>>>2</td>
    </tr>
    <tr>
        <td>Multinomial</td><td>Multinomial(数值,...)<br>返回参数和的阶乘与各参数阶乘乘积的比值</td>
        <td>Multinomial(1,2,3) <br>>>60</td>
    </tr>
    <tr>
        <td>Product</td><td>Product(数值,...)<br>将所有以参数形式给出的数值相乘,并返回乘积值。</td>
        <td>Product(1,2,3,4) <br>>>24</td>
    </tr>
    <tr>
        <td>SqrtPi</td><td>SqrtPi(数值)<br>返回某数与 PI 的乘积的平方根</td>
        <td>SqrtPi(3) <br>>>3.069980124</td>
    </tr>
    <tr>
        <td>SumQq</td><td>SumQq(数值,...)<br>返回参数的平方和</td>
        <td>SumQq(1,2) <br>>>5</td>
    </tr>
    <tr>
        <td rowspan="12">转<br><br>化</td>
        <td>Dec2Bin ★</td><td>Dec2Bin(数值[,位数])<br>十进制转二进制 </td>
        <td>Dec2Bin(100) <br>>> </td>
    </tr>
    <tr>
        <td>Dec2Oct ★</td><td>Dec2Oct(数值[,位数])<br>十进制转八进制 </td>
        <td>Dec2Oct(100) <br>>> </td>
    </tr>
    <tr>
        <td>Dec2Hex ★</td><td>Dec2Hex(数值[,位数])<br>十进制转十六进制 </td>
        <td>Dec2Hex(100) <br>>> </td>
    </tr>
    <tr>
        <td>Hex2Bin ★</td><td>Hex2Bin(数值[,位数])<br>十六进制转二进制 </td>
        <td>Hex2Bin(100) <br>>> </td>
    </tr>
    <tr>
        <td>Hex2Oct ★</td><td>Hex2Oct(数值[,位数])<br>十六进制转八进制 </td>
        <td>Hex2Oct(100) <br>>> </td>
    </tr>
    <tr>
        <td>Hex2Dec ★</td><td>Hex2Dec(数值)<br>十六进制转十进制 </td>
        <td>Hex2Dec(100) <br>>> </td>
    </tr>
    <tr>
        <td>Oct2Bin ★</td><td>Oct2Bin(数值[,位数])<br>八进制转二进制 </td>
        <td>Oct2Bin(100) <br>>> </td>
    </tr>
    <tr>
        <td>Oct2Dec ★</td><td>Oct2Dec(数值)<br>八进制转十进制 </td>
        <td>Oct2Dec(100) <br>>> </td>
    </tr>
    <tr>
        <td>Oct2Hex ★</td><td>Oct2Hex(数值[,位数])<br>八进制转十六进制 </td>
        <td>Oct2Hex(100) <br>>> </td>
    </tr>
    <tr>
        <td>Bin2Oct ★</td><td>Bin2Oct(数值[,位数])<br>二进制转八进制 </td>
        <td>Bin2Oct(100) <br>>> </td>
    </tr>
    <tr>
        <td>Bin2Dec ★</td><td>Bin2Dec(数值)<br>二进制转十进制 </td>
        <td>Bin2Dec(100) <br>>> </td>
    </tr>
    <tr>
        <td>Bin2Hex ★</td><td>Bin2Hex(数值[,位数])<br>二进制转十六进制 </td>
        <td>Bin2Hex(100) <br>>> </td>
    </tr>
</table>

#### 文本函数
<table>
    <tr><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>Asc ★</td><td>Asc(字符串)<br>将字符串内的全角英文字母更改为半角字符</td>
        <td>Asc('ａｂｃＡＢＣ１２３') <br>>>abcABC123</td>
    </tr>
    <tr>
        <td>Jis ★<br> WideChar ★</td><td>Jis(字符串)<br>将字符串中的半角英文字符更改为全角字符</td>
        <td>Jis('abcABC123') <br>>>ａｂｃＡＢＣ１２３</td>
    </tr>
    <tr>
        <td>Char ★</td><td>Char(数值)<br>返回由代码数值指定的字符</td>
        <td>Char(49) <br>>>1</td>
    </tr>
    <tr>
        <td>Clean ★</td><td>Clean(字符串)<br>删除文本中所有打印不出的字符</td>
        <td>cleCleanan('\r112\t') <br>>>112</td>
    </tr>
    <tr>
        <td>Code ★</td><td>Code(字符串)<br>返回文本字符串中第一个字符的数值代码</td>
        <td>Code("1") <br>>>49</td>
    </tr>
    <tr>
        <td>Concatenate ★</td><td>Concatenate(字符串1,...)<br>将若干文本项合并到一个文本项中</td>
        <td>Concatenate('tt','11') <br>>>tt11</td>
    </tr>
    <tr>
        <td>Exact ★</td><td>Exact(字符串1,字符串2)<br>检查两个文本值是否完全相同</td>
        <td>Exact("11","22") <br>>>false</td>
    </tr>
    <tr>
        <td>Find ★ ▲</td><td>Find(要查找的字符串,被查找的字符串[,开始位置])<br>在一文本值内查找另一文本值（区分大小写） </td>
        <td>Find("11","12221122") <br>>>5</td>
    </tr>
    <tr>
        <td>Left ★</td><td>Left(字符串[,字符个数])<br>返回文本值最左边的字符</td>
        <td>Left('123222',3) <br>>>123</td>
    </tr>
    <tr>
        <td>Len ★</td><td>Len(字符串)<br>返回文本字符串中的字符个数</td>
        <td>Len('123222') <br>>>6</td>
    </tr>
    <tr>
        <td>Mid ★  ▲</td><td>Mid(字符串,开始位置,字符个数)<br>从文本字符串中的指定位置起返回特定个数的字符</td>
        <td>Mid('ABCDEF',2,3) <br>>>BCD</td>
    </tr>
    <tr>
        <td>Proper ★</td><td>Proper(字符串)<br>将文本值中每一个单词的首字母设置为大写</td>
        <td>Proper('abc abc') <br>>>Abc Abc</td>
    </tr>
    <tr>
        <td>Replace ★  ▲</td>
        <td>Replace(原字符串,开始位置,字符个数,新字符串)<br>
        replace(原字符串,要替换的字符串, 新字符串)<br>
        替换文本内的字符</td>
        <td>Replace("abccd",2,3,"2") <br>>>a2d<br>
        Replace("abccd","bc","2") <br>>>a2cd
        </td>
    </tr>
    <tr>
        <td>Rept ★</td><td>Rept(字符串,重复次数)<br>按给定次数重复文本</td>
        <td>Rept("q",3) <br>>>qqq</td>
    </tr>
    <tr>
        <td>Right ★</td><td>Right(字符串[,字符个数])<br>返回文本值最右边的字符</td>
        <td>Right("123q",3) <br>>>23q</td>
    </tr>
    <tr>
        <td>RMB ★</td><td>RMB(数值)<br>将数值转换为大写数值文本</td>
        <td>RMB(12.3) <br>>>壹拾贰元叁角</td>
    </tr>
    <tr>
        <td>Search ★ ▲</td><td>Search(要找的字符串,被查找的字符串[,开始位置])<br>在一文本值中查找另一文本值（不区分大小写）</td>
        <td>Search("aa","abbAaddd") <br>>>4</td>
    </tr>
    <tr>
        <td>Substitute ★</td><td>Substitute(字符串,原字符串,新字符串[,替换序号])<br>在文本字符串中以新文本替换旧文本</td>
        <td>Substitute("ababcc","ab","12") <br>>>1212cc</td>
    </tr>
    <tr>
        <td>T ★</td><td>t(数值)<br>将参数转换为文本</td>
        <td>T('123') <br>>>123</td>
    </tr>
    <tr>
        <td>Text ★</td><td>Text(数值,数值格式)<br>设置数值的格式并将数值转换为文本</td>
        <td>Text(123,"0.00") <br>>>123.00</td>
    </tr>
    <tr>
        <td>Trim ★</td><td>Trim(字符串)<br>删除文本中的空格</td>
        <td>Trim(" 123 123 ")<br>>>123 123</td>
    </tr>
    <tr>
        <td>Lower ★<br>ToLower ★</td><td>lower(字符串)<br>tolower(字符串)<br>将文本转换为小写形式</td>
        <td>Lower('ABC') <br>>>abc</td>
    </tr>
    <tr>
        <td>Upper ★<br>ToUpper ★</td><td>upper(字符串)<br>toupper(字符串)<br>将文本转换为大写形式</td>
        <td>Upper("abc") <br>>>ABC</td>
    </tr>
    <tr>
        <td>Value ★</td><td>Value(字符串)<br>将文本参数转换为数值</td>
        <td>Value("123") <br>>>123</td>
    </tr>
</table>

#### 日期与时间函数
<table>
    <tr><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>Now</td><td>Now()<br>返回当前日期和时间的序列号</td>
        <td>Now() <br>>>2017-01-07 11:00:00</td>
    </tr>
    <tr>
        <td>Today</td><td>Today()<br>返回今天日期的序列号</td>
        <td>Today() <br>>>2017-01-07</td>
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
        <td>Date ★</td><td>Date(年,月,日[,时[,分[,秒]]])<br>返回特定日期的序列号</td>
        <td>Date(2016,1,1) <br>>>2016-01-01</td>
    </tr>
    <tr>
        <td>Time ★</td><td>Time(时,分,秒)<br>返回特定时间的序列号</td>
        <td>Time(12,13,14) <br>>>12:13:14</td>
    </tr>
    <tr>
        <td>Year ★</td><td>Year(日期)<br>将序列号转换为年</td>
        <td>Year(NOW()) <br>>>2017</td>
    </tr>
    <tr>
        <td>Month ★</td><td>Month(日期)<br>将序列号转换为月</td>
        <td>Month(NOW()) <br>>>1</td>
    </tr>
    <tr>
        <td>Day ★</td><td>Day(日期)<br>将序列号转换为月份中的日</td>
        <td>Day(NOW()) <br>>>7</td>
    </tr>
    <tr>
        <td>Hour ★</td><td>Hour(日期)<br>将序列号转换为小时</td>
        <td>Hour(NOW()) <br>>>11</td>
    </tr>
    <tr>
        <td>Minute ★</td><td>Minute(日期)<br>将序列号转换为分钟</td>
        <td>Minute(NOW()) <br>>>12</td>
    </tr>
    <tr>
        <td>Second ★</td><td>Second(日期)<br>将序列号转换为秒</td>
        <td>Second(NOW()) <br>>>34</td>
    </tr>
    <tr>
        <td>Weekday ★</td><td>Weekday(日期)<br>将序列号转换为星期几</td>
        <td>Weekday(date(2017,1,7)) <br>>>7</td>
    </tr>
    <tr>
        <td>DateDif</td><td>DateDif(开始日期,结束日期,类型Y/M/D/YD/MD/YM)<br>返回两个日期之间的相隔天数</td>
        <td>DateDif("1975-1-30","2017-1-7","Y") <br>>>41</td>
    </tr>
    <tr>
        <td>Days360</td><td>Days360(开始日期,结束日期[,选项0/1])<br>以一年 360 天为基准计算两个日期间的天数</td>
        <td>Days360('1975-1-30','2017-1-7') <br>>>15097</td>
    </tr>
    <tr>
        <td>EDate</td><td>EDate(开始日期,月数)<br>返回用于表示开始日期之前或之后月数的日期的序列号</td>
        <td>EDate("2012-1-31",32) <br>>>2014-09-30</td>
    </tr>
    <tr>
        <td>EoMonth</td><td>EoMonth(开始日期,月数)<br>返回指定月数之前或之后的月份的最后一天的序列号</td>
        <td>EoMonth("2012-2-1",32) <br>>>2014-10-31</td>
    </tr>
    <tr>
        <td>NetWorkdays</td><td>NetWorkdays(开始日期,结束日期[,假日])<br>返回两个日期之间的全部工作日数</td>
        <td>NetWorkdays("2012-1-1","2013-1-1") <br>>>262</td>
    </tr>
    <tr>
        <td>Workday</td><td>Workday(开始日期,天数[,假日])<br>返回指定的若干个工作日之前或之后的日期的序列号</td>
        <td>Workday("2012-1-2",145) <br>>>2012-07-23</td>
    </tr>
    <tr>
        <td>WeekNum</td><td>WeekNum(日期[,类型：1/2])<br>将序列号转换为一年中相应的周数</td>
        <td>WeekNum("2016-1-3") <br>>>2</td>
    </tr>
</table>

#### 统计函数
<table>
    <tr><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>Max</td><td>Max(数值,...)<br>返回参数列表中的最大值</td>
        <td>Max(1,2,3,4,2,2,1,4) <br>>>4</td>
    </tr>
    <tr>
        <td>Median</td><td>Median(数值,...)<br>返回给定数值的中值</td>
        <td>Median(1,2,3,4,2,2,1,4) <br>>>2</td>
    </tr>
    <tr>
        <td>Min</td><td>Min(数值,...)<br>返回参数列表中的最小值</td>
        <td>Min(1,2,3,4,2,2,1,4) <br>>>1</td>
    </tr>
    <tr>
        <td>Quartile</td><td>Quartile(数值,四分位：0-4)<br>返回数据集的四分位数</td>
        <td>Quartile({1,2,3,4,2,2,1,4},0) <br>>>1</td>
    </tr>
    <tr>
        <td>Mode</td><td>Mode(数值,...)<br>返回在数组中出现频率最多的数值</td>
        <td>Mode(1,2,3,4,2,2,1,4) <br>>>2</td>
    </tr>
    <tr>
        <td>Large ▲</td><td>Large(数组,K)<br>返回数据集中第 k 个最大值</td>
        <td>Large({1,2,3,4,2,2,1,4},3) <br>>>3</td>
    </tr>
    <tr>
        <td>Small ▲</td><td>Small(数值,K)<br>返回数据集中第 k 个最小值</td>
        <td>Small({1,2,3,4,2,2,1,4},3) <br>>>2</td>
    </tr>
    <tr>
        <td>Percentile</td><td>Percentile(数值,K)<br>返回区域中的第 k 个百分位值</td>
        <td>Percentile({1,2,3,4,2,2,1,4},0.4) <br>>>2</td>
    </tr>
    <tr>
        <td>PercentRank</td><td>PercentRank(数值,K)<br>返回数据集中值的百分比排位</td>
        <td>PercentRank({1,2,3,4,2,2,1,4},3) <br>>>0.714</td>
    </tr>
    <tr>
        <td>Average</td><td>Average(数值,...)<br>返回参数的平均值</td>
        <td>Average(1,2,3,4,2,2,1,4) <br>>>2.375</td>
    </tr>
    <tr>
        <td>AverageIf</td><td>AverageIf({数值,...},条件[,{值1,...}])<br>返回参数的平均值</td>
        <td>AverageIf({1,2,3,4,2,2,1,4},'>1') <br>>>2.833333333</td>
    </tr>
    <tr>
        <td>GeoMean</td><td>GeoMean(数值,...)<br>返回正数数组或区域的几何平均值</td>
        <td>GeoMean(1,2,3,4) <br>>>2.213363839</td>
    </tr>
    <tr>
        <td>HarMean</td><td>HarMean(数值,...)<br>返回数据集合的调和平均值</td>
        <td>HarMean(1,2,3,4) <br>>>1.92</td>
    </tr>
    <tr>
        <td>Count</td><td>Count(数值,...)<br>计算参数列表中数值的个数</td>
        <td>Count(1,2,3,4,2,2,1,4) <br>>>8</td>
    </tr>
    <tr>
        <td>CountIf</td><td>CountIf({数值,...},条件[,{值1,...}])<br>计算参数列表中数值的个数</td>
        <td>CountIf({1,2,3,4,2,2,1,4},'>1') <br>>>6</td>
    </tr>
    <tr>
        <td>Sum</td><td>Sum(数值,...)<br>返回所有数值之和。</td>
        <td>Sum(1,2,3,4) <br>>>10</td>
    </tr>
    <tr>
        <td>SumIf</td><td>SumIf({数值,...},条件[,{值1,...}])<br>返回所有数值之和</td>
        <td>SumIf({1,2,3,4,2,2,1,4},'>1') <br>>>17</td>
    </tr>
    <tr>
        <td>Avedev</td><td>Avedev(数值,...)<br>返回数据点与其平均值的绝对偏差的平均值</td>
        <td>Avedev(1,2,3,4,2,2,1,4) <br>>>0.96875</td>
    </tr>
    <tr>
        <td>Stdev</td><td>Stdev(数值,...)<br>基于样本估算标准偏差</td>
        <td>Stdev(1,2,3,4,2,2,1,4) <br>>>1.1877349391654208</td>
    </tr>
    <tr>
        <td>Stdevp</td><td>Stdevp(数值,...)<br>计算基于整个样本总体的标准偏差</td>
        <td>Stdevp(1,2,3,4,2,2,1,4) <br>>>1.1110243021644486</td>
    </tr>
    <tr>
        <td>DevSq</td><td>DevSq(数值,...)<br>返回偏差的平方和</td>
        <td>DevSq(1,2,3,4,2,2,1,4) <br>>>9.875</td>
    </tr>
    <tr>
        <td>Var</td><td>Var(数值,...)<br>基于样本估算方差</td>
        <td>Var(1,2,3,4,2,2,1,4) <br>>>1.4107142857142858</td>
    </tr>
    <tr>
        <td>VarP</td><td>VarP(数值,...)<br>基于整个样本总体计算方差</td>
        <td>VarP(1,2,3,4,2,2,1,4) <br>>>1.234375</td>
    </tr>
    <tr>
        <td>NormDist</td><td>NormDist(数值,算术平均值,标准偏差,返回类型：0/1)<br>返回正态累积分布</td>
        <td>NormDist(3,8,4,1) <br>>>0.105649774</td>
    </tr>
    <tr>
        <td>NormInv</td><td>NormInv(分布概率,算术平均值,标准偏差)<br>返回反正态累积分布</td>
        <td>NormInv(0.8,8,3) <br>>>10.5248637</td>
    </tr>
    <tr>
        <td>NormSDist</td><td>NormSDist(数值)<br>返回标准正态累积分布函数,该分布的平均值为 0,标准偏差为 1。</td>
        <td>NormSDist(1) <br>>>0.841344746</td>
    </tr>
    <tr>
        <td>NormSInv</td><td>NormSInv(数值)<br>返回反标准正态累积分布</td>
        <td>NormSInv(0.3) <br>>>-0.524400513</td>
    </tr>
    <tr>
        <td>BetaDist</td><td>BetaDist(数值,分布参数α,分布参数β)<br>返回 Beta 累积分布函数</td>
        <td>BetaDist(0.5,11,22) <br>>>0.97494877</td>
    </tr>
    <tr>
        <td>BetaInv</td><td>BetaInv(数值,分布参数α,分布参数β)<br>返回指定 Beta 分布的累积分布函数的反函数</td>
        <td>BetaInv(0.5,23,45) <br>>>0.336640759</td>
    </tr>
    <tr>
        <td>BinomDist</td><td>BinomDist(试验成功次数,试验次数,成功概率,返回类型：0/1)<br>返回一元二项式分布概率</td>
        <td>BinomDist(12,45,0.5,0) <br>>>0.000817409</td>
    </tr>
    <tr>
        <td>ExponDist</td><td>ExponDist(函数值,参数值,返回类型：0/1)<br>返回指数分布</td>
        <td>ExponDist(3,1,0) <br>>>0.049787068</td>
    </tr>
    <tr>
        <td>FDist</td><td>fDist(数值X,分子自由度,分母自由度)<br>返回 F 概率分布</td>
        <td>FDist(0.4,2,3) <br>>>0.701465776</td>
    </tr>
    <tr>
        <td>FInv</td><td>fInv(分布概率,分子自由度,分母自由度)<br>返回 F 概率分布的反函数</td>
        <td>FInv(0.7,2,3) <br>>>0.402651432</td>
    </tr>
    <tr>
        <td>Fisher</td><td>Fisher(数值)<br>返回点 x 的 Fisher 变换。该变换生成一个正态分布而非偏斜的函数</td>
        <td>Fisher(0.68) <br>>>0.8291140383</td>
    </tr>
    <tr>
        <td>FisherInv</td><td>FisherInv(数值)<br>返回 Fisher 变换的反函数值。</td>
        <td>FisherInv(0.6) <br>>>0.537049567</td>
    </tr>
    <tr>
        <td>GammaDist</td><td>GammaDist(数值,分布参数α,分布参数β,返回类型：0/1)<br>返回 γ 分布</td>
        <td>GammaDist(0.5,3,4,0) <br>>>0.001723627</td>
    </tr>
    <tr>
        <td>GammaInv</td><td>GammaInv(分布概率,分布参数α,分布参数β)<br>返回 γ 累积分布函数的反函数</td>
        <td>GammaInv(0.2,3,4) <br>>>6.140176811</td>
    </tr>
    <tr>
        <td>GammaLn</td><td>GammaLn(数值)<br>返回 γ 累积分布函数的反函数</td>
        <td>GammaLn(4) <br>>>1.791759469</td>
    </tr>
    <tr>
        <td>HypgeomDist</td><td>HypgeomDist(样本成功次数,样本容量,样本总体成功次数,样本总体容量)<br>返回超几何分布</td>
        <td>HypgeomDist(23,45,45,100) <br>>>0.08715016</td>
    </tr>
    <tr>
        <td>LogInv</td><td>LogInv(分布概率,算法平均数,标准偏差)<br>返回 x 的对数累积分布函数的反函数</td>
        <td>LogInv(0.1,45,33) <br>>>15.01122624</td>
    </tr>
    <tr>
        <td>LognormDist</td><td>LognormDist(数值,算法平均数,标准偏差)<br>返回反对数正态分布</td>
        <td>LognormDist(15,23,45) <br>>>0.326019201</td>
    </tr>
    <tr>
        <td>NegbinomDist</td><td>NegbinomDist(失败次数,成功极限次数,成功概率)<br>返回负二项式分布</td>
        <td>NegbinomDist(23,45,0.7) <br>>>0.053463314</td>
    </tr>
    <tr>
        <td>Poisson</td><td>poisson(数值,算法平均数,返回类型：0/1)<br>返回 Poisson 分布</td>
        <td>Poisson(23,23,0) <br>>>0.082884384</td>
    </tr>
    <tr>
        <td>TDist</td><td>tDist(数值,自由度,返回类型：1/2)<br>返回学生的 t 分布</td>
        <td>TDist(1.2,24,1) <br>>>0.120925677</td>
    </tr>
    <tr>
        <td>TInv</td><td>TInv(分布概率,自由度)<br>返回学生的 t 分布的反分布</td>
        <td>TInv(0.12,23) <br>>>1.614756561</td>
    </tr>
    <tr>
        <td>Weibull</td><td>weibull(数值,分布参数α,分布参数β,返回类型：0/1)<br>返回 Weibull 分布</td>
        <td>Weibull(1,2,3,1) <br>>>0.105160683</td>
    </tr>
</table>

#### 查找引用
<table>
    <tr><td>函数名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>VLookUp ★ ▲</td><td>VLookUp({数组,...},值,{列索引}[,模糊匹配:0/1])<br>纵向查找函数。模糊匹配默认1</td> <td></td>
    </tr>
    <tr>
        <td>VLookUp ★ ▲</td><td>VLookUp({Json,...},公式字符串,属性名)<br>JSON数组查找函数。</td> <td></td>
    </tr>
</table>

#### 增加函数 类C#方法
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
        <td>Base64ToText ★</td><td>Base64ToText(文本[,编码类型])<br>   将Base64转换为字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Base64UrlToText ★</td><td>Base64UrlToText(文本[,编码类型])<br>   将Url类型的Base64 转换为字符串。</td> <td></td>
    </tr>
	<tr>
        <td>TextToBase64 ★</td><td>TextToBase64(文本[,编码类型])<br>   将字符串转换为Base64字符串。</td> <td></td>
    </tr>
	<tr>
        <td>TextToBase64Url ★</td><td>TextToBase64Url(文本[,编码类型])<br>   将字符串 转换为Url类型的Base64 字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Regex ★ ▲</td><td>Regex(文本,匹配文本])<br>   并返回匹配的字符串。</td> <td></td>
    </tr>
	<tr>
        <td>RegexRepalce ★</td><td>RegexRepalce(文本,匹配文本,替换文本)<br>  匹配替换字符串。</td> <td></td>
    </tr>
	<tr>
        <td>IsRegex ★<br>IsMatch ★</td><td>IsRegex(文本,匹配文本)<br>IsMatch(文本,匹配文本)<br>  判断是否匹配。</td> <td></td>
    </tr>
	<tr>
        <td>Guid</td><td>Guid()<br>  生成Guid字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Md5 ★</td><td>Md5(文本[,编码类型])<br> 返回Md5的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Sha1 ★</td><td>Sha1(文本[,编码类型])<br> 返回Sha1的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Sha256 ★</td><td>Sha256(文本[,编码类型])<br> 返回Sha256的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Sha512 ★</td><td>Sha512(文本[,编码类型])<br> 返回Sha512的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Crc32 ★</td><td>Crc32(文本[,编码类型])<br> 返回Crc32的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>HmacMd5 ★</td><td>HmacMd5(文本,secret[,编码类型])<br> 返回HmacMd5的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>HmacSha1 ★</td><td>HmacSha1(文本,secret[,编码类型])<br> 返回HmacSha1的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>HmacSha256 ★</td><td>HmacSha256(文本,secret[,编码类型])<br> 返回HmacSha256的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>HmacSha512 ★</td><td>HmacSha512(文本,secret[,编码类型])<br> 返回HmacSha512的Hash字符串。</td> <td></td>
    </tr>
	<tr>
        <td>TrimStart ★<br>LTrim ★</td><td>TrimStart(文本)<br>LTrim(文本)<br>LTrim(文本[,字符集])<br>   消空字符串左边。</td> <td></td>
    </tr>
	<tr>
        <td>TrimEnd ★<br>RTrim ★</td><td>TrimEnd(文本)<br>RTrim(文本)<br>RTrim(文本,字符集)<br>   消空字符串右边。</td> <td></td>
    </tr>
	<tr>
        <td>IndexOf ★ ▲</td><td>IndexOf(文本,查找文本[,开始位置[,索引]])<br>   查找字符串位置。</td> <td></td>
    </tr>
	<tr>
        <td>LastIndexOf ★ ▲</td><td>LastIndexOf(文本,查找文本[,开始位置[,索引]])<br>   查找字符串位置。</td> <td></td>
    </tr>
	<tr>
        <td>Split ★</td><td>Split(文本,分隔符)<br> 生成数组<br>Split(文本,分隔符,索引)<br>  返回分割后索引指向的字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Join ★</td><td>Join(文本1,文本2....)<br>  合并字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Substring ★ ▲</td><td>Substring(文本,位置)<br>Substring(文本,位置,数量)<br>  切割字符串。</td> <td></td>
    </tr>
	<tr>
        <td>StartsWith ★</td><td>StartsWith(文本,开始文本[,是否忽略大小写:1/0])<br>  确定此字符串实例的开头是否与指定的字符串匹配。</td> <td></td>
    </tr>
	<tr>
        <td>EndsWith ★</td><td>EndsWith(文本,开始文本[,是否忽略大小写:1/0])<br>  确定使用指定的比较选项进行比较时此字符串实例的结尾是否与指定的字符串匹配。</td> <td></td>
    </tr>
	<tr>
        <td>IsNullOrEmpty ★</td><td>IsNullOrEmpty(文本)<br>  指示指定的字符串是 null 还是 空字符串。</td> <td></td>
    </tr>	
	<tr>
        <td>IsNullOrWhiteSpace ★</td><td>IsNullOrWhiteSpace(文本)<br>  指示指定的字符串是 null、空还是仅由空白字符组成。</td> <td></td>
    </tr>
	<tr>
        <td>RemoveStart ★</td><td>RemoveStart(文本,左边文本[,忽略大小写])<br>匹配左边，成功则去除左边字符串。</td> <td></td>
    </tr>
	<tr>
        <td>RemoveEnd ★</td><td>RemoveEnd(文本,右边文本[,忽略大小写])<br>匹配右边，成功则去除右边字符串。</td> <td></td>
    </tr>
	<tr>
        <td>Json ★</td><td>json(文本)<br>动态json查询。</td> <td></td>
    </tr>
</table>


## 捐赠

如果这个类库有帮助到您，请 Star 这个仓库。

你也可以选择使用支付宝或微信给我捐赠：

![Alipay, WeChat](https://toolgood.github.io/image/toolgood.png)

