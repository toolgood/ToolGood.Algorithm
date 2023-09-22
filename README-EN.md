ToolGood.Algorithm
===================
[中文文档](README.md)

ToolGood.Algorithm is a powerful, lightweight, `Excel formula` compatible algorithm library aimed at improving developers’ productivity in different business scenes. 


**Applicable scenarios:** Code and algorithm are separated to avoid forced project upgrade 

1）Uncertain algorithm at the beginning of the project; 

2）Algorithms that are frequently changed during project maintenance; 

3）Algorithms in financial data and statistical data (Note: Some formulas use the `double` type, and it is recommended to use `fen` as the unit); 

4）The report is exported, the data source uses the stored procedure, and the algorithm is set in the Word document. Example https://github.com/toolgood/ToolGood.WordTemplate 

5）Rule engines, such as: https://github.com/toolgood/ToolGood.FlowVision


## Quick start
``` csharp
    AlgorithmEngine engine = new AlgorithmEngine();
    double a=0.0;
    if (engine.Parse("1+2")) {
        var o = engine.Evaluate();
        a=o.NumberValue;
    }
    var b = engine.TryEvaluate("1=1 && 1<2 and 7-8>1", 0);// Support(支持) && || and or 
    var c = engine.TryEvaluate("2+3", 0);
    var q = engine.TryEvaluate("-7 < -2 ?1 : 2", 0);
    var e = engine.TryEvaluate("count(array(1, 2, 3, 4))", 0);//{} represents array, return: 4 {}代表数组, 返回:4
    var s = engine.TryEvaluate("'aa'&'bb'", ""); //String connection, return: AABB 字符串连接, 返回:aabb
    var r = engine.TryEvaluate("(1=1)*9+2", 0); //Return: 11 返回:11
    var d = engine.TryEvaluate("'2016-1-1'+1", DateTime.MinValue); //Return date: 2016-1-2 返回日期:2016-1-2
    var t = engine.TryEvaluate("'2016-1-1'+9*'1:0'", DateTime.MinValue);//Return datetime:2016-1-1 9:0  返回日期:2016-1-1 9:0
    var j = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\", \"Age\":51, \"Birthday\":\"04/26/1564 00:00:00\"}').Age", null);//Return 51 返回51
    var k = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare   \", \"Age\":51, \"Birthday\":\"04/26/1564 00:00:00\"}')[Name].Trim()", null);//Return to "William Shakespeare"  返回"William Shakespeare" (不带空格)
    var l = engine.TryEvaluate("json('{\"Name1\":\"William Shakespeare \", \"Age\":51, \"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2, 3)", null); ;//Return "ill"  返回"ill"

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
``` csharp
    //Define cylinder information  定义圆柱信息 
    public class Cylinder : AlgorithmEngine
    {
        private int _radius;
        private int _height;
        public Cylinder(int radius, int height)
        {
            _radius = radius;
            _height = height;
        }

        protected override Operand GetParameter(string parameter)
        {
            if (parameter == "半径")
            {
                return Operand.Create(_radius);
            }
            if (parameter == "直径")
            {
                return Operand.Create(_radius * 2);
            }
            if (parameter == "高")
            {
                return Operand.Create(_height);
            }
            return base.GetParameter(parameter);
        }
    }
    //Call method  调用方法
    Cylinder c = new Cylinder(3, 10);
    c.TryEvaluate("[半径]*[半径]*pi()", 0.0);      //Round bottom area  圆底面积
    c.TryEvaluate("[直径]*pi()", 0.0);            //The length of the circle  圆的长
    c.TryEvaluate("[半径]*[半径]*pi()*[高]", 0.0); //Volume of circle 圆的体积
    c.TryEvaluate("['半径']*[半径]*pi()*[高]", 0.0); //Volume of circle 圆的体积
    c.EvaluateFormula("'圆'-[半径]-高", '-'); // Return: 圆-3-10
    c.GetSimplifiedFormula("半径*if(半径>2, 1+4, 3)"); // Return: 3 * 5
```
Parameters are defined in square brackets, such as `[parameter name]`. 

Note: You can also use `AddParameter`, `AddParameterFromJson` to add methods, and use `DiyFunction`+= to customize functions. 

Note 2: use `AlgorithmEngineHelper.GetDiyNames` get `parameter name` and `custom function name`.


## Custom parameters
``` csharp
    var helper = new ToolGood.Algorithm.AlgorithmEngineHelper();
    helper.IsKeywords("false"); // return true
    helper.IsKeywords("true"); // return true
    helper.IsKeywords("mysql"); // return false

    DiyNameInfo p5 = helper.GetDiyNames("ddd(d1, 22)");
    Assert.AreEqual("ddd", p5.Functions[0]);
    Assert.AreEqual("d1", p5.Parameters[0]);

