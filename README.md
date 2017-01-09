ToolGood.Algorithm
===================
ToolGood.Algorithm支持`四则运算`、`Excel函数`，并支持`自定义参数`。

## 快速上手
``` csharp
    AlgorithmEngine engine = new AlgorithmEngine();
    double t=0.0;
    if (engine.Parse("1+2")) {
        t = (double)engine.Evaluate();
    }
    var c = engine.TryEvaluate("2+3", 0);
```
支持常量`pi`,`e`,`true`,`false`

## 自定义参数
``` csharp
    //定义圆柱信息
    public class Cylinder : AlgorithmEngine
    {
        private int _radius;
        private int _height;
        public Cylinder(int radius, int height)
        {
            _radius = radius;
            _height = height;
        }

        protected override Operand GetParameter(Operand curOpd)
        {
            if (curOpd.Parameter == "[半径]") {
                return new Operand(OperandType.NUMBER, _radius);
            }
            if (curOpd.Parameter == "[直径]") {
                return new Operand(OperandType.NUMBER, _radius * 2);
            }
            if (curOpd.Parameter == "[高]") {
                return new Operand(OperandType.NUMBER, _height);
            }
            return base.GetParameter(curOpd);
        }
    }
    //调用方法
    Cylinder c = new Cylinder(3, 10);
    c.TryEvaluate("[半径]*[半径]*pi()", 0.0);      //圆底面积
    c.TryEvaluate("[直径]*pi()", 0.0);            //圆的长
    c.TryEvaluate("[半径]*[半径]*pi()*[高]", 0.0); //圆的体积
```


## Excel函数
函数：`逻辑函数`、`数学与三角函数`、`文本函数`、`统计函数`、`日期与时间函数`

#### 逻辑函数
<table>
    <tr><td>公式名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>IF</td>
        <td>执行真假值判断，根据逻辑计算的真假值，返回不同结果。</td>
        <td>if(1=1,1,2) >>1</td>
    </tr>
    <tr>
        <td>IFERROR</td>
        <td>如果公式计算出错误则返回您指定的值；否则返回公式结果。</td>
        <td>iferror(1/0,1,2) >>1</td>
    </tr>
    <tr>
        <td>IFNUMBER</td>
        <td>指定要执行的逻辑检测</td>
        <td>ifnumber(4,1,2) >>1</td>
    </tr>
    <tr>
        <td>IFTEXT</td>
        <td>指定要执行的逻辑检测</td>
        <td>iftext('a',1,2) >>1</td>
    </tr>
    <tr>
        <td>ISNUMBER</td>
        <td>判断是否数字</td>
        <td>ISNUMBER(1) >>true</td>
    </tr>
    <tr>
        <td>ISTEXT</td>
        <td>判断是否文字</td>
        <td>istext('1') >>true </td>
    </tr>
    <tr>
        <td>AND</td>
        <td>如果所有参数均为TRUE，则返回TRUE</td>
        <td>and(1,2=2) >>true</td>
    </tr>
    <tr>
        <td>OR</td>
        <td>如果任一参数为TRUE，则返回TRUE</td>
        <td>or(1,2=3) >>true</td>
    </tr>
    <tr>
        <td>NOT</td>
        <td>对参数的逻辑值求反</td>
        <td>NOT(true()) >>false</td>
    </tr>
    <tr>
        <td>TRUE</td>
        <td>返回逻辑值TRUE</td>
        <td>true() >>true</td>
    </tr>
    <tr>
        <td>FALSE</td>
        <td>返回逻辑值FALSE</td>
        <td>false() >>false</td>
    </tr>
</table>

