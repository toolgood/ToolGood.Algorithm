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
公式：`流程`、`数学`、`字符串`、`统计`、`日期`

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
<table>
    <tr><td>公式名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td>abs</td><td>返回数字的绝对值</td>
        <td>abs(-1)=1</td>
    </tr>
    <tr>
        <td>acos</td><td>返回数字的反余弦值</td>
        <td></td>
    </tr>
    <tr>
        <td>acosh</td><td>返回数字的反双曲余弦值</td>
        <td></td>
    </tr>
    <tr>
        <td>asin</td><td>返回数字的反正弦值</td>
        <td></td>
    </tr>
    <tr>
        <td>atan</td><td>返回数字的反正切值</td>
        <td></td>
    </tr>
    <tr>
        <td>atan2</td><td>从X和Y坐标返回反正切</td>
        <td></td>
    </tr>
    <tr>
        <td>ceiling</td><td>将数字舍入,取天花板值</td>
        <td></td>
    </tr>
    <tr>
        <td>cos</td><td>返回数字的余弦值</td>
        <td></td>
    </tr>
    <tr>
        <td>cosh</td><td>返回数字的双曲余弦值</td>
        <td></td>
    </tr>
    <tr>
        <td>degrees</td><td>将弧度转换为度</td>
        <td></td>
    </tr>
    <tr>
        <td>even</td><td>将数字向上舍入为最接近的偶型整数</td>
        <td></td>
    </tr>
    <tr>
        <td>exp</td><td>返回e的指定数乘幂</td>
        <td></td>
    </tr>
    <tr>
        <td>fact</td><td>返回数字的阶乘</td>
        <td></td>
    </tr>
    <tr>
        <td>factdouble</td><td>返回数字的双倍阶乘</td>
        <td></td>
    </tr>
    <tr>
        <td>floor</td><td>将数字舍入为最接近的整数，取地板值</td>
        <td></td>
    </tr>
    <tr>
        <td>gcd</td><td>返回最大公约数</td>
        <td></td>
    </tr>
    <tr>
        <td>int</td><td></td>
        <td></td>
    </tr>
    <tr>
        <td>lgm</td><td>返回整数参数的最小公倍数</td>
        <td></td>
    </tr>
    <tr>
        <td>ln</td><td>返回数字的自然对数</td>
        <td></td>
    </tr>
    <tr>
        <td>log</td><td>返回数字的常用对数</td>
        <td></td>
    </tr>
    <tr>
        <td>LOG10</td><td>返回数字的10对数</td>
        <td></td>
    </tr>
    <tr>
        <td>MULTINOMIAL</td><td>返回参数和的阶乘与各参数阶乘乘积的比值</td>
        <td></td>
    </tr>
    <tr>
        <td>mod</td><td>返回两数相除的余数</td>
        <td></td>
    </tr>
    <tr>
        <td>MROUND</td><td>返回一个舍入到所需倍数的数字</td>
        <td></td>
    </tr>
    <tr>
        <td>ODD</td><td>将数字向上舍入为最接近的奇型整数</td>
        <td></td>
    </tr>
    <tr>
        <td>PI</td><td>返回 PI 值</td>
        <td></td>
    </tr>
    <tr>
        <td>POWER</td><td>返回数的乘幂结果</td>
        <td></td>
    </tr>
    <tr>
        <td>PRODUCT</td><td>将所有以参数形式给出的数字相乘</td>
        <td></td>
    </tr>
    <tr>
        <td>QUOTIENT</td><td>返回商的整数部分，该函数可用于舍掉商的小数部分。</td>
        <td></td>
    </tr>
    <tr>
        <td>RADIANS</td><td>将度转换为弧度</td>
        <td></td>
    </tr>
    <tr>
        <td>RAND</td><td>返回 0 到 1 之间的随机数 </td>
        <td></td>
    </tr>
    <tr>
        <td>RANDBETWEEN</td><td>返回指定数字之间的随机数</td>
        <td></td>
    </tr>
    <tr>
        <td>ROUND</td><td>将数字舍入到指定位数</td>
        <td></td>
    </tr>
    <tr>
        <td>ROUNDDOW</td><td>将数字朝零的方向舍入</td>
        <td></td>
    </tr>
    <tr>
        <td>ROUNDUP</td><td>将数朝远离零的方向舍入</td>
        <td></td>
    </tr>

</table>


#### 字符串公式
<table>
    <tr><td>公式名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td></td>
        <td></td>
        <td></td>
    </tr>
</table>


#### 统计公式
<table>
    <tr><td>公式名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td></td>
        <td></td>
        <td></td>
    </tr>
</table>

#### 日期公式
<table>
    <tr><td>公式名</td><td>说明</td><td>示例</td></tr>
    <tr>
        <td></td>
        <td></td>
        <td></td>
    </tr>
</table>