```


## Excel Formula

Functions: `logical functions`, `mathematics and trigonometric functions`, `text functions`, `statistical functions`, `date and time functions` 

Note: Function names are not case sensitive. Parameters with square brackets can be omitted. The return value of the example is approximate. 

Note 2: The function name with ★ indicates that the first parameter can be prefixed, such as `(-1).ISTEXT()` 

Note 3: The function name with ▲ means that it is affected by `Excel Index`, 

#### Logical function
<table>
    <tr><td>function name</td><td>description</td><td>Example</td></tr>
    <tr>
        <td>IF</td><td>if(condition, trueValue[, falseValue])<br>Execute the judgment and return different results according to the true or false of the logical calculation.</td>
        <td>if(1=1, 1, 2) <br>>>1</td>
    </tr>
    <tr>
        <td>ifError</td><td>ifError(condition, trueValue[, falseValue])<br>If the formula calculates incorrectly, the value you specify is returned; otherwise, the formula result is returned.</td>
        <td>ifError(1/0, 1, 2) <br>>>1</td>
    </tr>
    <tr>
        <td>isError ★</td>
        <td>
        isError(value)<br>To determine whether there is an error, return TRUE or FALSE
        isError(value, replace)<br>If there is an error, return the replacement value, otherwise return the original value
        </td>
        <td>isError(1) <br>>>false</td>
    </tr>
    <tr>
        <td>isNull ★</td>
        <td>
        isNull(value)<br>Determine whether it is null or return TRUE or FALSE
        isNull(value, replace)<br>If null, return the replacement value, otherwise return the original value
        </td>
        <td>isNull(null) <br>>>true</td>
    </tr>
    <tr>
        <td>isNullOrError ★</td>
        <td>
        isNullOrError(value)<br>To determine whether it is null or error, return TRUE or FALSE
        isNullOrError(value, replace)<br>If it is null or wrong, return the replacement value, otherwise return the original value
        </td>
        <td>isNullOrError(null) <br>>>true</td>
    </tr>
    <tr>
        <td>isNumber ★</td><td>isNumber(value)<br>Determine whether it is a numeric value, and return TRUE or FALSE</td>
        <td>isNumber(1) <br>>>true</td>
    </tr>
    <tr>
        <td>isText ★</td><td>isText(value)<br>Determine whether it is a text and return TRUE or FALSE</td>
        <td>isText('1') <br>>>true </td>
    </tr>
    <tr>
        <td>IsNonText ★</td><td>IsNonText(value)<br>Determine whether it is not a text and return TRUE or FALSE</td>
        <td>IsNonText('1') <br>>>false </td>
    </tr>
    <tr>
        <td>IsLogical ★</td><td>IsLogical(value)<br>Determine whether it is a logical value and return TRUE or FALSE</td>
        <td>IsLogical('1') <br>>>false </td>
    </tr>
    <tr>
        <td>IsEven ★</td><td>IsEven(value)<br>If the value is even, return TRUE or FALSE</td>
        <td>IsEven('1') <br>>>false </td>
    </tr>
    <tr>
        <td>IsOdd ★</td><td>IsOdd(value)<br>If the value is odd, return TRUE or FALSE</td>
        <td>IsOdd('1') <br>>>true </td>
    </tr>
    <tr>
        <td>AND</td><td>and(logic1, ...)<br>If all parameters are true, return true. If there is an error, report it first</td>
        <td>and(1, 2=2) <br>>>true</td>
    </tr>
    <tr>
        <td>OR</td><td>or(logic1, ...)<br>If any parameter is TRUE, return TRUE. If there is an error, report it first</td>
        <td>or(1, 2=3) <br>>>true</td>
    </tr>
    <tr>
        <td>NOT</td><td>not(logic)<br>Negate the logical value of a parameter</td>
        <td>NOT(true()) <br>>>false</td>
    </tr>
    <tr>
        <td>TRUE</td><td>true()<br>Return TRUE</td>
        <td>true() <br>>>true</td>
    </tr>
    <tr>
        <td>FALSE</td><td>false()<br>Return FALSE</td>
        <td>false() <br>>>false</td>
    </tr>
</table>

#### Mathematics and Trigonometric Functions
<table>
    <tr><td>classification</td><td>function name</td><td>description</td><td>Example</td></tr>
    <tr>
        <td rowspan="14">basic mathematics</td>
        <td>E</td><td>e()<br>Return e value</td>
        <td>E() <br>>> </td>
    </tr>
    <tr>
        <td>PI</td><td>pi()<br>Return PI value</td>
        <td>pi() <br>>>3.141592654</td>
    </tr>
    <tr>
        <td>abs</td><td>abs(number)<br>Returns the absolute value of a numerical value</td>
        <td>abs(-1) <br>>>1</td>
    </tr>
    <tr>
        <td>QUOTIENT</td><td>quotient(number, dividend)<br>Returns the integer portion of the quotient, which can be used to round off the fractional portion of the quotient.</td>
        <td>QUOTIENT(7, 3) <br>>>2</td>
    </tr>
    <tr>
        <td>mod</td><td>mod(number, dividend)<br>Returns the remainder of the division of two numbers</td>
        <td>MOD(7, 3) <br>>>1</td>
    </tr>
    <tr>
        <td>SIGN</td><td>sign(number)<br>Returns the sign of a numerical value. Returns 1 when the value is positive, 0 when it is zero, and -1 when it is negative.</td>
        <td>SIGN(-9) <br>>>-1</td>
    </tr>
   <tr>
        <td>SQRT</td><td>sqrt(number)<br>Returns the positive square root</td>
        <td>SQRT(9) <br>>>3</td>
    </tr>
    <tr>
        <td>TRUNC</td><td>trunc(number)<br>Truncate the value</td>
        <td>TRUNC(9.222) <br>>>9</td>
    </tr>
    <tr>
        <td>int ★</td><td>int(number)<br>Rounds the value down to the nearest integer.</td>
        <td>int(9.222) <br>>>9</td>
    </tr>
    <tr>
        <td>gcd</td><td>gcd(number, ...)<br>Returns the maximum common divisor</td>
        <td>GCD(3, 5, 7) <br>>>1</td>
    </tr>
    <tr>
        <td>LCM</td><td>lcm(number, ...)<br>Returns the least common multiple of an integer parameter</td>
        <td>LCM(3, 5, 7) <br>>>105</td>
    </tr>
    <tr>
        <td>combin</td><td>combin(tatal, number)<br>Calculate the number of combinations to extract several objects from a given number of object sets</td>
        <td>combin(10, 2) <br>>>45</td>
    </tr>
    <tr>
        <td>PERMUT</td><td>permut(tatal, number)<br>Returns the ranking of several objects selected from a given number of object collections</td>
        <td>PERMUT(10, 2) <br>>>990</td>
    </tr>
    <tr>
        <td>FIXED</td><td>fixed(number[, decimalDigit[, hasComma]])<br>Format numeric values to text with fixed decimal places</td>
        <td>FIXED(4567.89, 1) <br>>>4, 567.9</td>
    </tr>
    <tr>
    <td rowspan="15">Triangulation function</td>
        <td>degrees</td><td>degrees(radian)<br>Convert radians to degrees</td>
        <td>degrees(pi()) <br>>>180</td>
    </tr>
    <tr>
        <td>RADIANS</td><td>radians(degree)<br>Convert degrees to radians</td>
        <td>RADIANS(180) <br>>>3.141592654</td>
    </tr>
    <tr>
        <td>cos</td><td>cos(radian)<br>Returns the cosine of a numerical value</td>
        <td>cos(1) <br>>>0.540302305868</td>
    </tr>
    <tr>
        <td>cosh</td><td>cosh(radian)<br>Returns the hyperbolic cosine of a value</td>
        <td>cosh(1) <br>>>1.54308063481</td>
    </tr>
    <tr>
        <td>SIN</td><td>sin(radian)<br>Returns the sine of a given angle</td>
        <td>sin(1) <br>>>0.84147098480</td>
    </tr>
    <tr>
        <td>SINH</td><td>sinh(radian)<br>Returns the hyperbolic sine of a numeric value</td>
        <td>sinh(1) <br>>>1.1752011936</td>
    </tr>
    <tr>
        <td>TAN</td><td>tan(radian)<br>Returns the tangent of a numerical value</td>
        <td>tan(1) <br>>>1.55740772465</td>
    </tr>
    <tr>
        <td>TANH</td><td>tanh(radian)<br>Returns the hyperbolic tangent of a value</td>
        <td>tanh(1) <br>>>0.761594155955</td>
    </tr>
    <tr>
        <td>acos</td><td>acos(number)<br>Returns the inverse cosine of a numeric value</td>
        <td>acos(0.5) <br>>>1.04719755119</td>
    </tr>
    <tr>
        <td>acosh</td><td>acosh(number)<br>Returns the inverse hyperbolic cosine of a value</td>
        <td>acosh(1.5) <br>>>0.962423650119</td>
    </tr>
    <tr>
        <td>asin</td><td>asin(number)<br>Returns the arcsine of a value</td>
        <td>asin(0.5) <br>>>0.523598775598</td>
    </tr>
    <tr>
        <td>asinh</td><td>asinh(number)<br>Returns the inverse hyperbolic sine of a value.</td>
        <td>asinh(1.5) <br>>>1.1947632172</td>
    </tr>
    <tr>
        <td>atan</td><td>atan(number)<br>Returns the inverse tangent of a value</td>
        <td>atan(1) <br>>>0.785398163397</td>
    </tr>
   <tr>
        <td>atanh</td><td>atanh(number)<br>Returns the inverse hyperbolic tangent of the parameter</td>
        <td>atanh(1) <br>>>0.549306144334</td>
    </tr>
    <tr>
        <td>atan2</td><td>atan2(number, number)<br>Return anti-tangent from X and Y coordinates</td>
        <td>atan2(1, 2) <br>>>1.10714871779</td>
    </tr>
    <tr>
        <td rowspan="8">Round off</td>
        <td>ROUND</td><td>round(number, decimalDigit)<br>Returns the value of a value rounded by the specified number of digits.</td>
        <td>ROUND(4.333, 2) <br>>>4.33</td>
    </tr>
    <tr>
        <td>roundDown</td><td>roundDown(number, decimalDigit)<br>Close to zero, rounding the value down (the direction in which the absolute value decreases).</td>
        <td>roundDown(4.333, 2) <br>>>4.33</td>
    </tr>
    <tr>
        <td>roundUp</td><td>roundUp(number, decimalDigit)<br>Away from zero, round the value upward (the direction in which the absolute value increases).</td>
        <td>roundUp(4.333, 2) <br>>>4.34</td>
    </tr>
    <tr>
        <td>CEILING</td><td>ceiling(number, roundingBase)<br>Rounding up (in the direction in which the absolute value increases) is a multiple of the nearest rounding base.</td>
        <td>CEILING(4.333, 0.1) <br>>>4.4</td>
    </tr>
    <tr>
        <td>floor</td><td>floor(number, roundingBase)<br>Round down to a multiple of the nearest Significance.</td>
        <td>FLOOR(4.333, 0.1) <br>>>4.3</td>
    </tr>
    <tr>
        <td>even</td><td>even(number)<br>Returns the nearest even number rounded in the direction of increasing the absolute value.</td>
        <td>EVEN(3) <br>>>4</td>
    </tr>
    <tr>
        <td>ODD</td><td>odd(number)<br>Rounds the value up to the nearest odd integer</td>
        <td>ODD(3.1) <br>>>5</td>
    </tr>
    <tr>
        <td>MROUND</td><td>mround(number, roundingBase)<br>Returns a value rounded to the desired multiple</td>
        <td>MROUND(13, 5) <br>>>15</td>
    </tr>
    <tr>
        <td rowspan="2">Random number</td>
        <td>RAND</td><td>rand()<br>Returns a random number between 0 and 1 </td>
        <td>RAND() <br>>>0.2</td>
    </tr>
    <tr>
        <td>randBetween</td><td>randBetween(min, max)<br>Returns a random integer greater than or equal to the specified minimum value and less than the specified maximum value.</td>
        <td>randBetween(2, 44) <br>>>9</td>
    </tr>
    <tr>
        <td rowspan="11">Power / logarithm / factorial</td>
        <td>fact</td><td>fact(number)<br>Returns the factorial of a number, where the factorial of a number is equal to 1'2'3 *. * the number.</td>
        <td>FACT(3) <br>>>6</td>
    </tr>
    <tr>
        <td>factdouble</td><td>factDouble(number)<br>Return the double factorial of the value</td>
        <td>factDouble(10) <br>>>3840</td>
    </tr>
    <tr>
        <td>POWER</td><td>power(number, power)<br>The power result of the return number</td>
        <td>POWER(10, 2) <br>>>100</td>
    </tr>
    <tr>
        <td>exp</td><td>exp(power)<br>Returns the power of the specified number of e</td>
        <td>exp(2) <br>>>7.389056099</td>
    </tr>
    <tr>
        <td>ln</td><td>ln(number)<br>Returns the natural logarithm of the value</td>
        <td>LN(4) <br>>>1.386294361</td>
    </tr>
    <tr>
        <td>log</td><td>log(number[, baseNumber])<br>Returns the common logarithm of the value, such as omitting the base. The default is 10.</td>
        <td>LOG(100, 10) <br>>>2</td>
    </tr>
    <tr>
        <td>LOG10</td><td>log10(number)<br>Returns the 10 logarithm of the value</td>
        <td>LOG10(100) <br>>>2</td>
    </tr>
    <tr>
        <td>MULTINOMIAL</td><td>multinomial(number, ...)<br>Returns the ratio of the factorial of the sum of parameters to the factorial product of each parameter</td>
        <td>MULTINOMIAL(1, 2, 3) <br>>>60</td>
    </tr>
    <tr>
        <td>PRODUCT</td><td>product(number, ...)<br>Multiplies all values given as parameters and returns the product value.</td>
        <td>PRODUCT(1, 2, 3, 4) <br>>>24</td>
    </tr>
    <tr>
        <td>SqrtPi</td><td>SqrtPi(number)<br>Returns the square root of the product of a number and PI</td>
        <td>SqrtPi(3) <br>>>3.069980124</td>
    </tr>
    <tr>
        <td>SUMSQ</td><td>sumQq(number, ...)<br>Returns the sum of squares of parameters</td>
        <td>SUMSQ(1, 2) <br>>>5</td>
    </tr>
    <tr>
        <td rowspan="12">Transformation</td>
        <td>DEC2BIN ★</td><td>DEC2BIN(number[, digit])<br>Decimal to binary </td>
        <td>DEC2BIN(100) <br>>> </td>
    </tr>
    <tr>
        <td>DEC2OCT ★</td><td>DEC2OCT(number[, digit])<br>Decimal to octal </td>
        <td>DEC2OCT(100) <br>>> </td>
    </tr>
    <tr>
        <td>DEC2HEX ★</td><td>DEC2HEX(number[, digit])<br>Convert from decimal to hexadecimal </td>
        <td>DEC2HEX(100) <br>>> </td>
    </tr>
    <tr>
        <td>HEX2BIN ★</td><td>HEX2BIN(number[, digit])<br>Hexadecimal to binary </td>
        <td>HEX2BIN(100) <br>>> </td>
    </tr>
    <tr>
        <td>HEX2OCT ★</td><td>HEX2OCT(number[, digit])<br>Convert hexadecimal to octal </td>
        <td>HEX2OCT(100) <br>>> </td>
    </tr>
    <tr>
        <td>HEX2DEC ★</td><td>HEX2DEC(number)<br>Hexadecimal to decimal </td>
        <td>HEX2DEC(100) <br>>> </td>
    </tr>
    <tr>
        <td>OCT2BIN ★</td><td>OCT2BIN(number[, digit])<br>Octal to binary </td>
        <td>OCT2BIN(100) <br>>> </td>
    </tr>
    <tr>
        <td>OCT2DEC ★</td><td>OCT2DEC(number)<br>Octal to decimal </td>
        <td>OCT2DEC(100) <br>>> </td>
    </tr>
    <tr>
        <td>OCT2HEX ★</td><td>OCT2HEX(number[, digit])<br>Octal to hexadecimal </td>
        <td>OCT2HEX(100) <br>>> </td>
    </tr>
    <tr>
        <td>BIN2OCT ★</td><td>BIN2OCT(number[, digit])<br>Binary to octal </td>
        <td>BIN2OCT(100) <br>>> </td>
    </tr>
    <tr>
        <td>BIN2DEC ★</td><td>BIN2DEC(number)<br>Binary to decimal </td>
        <td>BIN2DEC(100) <br>>> </td>
    </tr>
    <tr>
        <td>BIN2HEX ★</td><td>BIN2HEX(number[, digit])<br>Binary to hexadecimal </td>
        <td>BIN2HEX(100) <br>>> </td>
    </tr>
</table>

#### Text function
<table>
    <tr><td>function name</td><td>description</td><td>Example</td></tr>
    <tr>
        <td>ASC ★</td><td>asc(text)<br>Change the full-width letters in a string to half-width characters</td>
        <td>asc('ａｂｃＡＢＣ１２３') <br>>>abcABC123</td>
    </tr>
    <tr>
        <td>JIS ★<br> WIDECHAR ★</td><td>jis(text)<br>Change half-width English characters in a string to full-width characters</td>
        <td>jis('abcABC123') <br>>>ａｂｃＡＢＣ１２３</td>
    </tr>
    <tr>
        <td>CHAR ★</td><td>CHAR(number)<br>Returns the character specified by the code value</td>
        <td>char(49) <br>>>1</td>
    </tr>
    <tr>
        <td>CLEAN ★</td><td>clean(text)<br>Delete all unprintable characters in the text</td>
        <td>clean('\r112\t') <br>>>112</td>
    </tr>
    <tr>
        <td>CODE ★</td><td>code(text)<br>Returns the numeric code of the first character in the text string</td>
        <td>CODE("1") <br>>>49</td>
    </tr>
    <tr>
        <td>CONCATENATE ★</td><td>concatenate(text1, ...)<br>Merge several text items into a single text item</td>
        <td>CONCATENATE('tt', '11') <br>>>tt11</td>
    </tr>
    <tr>
        <td>EXACT ★</td><td>exact(text1, text2)<br>Check whether the two text values are exactly the same</td>
        <td>EXACT("11", "22") <br>>>false</td>
    </tr>
    <tr style="color:red">
        <td>FIND ★ ▲</td><td>find(text, findText[, startIndex])<br>Find another text value within one text value (case sensitive) </td>
        <td>FIND("11", "12221122") <br>>>5</td>
    </tr>
    <tr>
        <td>LEFT ★</td><td>left(text[, count])<br>Returns the leftmost character of the text value</td>
        <td>LEFT('123222', 3) <br>>>123</td>
    </tr>
    <tr>
        <td>LEN ★</td><td>len(text)<br>Returns the number of characters in a text string</td>
        <td>LEN('123222') <br>>>6</td>
    </tr>
    <tr>
        <td>MID ★  ▲</td><td>mid(text, startIndex, count)<br>Returns a specific number of characters from a specified position in a text string</td>
        <td>MID('ABCDEF', 2, 3) <br>>>BCD</td>
    </tr>
    <tr>
        <td>PROPER ★</td><td>proper(text)<br>Set the first letter of each word in the text value to uppercase</td>
        <td>PROPER('abc abc') <br>>>Abc Abc</td>
    </tr>
    <tr>
        <td>REPLACE ★  ▲</td>
        <td>replace(srcText, startIndex, count, newText)<br>
        replace(srcText, repalceText, newText)<br>
        Replace characters in text</td>
        <td>REPLACE("abccd", 2, 3, "2") <br>>>a2d<br>
        REPLACE("abccd", "bc", "2") <br>>>a2cd
        </td>
    </tr>
    <tr>
        <td>REPT ★</td><td>rept(text, times)<br>Repeat the text a given number of times</td>
        <td>REPT("q", 3) <br>>>qqq</td>
    </tr>
    <tr>
        <td>RIGHT ★</td><td>right(text[, count])<br>Returns the rightmost character of the text value</td>
        <td>RIGHT("123q", 3) <br>>>23q</td>
    </tr>
    <tr>
        <td>RMB ★</td><td>RMB(number)<br>Convert numeric values to chinese uppercase numeric text</td>
        <td>RMB(12.3) <br>>>壹拾贰元叁角</td>
    </tr>
    <tr>
        <td>SEARCH ★ ▲</td><td>search(findText, text[, startIndex])<br>Find another text value in one text value (case-insensitive)</td>
        <td>SEARCH("aa", "abbAaddd") <br>>>4</td>
    </tr>
    <tr>
        <td>SUBSTITUTE ★</td><td>substitute(text, srcText, newText[, index])<br>Replace old text with new text in a text string</td>
        <td>SUBSTITUTE("ababcc", "ab", "12") <br>>>1212cc</td>
    </tr>
    <tr>
        <td>T ★</td><td>t(number)<br>Convert parameters to text</td>
        <td>T('123') <br>>>123</td>
    </tr>
    <tr>
        <td>TEXT ★</td><td>text(number, format)<br>Format numeric values and convert them to text</td>
        <td>TEXT(123, "0.00") <br>>>123.00</td>
    </tr>
    <tr>
        <td>TRIM ★</td><td>trim(text)<br>Delete spaces in text</td>
        <td>TRIM(" 123 123 ")<br>>>123 123</td>
    </tr>
    <tr>
        <td>LOWER ★<br>TOLOWER ★</td><td>lower(text)<br>tolower(text)<br>Convert text to lowercase</td>
        <td>LOWER('ABC') <br>>>abc</td>
    </tr>
    <tr>
        <td>UPPER ★<br>TOUPPER ★</td><td>upper(text)<br>toupper(text)<br>Convert text to uppercase</td>
        <td>UPPER("abc") <br>>>ABC</td>
    </tr>
    <tr>
        <td>VALUE ★</td><td>value(text)<br>Convert text parameters to numeric values</td>
        <td>VALUE("123") <br>>>123</td>
    </tr>
</table>

#### Date and time functions
<table>
    <tr><td>function name</td><td>description</td><td>Example</td></tr>
    <tr>
        <td>NOW</td><td>now()<br>Returns the current date and time</td>
        <td>NOW() <br>>>2017-01-07 11:00:00</td>
    </tr>
    <tr>
        <td>TODAY</td><td>today()<br>Return to today's date</td>
        <td>TODAY() <br>>>2017-01-07</td>
    </tr>
    <tr>
        <td>DateValue ★</td><td>DateValue(text)<br>Convert a text format to a date</td>
        <td>DateValue("2017-01-02") <br>>>2017-01-02</td>
    </tr>
    <tr>
        <td>TimeValue ★</td><td>TimeValue(text)<br>Convert text formatted time to date</td>
        <td>TimeValue("12:12:12") <br>>>12:12:12</td>
    </tr>
    <tr>
        <td>DATE ★</td><td>date(year, month, day[, hour[, minute[, second]]])<br>Returns a specific date</td>
        <td>DATE(2016, 1, 1) <br>>>2016-01-01</td>
    </tr>
    <tr>
        <td>TIME ★</td><td>time(hour, minute, second)<br>Returns a specific time</td>
        <td>TIME(12, 13, 14) <br>>>12:13:14</td>
    </tr>
    <tr>
        <td>YEAR ★</td><td>year(date)<br>Returns year</td>
        <td>YEAR(NOW()) <br>>>2017</td>
    </tr>
    <tr>
        <td>MONTH ★</td><td>month(date)<br>Returns month</td>
        <td>MONTH(NOW()) <br>>>1</td>
    </tr>
    <tr>
        <td>DAY ★</td><td>day(date)<br>Returns day</td>
        <td>DAY(NOW()) <br>>>7</td>
    </tr>
    <tr>
        <td>HOUR ★</td><td>hour(date)<br>Returns hour</td>
        <td>HOUR(NOW()) <br>>>11</td>
    </tr>
    <tr>
        <td>MINUTE ★</td><td>minute(date)<br>Returns minute</td>
        <td>MINUTE(NOW()) <br>>>12</td>
    </tr>
    <tr>
        <td>SECOND ★</td><td>second(date)<br>Returns second</td>
        <td>SECOND(NOW()) <br>>>34</td>
    </tr>
    <tr>
        <td>WEEKDAY ★</td><td>WEEKDAY(date)<br>Returns weekday</td>
        <td>WEEKDAY(date(2017, 1, 7)) <br>>>7</td>
    </tr>
    <tr>
        <td>dateDIF</td><td>dateDif(startDate, endDate, type:Y/M/D/YD/MD/YM)<br>Returns the number of days between two dates</td>
        <td>dateDIF("1975-1-30", "2017-1-7", "Y") <br>>>41</td>
    </tr>
    <tr>
        <td>DAYS360</td><td>days360(startDate, endDate[, type:0/1])<br>Calculate the number of days in a two-day period on the basis of 360 days a year</td>
        <td>DAYS360('1975-1-30', '2017-1-7') <br>>>15097</td>
    </tr>
    <tr>
        <td>EDATE</td><td>eDate(startDate, month)<br>Returns the serial number used to represent the number of months before or after the start date</td>
        <td>EDATE("2012-1-31", 32) <br>>>2014-09-30</td>
    </tr>
    <tr>
        <td>EOMONTH</td><td>eoMonth(startDate, month)<br>Returns the serial number of the last day of the month before or after the specified number of months</td>
        <td>EOMONTH("2012-2-1", 32) <br>>>2014-10-31</td>
    </tr>
    <tr>
        <td>netWorkdays</td><td>netWorkdays(startDate, endDate[, holidays])<br>Returns the total number of working days between two dates</td>
        <td>netWorkdays("2012-1-1", "2013-1-1") <br>>>262</td>
    </tr>
    <tr>
        <td>workDay</td><td>workday(startDate, days[, holidays])<br>Returns the serial number of the date before or after the specified number of working days</td>
        <td>workDay("2012-1-2", 145) <br>>>2012-07-23</td>
    </tr>
    <tr>
        <td>WEEKNUM</td><td>weekNum(date[, type：1/2])<br>Returns week number</td>
        <td>WEEKNUM("2016-1-3") <br>>>2</td>
    </tr>
</table>

##### Extension function
<table>
    <tr><td>function name</td><td>description</td><td>Example</td></tr>
    <tr>
        <td>AddYears ★</td><td>AddYears(date, number)<br>Add Years</td> <td></td>
    </tr>
	<tr>
        <td>AddMonths ★</td><td>AddMonths(date, number)<br>Add Months</td> <td></td>
    </tr>
	<tr>
        <td>AddDays ★</td><td>AddDays(date, number)<br>Add Days</td> <td></td>
    </tr>
    <tr>
        <td>AddHours ★</td><td>AddHours(date, number)<br>Add Hours</td> <td></td>
    </tr>
    <tr>
        <td>AddMinutes ★</td><td>AddMinutes(date, number)<br>Add Minutes</td> <td></td>
    </tr>
    <tr>
        <td>AddSeconds ★</td><td>AddSeconds(date, number)<br>Add Seconds</td> <td></td>
    </tr>
    <tr>
        <td>DateValue ★</td><td>DateValue(value, number)<br>Conversion time<br>
        DateValue(text/number, 0) <br>Parse, automatically match to a date similar to the current date<br>
        DateValue(text, 1) <br>Conversion date, text format<br>
        DateValue(number, 2) <br>Conversion date, Excel value<br>
        DateValue(number, 3) <br>Convert to date, timestamp (milliseconds)<br>
        DateValue(number, 4) <br>Convert to date, timestamp (seconds)<br>
        </td> <td></td>
    </tr>
    <tr>
        <td>Timestamp ★</td><td>Timestamp(date[, type:0/1])<br>Switch to timestamp. Default is millisecond.<br>
        Timestamp(date, 0) <br>Convert to timestamp (milliseconds)<br>
        Timestamp(date, 1) <br>Convert to timestamp (seconds)<br>
        </td> <td></td>
    </tr>
</table>

Note: The `UseLocalTime` attribute affects the conversion of `DateValue`/`Timestamp`. Set `true` to directly convert to local time.

#### Statistical function
<table>
    <tr><td>function name</td><td>description</td><td>Example</td></tr>
    <tr>
        <td>MAX</td><td>max(number, ...)<br>Returns the maximum value in the parameter list</td>
        <td>max(1, 2, 3, 4, 2, 2, 1, 4) <br>>>4</td>
    </tr>
    <tr>
        <td>MEDIAN</td><td>median(number, ...)<br>Returns the median of a given value</td>
        <td>MEDIAN(1, 2, 3, 4, 2, 2, 1, 4) <br>>>2</td>
    </tr>
    <tr>
        <td>MIN</td><td>min(number, ...)<br>Returns the minimum value in the parameter list</td>
        <td>MIN(1, 2, 3, 4, 2, 2, 1, 4) <br>>>1</td>
    </tr>
    <tr>
        <td>QUARTILE</td><td>quartile(number, quartile：0-4)<br>Returns the quartile of the dataset</td>
        <td>QUARTILE({1, 2, 3, 4, 2, 2, 1, 4}, 0) <br>>>1</td>
    </tr>
    <tr>
        <td>MODE</td><td>mode(number, ...)<br>Returns the number that occurs most frequently in the array</td>
        <td>MODE(1, 2, 3, 4, 2, 2, 1, 4) <br>>>2</td>
    </tr>
    <tr>
        <td>LARGE ▲</td><td>large(array, K)<br>Returns the k largest value of the data set</td>
        <td>LARGE({1, 2, 3, 4, 2, 2, 1, 4}, 3) <br>>>3</td>
    </tr>
    <tr>
        <td>SMALL ▲</td><td>small(number, K)<br>Returns the k-th minimum of the data set</td>
        <td>SMALL({1, 2, 3, 4, 2, 2, 1, 4}, 3) <br>>>2</td>
    </tr>
    <tr>
        <td>PERCENTILE</td><td>percentile(number, K)<br>Returns the k percentile in the area</td>
        <td>PERCENTILE({1, 2, 3, 4, 2, 2, 1, 4}, 0.4) <br>>>2</td>
    </tr>
    <tr>
        <td>PERCENTRANK</td><td>percentRank(number, K)<br>Returns the percentage ranking of the values in the data set</td>
        <td>PERCENTRANK({1, 2, 3, 4, 2, 2, 1, 4}, 3) <br>>>0.714</td>
    </tr>
    <tr>
        <td>AVERAGE</td><td>average(number, ...)<br>Returns the average value of the parameter</td>
        <td>AVERAGE(1, 2, 3, 4, 2, 2, 1, 4) <br>>>2.375</td>
    </tr>
    <tr>
        <td>averageIf</td><td>averageIf({number1, ...}, condition[, {number1, ...}])<br>Returns the average value of the parameter</td>
        <td>averageIf({1, 2, 3, 4, 2, 2, 1, 4}, '>1') <br>>>2.833333333</td>
    </tr>
    <tr>
        <td>GEOMEAN</td><td>geoMean(number, ...)<br>Returns the geometric mean of a positive array or region</td>
        <td>GEOMEAN(1, 2, 3, 4) <br>>>2.213363839</td>
    </tr>
    <tr>
        <td>HARMEAN</td><td>harMean(number, ...)<br>Returns the harmonic average of the data set</td>
        <td>HARMEAN(1, 2, 3, 4) <br>>>1.92</td>
    </tr>
    <tr>
        <td>COUNT</td><td>count(number, ...)<br>Calculate the number of values in the parameter list</td>
        <td>COUNT(1, 2, 3, 4, 2, 2, 1, 4) <br>>>8</td>
    </tr>
    <tr>
        <td>countIf</td><td>countIf({number1, ...}, condition[, {number1, ...}])<br>Calculate the number of values in the parameter list</td>
        <td>countIf({1, 2, 3, 4, 2, 2, 1, 4}, '>1') <br>>>6</td>
    </tr>
    <tr>
        <td>SUM</td><td>sum(number, ...)<br>Returns the sum of all values.</td>
        <td>SUM(1, 2, 3, 4) <br>>>10</td>
    </tr>
    <tr>
        <td>sumIf</td><td>sumIf({number, ...}, condition[, {number1, ...}])<br>Returns the sum of all values.</td>
        <td>sumIf({1, 2, 3, 4, 2, 2, 1, 4}, '>1') <br>>>17</td>
    </tr>
    <tr>
        <td>AVEDEV</td><td>aveDev(number, ...)<br>Returns the average of the absolute deviation of a data point from its average</td>
        <td>AVEDEV(1, 2, 3, 4, 2, 2, 1, 4) <br>>>0.96875</td>
    </tr>
    <tr>
        <td>STDEV</td><td>stDev(number, ...)<br>Estimation of standard deviation based on samples</td>
        <td>STDEV(1, 2, 3, 4, 2, 2, 1, 4) <br>>>1.1877349391654208</td>
    </tr>
    <tr>
        <td>STDEVP</td><td>stDevp(number, ...)<br>Calculate the standard deviation based on the whole sample population</td>
        <td>STDEVP(1, 2, 3, 4, 2, 2, 1, 4) <br>>>1.1110243021644486</td>
    </tr>
    <tr>
        <td>DEVSQ</td><td>devSq(number, ...)<br>Returns the sum of squares of deviations</td>
        <td>DEVSQ(1, 2, 3, 4, 2, 2, 1, 4) <br>>>9.875</td>
    </tr>
    <tr>
        <td>VAR</td><td>var(number, ...)<br>Estimation of variance based on samples</td>
        <td>VAR(1, 2, 3, 4, 2, 2, 1, 4) <br>>>1.4107142857142858</td>
    </tr>
    <tr>
        <td>VARP</td><td>varp(number, ...)<br>Calculate the variance based on the whole sample population</td>
        <td>VARP(1, 2, 3, 4, 2, 2, 1, 4) <br>>>1.234375</td>
    </tr>
    <tr>
        <td>normDist</td><td>normDist(number, arithmeticMean, StDev, returnType：0/1)<br>Return to normal cumulative distribution</td>
        <td>normDist(3, 8, 4, 1) <br>>>0.105649774</td>
    </tr>
    <tr>
        <td>normInv</td><td>normInv(distributionProbability, arithmeticMean, StDev)<br>Returns the anti-normal cumulative distribution</td>
        <td>normInv(0.8, 8, 3) <br>>>10.5248637</td>
    </tr>
    <tr>
        <td>NormSDist</td><td>normSDist(number)<br>Returns the standard normal cumulative distribution function, with an average of 0 and a standard deviation of 1.</td>
        <td>NORMSDist(1) <br>>>0.841344746</td>
    </tr>
    <tr>
        <td>normSInv</td><td>normSInv(number)<br>Return anti-standard normal cumulative distribution</td>
        <td>normSInv(0.3) <br>>>-0.524400513</td>
    </tr>
    <tr>
        <td>BetaDist</td><td>BetaDist(number, α, β)<br>Returns the Beta cumulative distribution function</td>
        <td>BetaDist(0.5, 11, 22) <br>>>0.97494877</td>
    </tr>
    <tr>
        <td>BetaInv</td><td>BetaInv(number, α, β)<br>Returns the inverse function of the cumulative distribution function of the specified Beta distribution</td>
        <td>BetaInv(0.5, 23, 45) <br>>>0.336640759</td>
    </tr>
    <tr>
        <td>binomDist</td><td>binomDist(successCount, testCount, successProbability, returnType：0/1)<br>Returns the probability of unary binomial distribution</td>
        <td>binomDist(12, 45, 0.5, 0) <br>>>0.000817409</td>
    </tr>
    <tr>
        <td>exponDist</td><td>exponDist(number, value, returnType：0/1)<br>Return exponential distribution</td>
        <td>exponDist(3, 1, 0) <br>>>0.049787068</td>
    </tr>
    <tr>
        <td>FDist</td><td>fDist(numberX, molecularFreedom, denominatorFreedom)<br>Return F probability distribution</td>
        <td>FDist(0.4, 2, 3) <br>>>0.701465776</td>
    </tr>
    <tr>
        <td>FInv</td><td>fInv(distributionProbability, molecularFreedom, denominatorFreedom)<br>Returns the inverse function of F probability distribution</td>
        <td>FInv(0.7, 2, 3) <br>>>0.402651432</td>
    </tr>
    <tr>
        <td>FISHER</td><td>fisher(number)<br>Returns the Fisher transformation of point x. The transformation produces a normal distribution rather than a skewed function.</td>
        <td>FISHER(0.68) <br>>>0.8291140383</td>
    </tr>
    <tr>
        <td>fisherInv</td><td>fisherInv(number)<br>Returns the inverse value of the Fisher transform.</td>
        <td>fisherInv(0.6) <br>>>0.537049567</td>
    </tr>
    <tr>
        <td>gammaDist</td><td>gammaDist(number, α, β, returnType：0/1)<br>Return gamma distribution</td>
        <td>gammaDist(0.5, 3, 4, 0) <br>>>0.001723627</td>
    </tr>
    <tr>
        <td>gammaInv</td><td>gammaInv(distributionProbability, α, β)<br>Returns the inverse function of the gamma cumulative distribution function</td>
        <td>gammaInv(0.2, 3, 4) <br>>>6.140176811</td>
    </tr>
    <tr>
        <td>GAMMALN</td><td>gammaLn(number)<br>Returns the natural logarithm of γ </td>
        <td>GAMMALN(4) <br>>>1.791759469</td>
    </tr>
    <tr>
        <td>hypgeomDist</td><td>hypgeomDist(successCount, testCount, successCountAll, testCountAll)<br>Returns the hypergeometric distribution</td>
        <td>hypgeomDist(23, 45, 45, 100) <br>>>0.08715016</td>
    </tr>
    <tr>
        <td>logInv</td><td>logInv(distributionProbability, average, StDev)<br>Returns the inverse function of the logarithmic cumulative distribution function of x </td>
        <td>logInv(0.1, 45, 33) <br>>>15.01122624</td>
    </tr>
    <tr>
        <td>LognormDist</td><td>lognormDist(number, average, StDev)<br>Returns the inverse normal distribution</td>
        <td>lognormDist(15, 23, 45) <br>>>0.326019201</td>
    </tr>
    <tr>
        <td>negbinomDist</td><td>negbinomDist(failureCount, successCount, successProbability)<br>Returns negative binomial distribution</td>
        <td>negbinomDist(23, 45, 0.7) <br>>>0.053463314</td>
    </tr>
    <tr>
        <td>POISSON</td><td>poisson(number, average, returnType：0/1)<br>Returns the Poisson distribution</td>
        <td>POISSON(23, 23, 0) <br>>>0.082884384</td>
    </tr>
    <tr>
        <td>TDist</td><td>tDist(number, freedom, returnType：1/2)<br>Returns the t distribution of students</td>
        <td>TDist(1.2, 24, 1) <br>>>0.120925677</td>
    </tr>
    <tr>
        <td>TInv</td><td>TInv(distributionProbability, freedom)<br>Returns the inverse distribution of students't-distribution</td>
        <td>TInv(0.12, 23) <br>>>1.614756561</td>
    </tr>
    <tr>
        <td>WEIBULL</td><td>weibull(number, α, β, returnType：0/1)<br>Returns the Weibull distribution</td>
        <td>WEIBULL(1, 2, 3, 1) <br>>>0.105160683</td>
    </tr>
</table>

#### Find references
<table>
    <tr><td>function name</td><td>description</td><td>Example</td></tr>
    <tr>
        <td>VLookUp ★ ▲</td><td>VLookUp({array, ...}, value, {colIndex}[, fuzzy:0/1])<br>Vertical search function. Fuzzy matching default 1</td> <td></td>
    </tr>
    <tr>
        <td>VLookUp ★ ▲</td><td>VLookUp({Json, ...}, formula, name)<br>JSON array lookup function.</td> <td></td>
    </tr>
</table>

#### Add function similar to C# method
<table>
    <tr><td>function name</td><td>description</td><td>Example</td></tr>
    <tr>
        <td>UrlEncode ★</td><td>UrlEncode(text)<br> Encode the URL string.</td> <td></td>
    </tr>
	<tr>
        <td>UrlDecode ★</td><td>UrlEncode(text)<br> Converts an URL-encoded string to a decoded string.</td> <td></td>
    </tr>
	<tr>
        <td>HtmlEncode ★</td><td>HtmlEncode(text)<br> Converts a string to a HTML-encoded string.</td> <td></td>
    </tr>
	<tr>
        <td>HtmlDecode ★</td><td>HtmlDecode(text)<br>  Transdecode the HTML-encoded string.</td> <td></td>
    </tr>
	<tr>
        <td>Base64ToText ★</td><td>Base64ToText(text[, encodingType])<br>   Converts Base64 to a string.</td> <td></td>
    </tr>
	<tr>
        <td>Base64UrlToText ★</td><td>Base64UrlToText(text[, encodingType])<br>   Converts a Base64 of type Url to a string.</td> <td></td>
    </tr>
	<tr>
        <td>TextToBase64 ★</td><td>TextToBase64(text[, encodingType])<br>   Converts a string to an Base64 string.</td> <td></td>
    </tr>
	<tr>
        <td>TextToBase64Url ★</td><td>TextToBase64Url(text[, encodingType])<br>   Converts a string to an Base64 string of type Url.</td> <td></td>
    </tr>
	<tr>
        <td>Regex ★ ▲</td><td>Regex(text, matchText)<br> returns a matching string.</td> <td></td>
    </tr>
	<tr>
        <td>RegexRepalce ★</td><td>RegexRepalce(text, matchText, replaceString)<br>  Matches the replacement string.</td> <td></td>
    </tr>
	<tr>
        <td>IsRegex ★<br>IsMatch ★</td><td>IsRegex(text, matchText)<br>IsMatch(text, matchText)<br>  To determine if there is a match.</td> <td></td>
    </tr>
	<tr>
        <td>Guid</td><td>Guid()<br>  Generate a Guid string.</td> <td></td>
    </tr>
	<tr>
        <td>Md5 ★</td><td>Md5(text[, encodingType])<br> Returns the Hash string of Md5.</td> <td></td>
    </tr>
	<tr>
        <td>Sha1 ★</td><td>Sha1(text[, encodingType])<br> Returns the Hash string of Sha1.</td> <td></td>
    </tr>
	<tr>
        <td>Sha256 ★</td><td>Sha256(text[, encodingType])<br> Returns the Hash string of Sha256.</td> <td></td>
    </tr>
	<tr>
        <td>Sha512 ★</td><td>Sha512(text[, encodingType])<br> Returns the Hash string of Sha512.</td> <td></td>
    </tr>
	<tr>
        <td>Crc32 ★</td><td>Crc32(text[, encodingType])<br> Returns the Hash string of Crc32.</td> <td></td>
    </tr>
	<tr>
        <td>HmacMd5 ★</td><td>HmacMd5(text, secret[, encodingType])<br> Returns the Hash string of HmacMd5.</td> <td></td>
    </tr>
	<tr>
        <td>HmacSha1 ★</td><td>HmacSha1(text, secret[, encodingType])<br> Returns the Hash string of HmacSha1.</td> <td></td>
    </tr>
	<tr>
        <td>HmacSha256 ★</td><td>HmacSha256(text, secret[, encodingType])<br> Returns the Hash string of HmacSha256.</td> <td></td>
    </tr>
	<tr>
        <td>HmacSha512 ★</td><td>HmacSha512(text, secret[, encodingType])<br> Returns the Hash string of HmacSha512.</td> <td></td>
    </tr>
	<tr>
        <td>TrimStart ★<br>LTrim ★</td><td>TrimStart(text)<br>LTrim(text)<br>LTrim(text[, characterSet])<br>   Empty the left side of the string.</td> <td></td>
    </tr>
	<tr>
        <td>TrimEnd ★<br>RTrim ★</td><td>TrimEnd(text)<br>RTrim(text)<br>RTrim(text, characterSet)<br>   Empty the right side of the string.</td> <td></td>
    </tr>
	<tr>
        <td>IndexOf ★ ▲</td><td>IndexOf(text, find[, start[, index]])<br>   Find the position of the string.</td> <td></td>
    </tr>
	<tr>
        <td>LastIndexOf ★ ▲</td><td>LastIndexOf(text, find[, start[, index]])<br>   Find the position of the string.</td> <td></td>
    </tr>
	<tr>
        <td>Split ★</td><td>Split(text, separator)<br> Generate array<br>Split(text, separator, index)<br>  Returns the string pointed to by the split index.</td> <td></td>
    </tr>
	<tr>
        <td>Join ★</td><td>Join(text1, text2....)<br>  Merge strings.</td> <td></td>
    </tr>
	<tr>
        <td>Substring ★ ▲</td><td>Substring(text, start)<br>Substring(text, start, count)<br>  Cut the string.</td> <td></td>
    </tr>
	<tr>
        <td>StartsWith ★</td><td>StartsWith(text, startText[, ignoreCase:1/0])<br>  Determines whether the beginning of this string instance matches the specified string.</td> <td></td>
    </tr>
	<tr>
        <td>EndsWith ★</td><td>EndsWith(text, startText[, ignoreCase:1/0])<br>  Determines whether the end of this string instance matches the specified string when comparing using the specified comparison option.</td> <td></td>
    </tr>
	<tr>
        <td>IsNullOrEmpty ★</td><td>IsNullOrEmpty(text)<br>  Indicates whether the specified string is null or an empty string.</td> <td></td>
    </tr>	
	<tr>
        <td>IsNullOrWhiteSpace ★</td><td>IsNullOrWhiteSpace(text)<br>  Indicates whether the specified string is null, empty, or consisting only of white space characters.</td> <td></td>
    </tr>
	<tr>
        <td>RemoveStart ★</td><td>RemoveStart(text, leftText[, ignoreCase])<br>Match the left, and if you succeed, remove the left string.</td> <td></td>
    </tr>
	<tr>
        <td>RemoveEnd ★</td><td>RemoveEnd(text, rightText[, ignoreCase])<br>Match the right, and if you succeed, remove the string on the right.</td> <td></td>
    </tr>
	<tr>
        <td>Json ★</td><td>json(text)<br>Dynamic json query.</td> <td></td>
    </tr>
</table>


 