#### 数学与三角函数
<table>
    <tr><td>分类</td><td>公式名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td rowspan="12">基础数学</td>
        <td>PI</td><td>返回 PI 值</td>
        <td>pi() >>3.141592654</td>
    </tr>
    <tr>
        <td>abs</td><td>abs(数字)<br>返回数字的绝对值</td>
        <td>abs(-1) >>1</td>
    </tr>
    <tr>
        <td>QUOTIENT</td><td>quotient(除数,被除数)<br>返回商的整数部分，该函数可用于舍掉商的小数部分。</td>
        <td>QUOTIENT(7,3) >>2</td>
    </tr>
    <tr>
        <td>mod</td><td>mod(除数,被除数)<br>返回两数相除的余数</td>
        <td>MOD(7,3) >>1</td>
    </tr>
    <tr>
        <td>SIGN</td><td>sign(数字)<br>返回数字的符号。当数字为正数时返回 1，为零时返回 0，为负数时返回 -1。</td>
        <td>SIGN(-9) >>-1</td>
    </tr>
   <tr>
        <td>SQRT</td><td>sqrt(数字)<br>返回正平方根</td>
        <td>SQRT(9) >>3</td>
    </tr>
    <tr>
        <td>TRUNC</td><td>trunc(数字)<br>将数字截尾取整</td>
        <td>TRUNC(9.222) >>9  TRUNC(-9.222) >>-9 </td>
    </tr>
    <tr>
        <td>int</td><td>int(数字)<br>将数字向下舍入到最接近的整数。</td>
        <td>int(9.222) >>9   int(-9.222) >>-10</td>
    </tr>
    <tr>
        <td>gcd</td><td>gcd(数字1,数字2,...)<br>返回最大公约数</td>
        <td>GCD(3,5,7) >>1</td>
    </tr>
    <tr>
        <td>LCM</td><td>lcm(数字1,数字2,...)<br>返回整数参数的最小公倍数</td>
        <td>LCM(3,5,7) >>105</td>
    </tr>
    <tr>
        <td>combin</td><td>combin(总数,排列数)<br>计算从给定数目的对象集合中提取若干对象的组合数</td>
        <td>combin(10,2) >>45</td>
    </tr>
    <tr>
        <td>PERMUT</td><td>permut(总数,排列数)<br>返回从给定数目的对象集合中选取的若干对象的排列数</td>
        <td>PERMUT(10,2) >>990</td>
    </tr>
    <tr>
    <td rowspan="15">三角函数</td>
        <td>degrees</td><td>将弧度转换为度</td>
        <td>degrees(pi()) >>180</td>
    </tr>
    <tr>
        <td>RADIANS</td><td>将度转换为弧度</td>
        <td>RADIANS(180) >>3.141592654</td>
    </tr>
    <tr>
        <td>cos</td><td>返回数字的余弦值</td>
        <td>cos(1) >>0.54030230586813977</td>
    </tr>
    <tr>
        <td>cosh</td><td>返回数字的双曲余弦值</td>
        <td>cosh(1) >>1.5430806348152437</td>
    </tr>
    <tr>
        <td>SIN</td><td>返回给定角度的正弦值</td>
        <td>sin(1) >>0.8414709848078965</td>
    </tr>
    <tr>
        <td>SINH</td><td>返回数字的双曲正弦值</td>
        <td>sinh(1) >>1.1752011936438014</td>
    </tr>
    <tr>
        <td>TAN</td><td>返回数字的正切值</td>
        <td>tan(1) >>1.5574077246549023</td>
    </tr>
    <tr>
        <td>TANH</td><td>返回数字的双曲正切值</td>
        <td>tanh(1) >>0.76159415595576485</td>
    </tr>
    <tr>
        <td>acos</td><td>返回数字的反余弦值</td>
        <td>acos(0.5) >>1.0471975511965979</td>
    </tr>
    <tr>
        <td>acosh</td><td>返回数字的反双曲余弦值</td>
        <td>acosh(1.5) >>0.96242365011920694</td>
    </tr>
    <tr>
        <td>asin</td><td>返回数字的反正弦值</td>
        <td>asin(0.5) >>0.52359877559829893</td>
    </tr>
    <tr>
        <td>asinh</td><td>返回数字的反双曲正弦值。</td>
        <td>asinh(1.5) >>1.1947632172871094</td>
    </tr>
    <tr>
        <td>atan</td><td>返回数字的反正切值</td>
        <td>atan(1) >>0.78539816339744828</td>
    </tr>
   <tr>
        <td>atanh</td><td>返回参数的反双曲正切值</td>
        <td>atanh(1) >>0.54930614433405489</td>
    </tr>
    <tr>
        <td>atan2</td><td>从X和Y坐标返回反正切</td>
        <td>atan2(1,2) >>1.1071487177940904</td>
    </tr>
    <tr>
        <td rowspan="8">四舍五入</td>
        <td>ROUND</td><td>返回某个数字按指定位数取整后的数字。</td>
        <td>ROUND(4.333,2) >>4.33</td>
    </tr>
    <tr>
        <td>ROUNDDOWN</td><td>靠近零值，向下（绝对值减小的方向）舍入数字。</td>
        <td>ROUNDDOWN(4.333,2) >>4.33</td>
    </tr>
    <tr>
        <td>ROUNDUP</td><td>远离零值，向上（绝对值增长的方向）舍入数字。</td>
        <td>ROUNDUP(4.333,2) >>4.34</td>
    </tr>
    <tr>
        <td>CEILING</td><td>向上舍入（沿绝对值增大的方向）为最接近的 舍入基数 的倍数。</td>
        <td>CEILING(4.333,0.1) >>4.4</td>
    </tr>
    <tr>
        <td>floor</td><td>向下舍入，使其等于最接近的 Significance 的倍数。</td>
        <td>FLOOR(4.333,0.1) >>4.3</td>
    </tr>
    <tr>
        <td>even</td><td>返回沿绝对值增大方向取整后最接近的偶数。</td>
        <td>EVEN(3) >>4</td>
    </tr>
    <tr>
        <td>ODD</td><td>将数字向上舍入为最接近的奇型整数</td>
        <td>ODD(3.1) >>5</td>
    </tr>
    <tr>
        <td>MROUND</td><td>返回一个舍入到所需倍数的数字</td>
        <td>MROUND(13,5) >>15</td>
    </tr>
    <tr>
        <td rowspan="2">随机数</td>
        <td>RAND</td><td>返回 0 到 1 之间的随机数 </td>
        <td>RAND() >>0.2</td>
    </tr>
    <tr>
        <td>RANDBETWEEN</td><td>返回大于等于指定的最小值，小于指定最大值之间的一个随机整数。</td>
        <td>RANDBETWEEN(2,44) >>9</td>
    </tr>
    <tr>
        <td rowspan="11">幂/对数/阶乘</td>
        <td>fact</td><td>返回数的阶乘，一个数的阶乘等于 1*2*3*…* 该数。</td>
        <td>FACT(3) >>6</td>
    </tr>
    <tr>
        <td>factdouble</td><td>返回数字的双倍阶乘</td>
        <td>FACTDOUBLE(10) >>3840</td>
    </tr>
    <tr>
        <td>POWER</td><td>返回数的乘幂结果</td>
        <td>POWER(10,2) >>100</td>
    </tr>
    <tr>
        <td>exp</td><td>返回e的指定数乘幂</td>
        <td>exp(2) >>7.389056099</td>
    </tr>
    <tr>
        <td>ln</td><td>返回数字的自然对数</td>
        <td>LN(4) >>1.386294361</td>
    </tr>
    <tr>
        <td>log</td><td>返回数字的常用对数</td>
        <td>LOG(100,10) >>2</td>
    </tr>
    <tr>
        <td>LOG10</td><td>返回数字的10对数</td>
        <td>LOG10(100) >>2</td>
    </tr>
    <tr>
        <td>MULTINOMIAL</td><td>返回参数和的阶乘与各参数阶乘乘积的比值</td>
        <td>MULTINOMIAL(1,2,3) >>60</td>
    </tr>
    <tr>
        <td>PRODUCT</td><td>将所有以参数形式给出的数字相乘，并返回乘积值。</td>
        <td>PRODUCT(1,2,3,4) >>24</td>
    </tr>
    <tr>
        <td>SQRTPI</td><td>返回某数与 PI 的乘积的平方根</td>
        <td>SQRTPI(3) >>3.069980124</td>
    </tr>
    <tr>
        <td>SUMSQ</td><td>返回参数的平方和</td>
        <td>SUMSQ(1,2) >>5</td>
    </tr>
