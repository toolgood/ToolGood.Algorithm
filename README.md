ToolGood.Algorithm
===================

#### 快速上手
``` csharp
    AlgorithmEngine engine = new AlgorithmEngine();
    double t=0.0;
    if (engine.Parse("1+2")) {
        t = (double)engine.Evaluate();
    }
    var c = engine.TryEvaluate("2+3", 0);
```


#### 类Excel公式
公式：`流程`、`数学`、`字符串`

##### 流程公式
<table>
    <tr><td>公式名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>IF</td>
        <td>指定要执行的逻辑检测</td>
        <td>if(1=1,1,2) ->1</td>
    </tr>
    <tr>
        <td>IFERROR</td>
        <td>指定要执行的逻辑检测</td>
        <td>iferror(1/0,1,2) ->1</td>
    </tr>
    <tr>
        <td>IFNUMBER</td>
        <td>指定要执行的逻辑检测</td>
        <td>ifnumber(4,1,2) ->1</td>
    </tr>
    <tr>
        <td>IFTEXT</td>
        <td>指定要执行的逻辑检测</td>
        <td>iftext('a',1,2) ->1</td>
    </tr>
    <tr>
        <td>ISNUMBER</td>
        <td>判断是否数字</td>
        <td>ISNUMBER(1) ->true</td>
    </tr>
    <tr>
        <td>ISTEXT</td>
        <td>判断是否文字</td>
        <td>istext('1') ->true </td>
    </tr>
    <tr>
        <td>AND</td>
        <td>如果所有参数均为TRUE，则返回TRUE</td>
        <td>and(1,2=2) ->true</td>
    </tr>
    <tr>
        <td>OR</td>
        <td>如果任一参数为TRUE，则返回TRUE</td>
        <td>or(1,2=3) ->true</td>
    </tr>
    <tr>
        <td>NOT</td>
        <td>对参数的逻辑值求反</td>
        <td>NOT(true()) ->false</td>
    </tr>
    <tr>
        <td>TRUE</td>
        <td>返回逻辑值TRUE</td>
        <td>true() ->true</td>
    </tr>
    <tr>
        <td>FALSE</td>
        <td>返回逻辑值FALSE</td>
        <td>false() ->false</td>
    </tr>
</table>

#### 数学公式
| 公式名               | 说明                 | 示例                 |
|: ------------------ :|: ------------------ :|: ------------------ :|
| abs                  | 返回数字的绝对值     | abs(-1)=1            |
| acos                 | 返回数字的反余弦值   | acos(0.5)=1 |
| acosh                | 返回数字的反双曲余弦值     | abs(-1)=1            |
| asin                 | 返回数字的反正弦值     | abs(-1)=1            |
| atan                 | 返回数字的反正切值     | abs(-1)=1            |
| atan2                | 从X和Y坐标返回反正切 | abs(-1)=1            |
| ceiling              | 将数字舍入为最接近的整数，或最接近的有效数字的倍数     | abs(-1)=1            |
| cos                  | 返回数字的余弦值     | abs(-1)=1            |
| cosh                 | 返回数字的双曲余弦值     | abs(-1)=1            |
| degrees              | 将弧度转换为度     | abs(-1)=1            |
| even                 | 将数字向上舍入为最接近的偶型整数     | abs(-1)=1            |
| exp                  | 返回e的指定数乘幂     | abs(-1)=1            |
| fact                 | 返回数字的阶乘     | abs(-1)=1            |
| factdouble           | 返回数字的双倍阶乘     | abs(-1)=1            |
| floor                | 将数字舍入为最接近的整数     | abs(-1)=1            |
| gcd                  | 返回最大公约数     | abs(-1)=1            |
| int                  | 返回      | abs(-1)=1            |
| lgm                  | 返回整数参数的最小公倍数      | abs(-1)=1            |
| ln                   | 返回数字的自然对数      | abs(-1)=1            |
| log                  | 返回数字的自然对数      | abs(-1)=1            |
| LOG10                | 返回数字的常用对数      | abs(-1)=1            |
| MULTINOMIAL          | 返回参数和的阶乘与各参数阶乘乘积的比值      | abs(-1)=1            |
| mod                  | 返回      | abs(-1)=1            |
| MROUND                  | 返回      | abs(-1)=1            |
| int                  | 返回      | abs(-1)=1            |
| int                  | 返回      | abs(-1)=1            |