</table>

#### 文本函数
<table>
    <tr><td>公式名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>ASC</td><td>将字符串内的全角英文字母更改为半角字符</td>
        <td>asc('ａｂｃＡＢＣ１２３') >>abcABC123</td>
    </tr>
    <tr>
        <td>JIS / WIDECHAR</td><td>将字符串中的半角英文字符更改为全角字符</td>
        <td>jis('abcABC123') >>ａｂｃＡＢＣ１２３</td>
    </tr>
    <tr>
        <td>CHAR</td><td>返回由代码数字指定的字符</td>
        <td>char(49) >>1</td>
    </tr>
    <tr>
        <td>CLEAN</td><td>删除文本中所有打印不出的字符</td>
        <td>clean('\r112\t') >>112</td>
    </tr>
    <tr>
        <td>CODE</td><td>返回文本字符串中第一个字符的数字代码</td>
        <td>CODE("1") >>49</td>
    </tr>
    <tr>
        <td>CONCATENATE</td><td>将若干文本项合并到一个文本项中</td>
        <td>CONCATENATE('tt','11') >>tt11</td>
    </tr>
    <tr>
        <td>EXACT</td><td>检查两个文本值是否完全相同</td>
        <td>EXACT("11","22") >>false</td>
    </tr>
    <tr>
        <td>FIND</td><td>在一文本值内查找另一文本值（区分大小写） </td>
        <td>FIND("11","12221122") >>5</td>
    </tr>
    <tr>
        <td>FIXED</td><td>将数字设置为具有固定小数位的文本格式</td>
        <td>FIXED(4567.89,1) >>4,567.9</td>
    </tr>
    <tr>
        <td>LEFT</td><td>返回文本值最左边的字符</td>
        <td>LEFT('123222',3) >>123</td>
    </tr>
    <tr>
        <td>LEN</td><td>返回文本字符串中的字符个数</td>
        <td>LEN('123222') >>6</td>
    </tr>
    <tr>
        <td>LOWER</td><td>将文本转换为小写形式</td>
        <td>LOWER('ABC') >>abc</td>
    </tr>
    <tr>
        <td>MID</td><td>从文本字符串中的指定位置起返回特定个数的字符</td>
        <td>MID('ABCDEF',2,3) >>BCD</td>
    </tr>
    <tr>
        <td>PROPER</td><td>将文本值中每一个单词的首字母设置为大写</td>
        <td>PROPER('abc abc') >>Abc Abc</td>
    </tr>
    <tr>
        <td>REPLACE</td><td>替换文本内的字符</td>
        <td>REPLACE("abccd",2,3,"2") >>a2d</td>
    </tr>
    <tr>
        <td>REPT</td><td>按给定次数重复文本</td>
        <td>REPT("q",3) >>qqq</td>
    </tr>
    <tr>
        <td>RIGHT</td><td>返回文本值最右边的字符</td>
        <td>RIGHT("123q",3) >>23q</td>
    </tr>
    <tr>
        <td>RMB</td><td>将数字转换为大写数字文本</td>
        <td>rmb(12.3) >>壹拾贰元叁角</td>
    </tr>
    <tr>
        <td>SEARCH</td><td>在一文本值中查找另一文本值（不区分大小写）</td>
        <td>SEARCH("aa","abbAaddd") >>4</td>
    </tr>
    <tr>
        <td>SUBSTITUTE</td><td>在文本字符串中以新文本替换旧文本</td>
        <td>SUBSTITUTE("ababcc","ab","12") >>1212cc</td>
    </tr>
    <tr>
        <td>T</td><td>将参数转换为文本</td>
        <td>T('123') >>123</td>
    </tr>
    <tr>
        <td>TEXT</td><td>设置数字的格式并将数字转换为文本</td>
        <td>TEXT(123,"0.00") >>123.00</td>
    </tr>
    <tr>
        <td>TRIM</td><td>删除文本中的空格</td>
        <td>TRIM(" 123 123 ")>>123 123</td>
    </tr>
    <tr>
        <td>UPPER</td><td>将文本转换为大写形式</td>
        <td>UPPER("abc") >>ABC</td>
    </tr>
    <tr>
        <td>VALUE</td><td>将文本参数转换为数字</td>
        <td>VALUE("123") >>123</td>
    </tr>
</table>

#### 日期与时间函数
<table>
    <tr><td>公式名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>DATEVALUE</td><td>将文本格式的日期转换为序列号</td>
        <td>DATEVALUE("2017-01-02") >>2017-01-02</td>
    </tr>
    <tr>
        <td>TIMEVALUE</td><td>将文本格式的时间转换为序列号</td>
        <td>TIMEVALUE("12:12:12") >>12:12:12</td>
    </tr>
    <tr>
        <td>DATE</td><td>返回特定日期的序列号</td>
        <td>DATE(2016,1,1) >>2016-01-01</td>
    </tr>
    <tr>
        <td>TIME</td><td>返回特定时间的序列号</td>
        <td>TIME(12,13,14) >>12:13:14</td>
    </tr>
    <tr>
        <td>NOW</td><td>返回当前日期和时间的序列号</td>
        <td>NOW() >>2017-01-07 11:00:00</td>
    </tr>
    <tr>
        <td>TODAY</td><td>返回今天日期的序列号</td>
        <td>TODAY() >>2017-01-07</td>
    </tr>
    <tr>
        <td>YEAR</td><td>将序列号转换为年</td>
        <td>YEAR(NOW()) >>2017</td>
    </tr>
    <tr>
        <td>MONTH</td><td>将序列号转换为月</td>
        <td>MONTH(NOW()) >>1</td>
    </tr>
    <tr>
        <td>DAY</td><td>将序列号转换为月份中的日</td>
        <td>DAY(NOW()) >>7</td>
    </tr>
    <tr>
        <td>HOUR</td><td>将序列号转换为小时</td>
        <td>HOUR(NOW()) >>11</td>
    </tr>
    <tr>
        <td>MINUTE</td><td>将序列号转换为分钟</td>
        <td>MINUTE(NOW()) >>12</td>
    </tr>
    <tr>
        <td>SECOND</td><td>将序列号转换为秒</td>
        <td>SECOND(NOW()) >>34</td>
    </tr>
    <tr>
        <td>WEEKDAY</td><td>将序列号转换为星期几</td>
        <td>WEEKDAY(date(2017,1,7)) >>7</td>
    </tr>
    <tr>
        <td>DATEDIF</td><td>返回两个日期之间的相隔天数</td>
        <td>DATEDIF("1975-1-30","2017-1-7","Y") >>41</td>
    </tr>
    <tr>
        <td>DAYS360</td><td>以一年 360 天为基准计算两个日期间的天数</td>
        <td>DAYS360('1975-1-30','2017-1-7') >>15097</td>
    </tr>
    <tr>
        <td>EDATE</td><td>返回用于表示开始日期之前或之后月数的日期的序列号</td>
        <td>EDATE("2012-1-31",32) >>2014-09-30</td>
    </tr>
    <tr>
        <td>EOMONTH</td><td>返回指定月数之前或之后的月份的最后一天的序列号</td>
        <td>EOMONTH("2012-2-1",32) >>2014-10-31</td>
    </tr>
    <tr>
        <td>NETWORKDAYS</td><td>返回两个日期之间的全部工作日数</td>
        <td>NETWORKDAYS("2012-1-1","2013-1-1") >>262</td>
    </tr>
    <tr>
        <td>WORKDAY</td><td>返回指定的若干个工作日之前或之后的日期的序列号</td>
        <td>WORKDAY("2012-1-2",145) >>2012-07-23</td>
    </tr>
    <tr>
        <td>WEEKNUM</td><td>将序列号转换为一年中相应的周数</td>
        <td>WEEKNUM("2016-1-3") >>2</td>
    </tr>
</table>

#### 统计函数
<table>
    <tr><td>公式名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>MAX</td><td>返回参数列表中的最大值</td>
        <td>max(1,2,3,4,2,2,1,4) >>4</td>
    </tr>
    <tr>
        <td>MEDIAN</td><td>返回给定数字的中值</td>
        <td>MEDIAN(1,2,3,4,2,2,1,4) >>2</td>
    </tr>
    <tr>
        <td>MIN</td><td>返回参数列表中的最小值</td>
        <td>MIN(1,2,3,4,2,2,1,4) >>1</td>
    </tr>
    <tr>
        <td>QUARTILE</td><td>返回数据集的四分位数</td>
        <td>QUARTILE({1,2,3,4,2,2,1,4},0) >>1</td>
    </tr>
    <tr>
        <td>MODE</td><td>返回数据集中出现最多的值间的概率</td>
        <td>MODE(1,2,3,4,2,2,1,4) >>2</td>
    </tr>
    <tr>
        <td>LARGE</td><td>返回数据集中第 k 个最大值</td>
        <td>LARGE({1,2,3,4,2,2,1,4},3) >>3</td>
    </tr>
    <tr>
        <td>SMALL</td><td>返回数据集中第 k 个最小值</td>
        <td>SMALL({1,2,3,4,2,2,1,4},3) >>2</td>
    </tr>
    <tr>
        <td>PERCENTILE</td><td>返回区域中的第 k 个百分位值</td>
        <td>PERCENTILE({1,2,3,4,2,2,1,4}，0.4) >>2</td>
    </tr>
    <tr>
        <td>PERCENTRANK</td><td>返回数据集中值的百分比排位</td>
        <td>PERCENTRANK({1,2,3,4,2,2,1,4}，3) >>0.714</td>
    </tr>
    <tr>
        <td>AVERAGE</td><td>返回参数的平均值</td>
        <td>AVERAGE(1,2,3,4,2,2,1,4) >>2.375</td>
    </tr>
    <tr>
        <td>AVERAGEIF</td><td>返回参数的平均值</td>
        <td>AVERAGEIF({1,2,3,4,2,2,1,4},'>1') >>2.833333333</td>
    </tr>
    <tr>
        <td>GEOMEAN</td><td>返回正数数组或区域的几何平均值</td>
        <td>GEOMEAN(1,2,3,4) >>2.213363839</td>
    </tr>
    <tr>
        <td>HARMEAN</td><td>返回数据集合的调和平均值</td>
        <td>HARMEAN(1,2,3,4) >>1.92</td>
    </tr>
    <tr>
        <td>COUNT</td><td>计算参数列表中数字的个数</td>
        <td>COUNT(1,2,3,4,2,2,1,4) >>8</td>
    </tr>
    <tr>
        <td>COUNTIF</td><td>计算参数列表中数字的个数</td>
        <td>COUNTIF({1,2,3,4,2,2,1,4},'>1') >>6</td>
    </tr>
    <tr>
        <td>SUM</td><td>返回所有数字之和。</td>
        <td>SUM(1,2,3,4) >>10</td>
    </tr>
    <tr>
        <td>SUMIF</td><td>返回所有数字之和</td>
        <td>SUMIF({1,2,3,4,2,2,1,4},'>1') >>17</td>
    </tr>
    <tr>
        <td>AVEDEV</td><td>返回数据点与其平均值的绝对偏差的平均值</td>
        <td>AVEDEV(1,2,3,4,2,2,1,4) >>0.96875</td>
    </tr>
    <tr>
        <td>STDEV</td><td>基于样本估算标准偏差</td>
        <td>STDEV(1,2,3,4,2,2,1,4) >>1.1877349391654208</td>
    </tr>
    <tr>
        <td>STDEVP</td><td>计算基于整个样本总体的标准偏差</td>
        <td>STDEVP(1,2,3,4,2,2,1,4) >>1.1110243021644486</td>
    </tr>
    <tr>
        <td>DEVSQ</td><td>返回偏差的平方和</td>
        <td>DEVSQ(1,2,3,4,2,2,1,4) >>9.875</td>
    </tr>
    <tr>
        <td>VAR</td><td>基于样本估算方差</td>
        <td>VAR(1,2,3,4,2,2,1,4) >>1.4107142857142858</td>
    </tr>
    <tr>
        <td>VARP</td><td>基于整个样本总体计算方差</td>
        <td>VARP(1,2,3,4,2,2,1,4) >>1.234375</td>
    </tr>
    <tr>
        <td>NormSDist</td><td>返回标准正态累积分布函数，该分布的平均值为 0，标准偏差为 1。</td>
        <td>NORMSDIST(1) >>0.841344746</td>
    </tr>
    <tr>
        <td>NORMDIST</td><td>返回正态累积分布</td>
        <td>NORMDIST(3,8,4,1) >>0.105649774</td>
    </tr>
    <tr>
        <td>NORMINV</td><td>返回反正态累积分布</td>
        <td>NORMINV(0.8,8,3) >>10.5248637</td>
    </tr>
    <tr>
        <td>NORMSINV</td><td>返回反标准正态累积分布</td>
        <td>NORMSINV(0.3) >>-0.524400513</td>
    </tr>
    <tr>
        <td>BETADIST</td><td>返回 Beta 累积分布函数，不支持上下界</td>
        <td>BETADIST(0.5,11,22) >>0.97494877</td>
    </tr>
    <tr>
        <td>BETAINV</td><td>返回指定 Beta 分布的累积分布函数的反函数,不支持上下界</td>
        <td>BETAINV(0.5,23,45) >>0.336640759</td>
    </tr>
    <tr>
        <td>BINOMDIST</td><td>返回一元二项式分布概率</td>
        <td>BINOMDIST(12,45,0.5,0) >>0.000817409</td>
    </tr>
    <tr>
        <td>EXPONDIST</td><td>返回指数分布</td>
        <td>EXPONDIST(3,1,0) >>0.049787068</td>
    </tr>
    <tr>
        <td>FDIST</td><td>返回 F 概率分布</td>
        <td>FDIST(0.4,2,3) >>0.701465776</td>
    </tr>
    <tr>
        <td>FINV</td><td>返回 F 概率分布的反函数</td>
        <td>FINV(0.7,2,3) >>0.402651432</td>
    </tr>
    <tr>
        <td>FISHER</td><td>返回点 x 的 Fisher 变换。该变换生成一个正态分布而非偏斜的函数</td>
        <td>FISHER(0.68) >>0.8291140383</td>
    </tr>
    <tr>
        <td>FISHERINV</td><td>返回 Fisher 变换的反函数值。</td>
        <td>FISHERINV(0.6) >>0.537049567</td>
    </tr>
    <tr>
        <td>GAMMADIST</td><td>返回 γ 分布</td>
        <td>GAMMADIST(0.5,3,4,0) >>0.001723627</td>
    </tr>
    <tr>
        <td>GAMMAINV</td><td>返回 γ 累积分布函数的反函数</td>
        <td>GAMMAINV(0.2,3,4) >>6.140176811</td>
    </tr>
    <tr>
        <td>GAMMALN</td><td>返回 γ 累积分布函数的反函数</td>
        <td>GAMMALN(4) >>1.791759469</td>
    </tr>
    <tr>
        <td>HYPGEOMDIST</td><td>返回超几何分布</td>
        <td>HYPGEOMDIST(23,45,45,100) >>0.08715016</td>
    </tr>
    <tr>
        <td>LOGINV</td><td>返回 x 的对数累积分布函数的反函数</td>
        <td>LOGINV(0.1,45,33) >>15.01122624</td>
    </tr>
    <tr>
        <td>LognormDist</td><td>返回反对数正态分布</td>
        <td>LOGNORMDIST(15,23,45) >>0.326019201</td>
    </tr>
    <tr>
        <td>NEGBINOMDIST</td><td>返回负二项式分布</td>
        <td>NEGBINOMDIST(23,45,0.7) >>0.053463314</td>
    </tr>
    <tr>
        <td>POISSON</td><td>返回 Poisson 分布</td>
        <td>POISSON(23,23,0) >>0.082884384</td>
    </tr>
    <tr>
        <td>TDIST</td><td>返回学生的 t 分布</td>
        <td>TDIST(1.2,24,1) >>0.120925677</td>
    </tr>
    <tr>
        <td>TINV</td><td>返回学生的 t 分布的反分布</td>
        <td>TINV(0.12,23) >>1.614756561</td>
    </tr>
    <tr>
        <td>WEIBULL</td><td>返回 Weibull 分布</td>
        <td>WEIBULL(1,2,3,1) >>0.105160683</td>
    </tr>

</table>


