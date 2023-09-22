package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.enums.AreaUnitType;
import toolgood.algorithm.enums.DistanceUnitType;
import toolgood.algorithm.enums.MassUnitType;
import toolgood.algorithm.enums.VolumeUnitType;

import static org.junit.Assert.assertEquals;

public class AlgorithmEngineTest_v3_5 {
    // 取消参数格式  {格式}  改成  {key:val,key2:v2}  {1,2,3,}
    // 由于 BigDecimal 精度问题 （java版本问题）  公式 1=0.001*1000 返回 false， 为了绝大部分的正确， 比较前将精度缩小到12位再进行比较

    @Test
    public void PARAM_test() {
        // PARAM 动态获取参数 
        Cylinder engine = new Cylinder(10, 15);
        int num = engine.TryEvaluate("PARAM('半径')", 0);
        assertEquals(num, 10);
        num = engine.TryEvaluate("PARAMETER('半径')", 0);
        assertEquals(num, 10);
        num = engine.TryEvaluate("GETPARAMETER('半径')", 0);
        assertEquals(num, 10);

        // 参数名称没有限制了
        num = engine.TryEvaluate("半径", 0);
        assertEquals(num, 10);
    }

    @Test
    public void Error_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String num = engine.TryEvaluate("Error('出错了')", "");
        assertEquals(num, "");
        assertEquals(engine.LastError, "出错了");
    }

    @Test
    public void Json_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String str = engine.TryEvaluate("{name:'toolgood', age:'12',}['name']", "");
        assertEquals(str, "toolgood");

        str = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}['other']['work']", "");
        assertEquals(str, "IT");

        // 使用json 方法 使用比较标准的 json格式， 不然会出错  
        str = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\")['name']", "");
        assertEquals(str, "toolgood");

        str = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\")['other']['work']", "");
        assertEquals(str, "IT");

        // 'HAS' | 'HASKEY' |'CONTAINS'|'CONTAINSKEY' 指向同一函数  只支持数组与json类型
        boolean b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('age')", false);
        assertEquals(b, true);
        b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasKey('age')", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").has('age')", false);
        assertEquals(b, true);

        // 注意只能获取第一层
        b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('work')", true);
        assertEquals(b, false);


        // 'HASVALUE' | 'CONTAINSVALUE' 指向同一函数   只支持数组与json类型
        b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasValue('toolgood')", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").hasValue('toolgood')", false);
        assertEquals(b, true);
    }

    @Test
    public void array_test() {
        AlgorithmEngine engine = new AlgorithmEngine();

        // 'HAS' | 'HASKEY' |'CONTAINS'|'CONTAINSKEY' 指向同一函数
        // 'HASVALUE' | 'CONTAINSVALUE' 指向同一函数 与上面的逻辑相同
        boolean b = engine.TryEvaluate("{1,2,3,4,}.has('1')", false);
        assertEquals(b, true);
        b = engine.TryEvaluate("{'abc','age'}.hasKey('age')", false);
        assertEquals(b, true);
        b = engine.TryEvaluate("{'abc','age'}.hasValue('age')", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("json(\"['abc','age']\").has('age')", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("json(\"['abc','age']\").hasValue('age')", false);
        assertEquals(b, true);

    }


    @Test
    public void Distance_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean b = engine.TryEvaluate("1=1m", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=10dm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=100cm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000mm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.001km", false);
        assertEquals(b, true);

        engine.DistanceUnit = DistanceUnitType.DM;
        b = engine.TryEvaluate("1=0.1m", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1dm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=10cm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=100mm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.0001km", false);
        assertEquals(b, true);

        engine.DistanceUnit = DistanceUnitType.CM;
        b = engine.TryEvaluate("1=0.01m", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.1dm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1cm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=10mm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.00001km", false);
        assertEquals(b, true);

        engine.DistanceUnit = DistanceUnitType.MM;
        b = engine.TryEvaluate("1=0.001m", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.01dm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.1cm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1mm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.000001km", false);
        assertEquals(b, true);

        engine.DistanceUnit = DistanceUnitType.KM;

        b = engine.TryEvaluate("1=1m*1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=10dm*1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=100cm*1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000mm*1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.001km*1000", false);
        assertEquals(b, true);
    }

    @Test
    public void Area_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean b = engine.TryEvaluate("1=1m*1m", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m2=1m*1m", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m2=10dm*10dm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m2=100cm*100cm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m2=1000mm*1000mm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m2=0.001km*0.001km", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1m2", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=100dm2", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=10000cm2", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000mm2", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.000001km2", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1km2=1km*1km", false);
        assertEquals(b, true);

        engine.AreaUnit = AreaUnitType.DM2;

        b = engine.TryEvaluate("1=1m2/100", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=100dm2/100", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=10000cm2/100", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000mm2/100", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.000001km2/100", false);
        assertEquals(b, true);

        engine.AreaUnit = AreaUnitType.CM2;

        b = engine.TryEvaluate("1=1m2/100/100", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=100dm2/100/100", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=10000cm2/100/100", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000mm2/100/100", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.000001km2/100/100", false);
        assertEquals(b, true);

        engine.AreaUnit = AreaUnitType.MM2;

        b = engine.TryEvaluate("1=1m2/100/100/100", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=100dm2/100/100/100", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=10000cm2/100/100/100", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000mm2/100/100/100", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.000001km2/100/100/100", false);
        assertEquals(b, true);

        engine.AreaUnit = AreaUnitType.KM2;

        b = engine.TryEvaluate("1=1m2*1000*1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=100dm2*1000*1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=10000cm2*1000*1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000mm2*1000*1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.000001km2*1000*1000", false);
        assertEquals(b, true);


    }

    @Test
    public void Volume_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean b = engine.TryEvaluate("1=1m*1m*1m", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m3=1m*1m*1m", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m3=1m2*1m", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m3=1000L", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1L=1000ml", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m3=1000ml*1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m3=10dm*10dm*10dm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m3=100cm*100cm*100cm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m3=1000mm*1000mm*1000mm", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1m3=0.001km*0.001km*0.001km", false);
        assertEquals(b, true);


        b = engine.TryEvaluate("1=1m3", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000dm3", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000cm3", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000000mm3", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1km3/1000/1000/1000", false);
        assertEquals(b, true);

        engine.VolumeUnit = VolumeUnitType.DM3;
        b = engine.TryEvaluate("1=1m3/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000dm3/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000cm3/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000000mm3/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000", false);
        assertEquals(b, true);

        engine.VolumeUnit = VolumeUnitType.CM3;
        b = engine.TryEvaluate("1=1m3/1000/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000dm3/1000/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000cm3/1000/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000000mm3/1000/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000/1000", false);
        assertEquals(b, true);

        engine.VolumeUnit = VolumeUnitType.MM3;
        b = engine.TryEvaluate("1=1m3/1000/1000/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000dm3/1000/1000/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000cm3/1000/1000/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000000000mm3/1000/1000/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000/1000/1000", false);
        assertEquals(b, true);

        engine.VolumeUnit = VolumeUnitType.KM3;
        b = engine.TryEvaluate("1=1m3*1000*1000*1000", false);
        assertEquals(b, true);
        b = engine.TryEvaluate("1=1000dm3*1000*1000*1000", false);
        assertEquals(b, true);
        b = engine.TryEvaluate("1=1000000cm3*1000*1000*1000", false);
        assertEquals(b, true);
        b = engine.TryEvaluate("1=1000000000mm3*1000*1000*1000", false);
        assertEquals(b, true);
        b = engine.TryEvaluate("1=1km3", false);
        assertEquals(b, true);
    }


    @Test
    public void Mass_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean b = engine.TryEvaluate("1=1kg", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000g", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.001t", false);
        assertEquals(b, true);

        engine.MassUnit = MassUnitType.G;

        b = engine.TryEvaluate("1=1kg/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000g/1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.001t/1000", false);
        assertEquals(b, true);

        engine.MassUnit = MassUnitType.T;

        b = engine.TryEvaluate("1=1kg*1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=1000g*1000", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=0.001t*1000", false);
        assertEquals(b, true);
    }

    @Test
    public void Unit_Error_Test() {
        // 下面是错误 演示， 因为计算时不会考虑单位，所以下面是正常通过的
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean b = engine.TryEvaluate("1m=1kg", false);
        assertEquals(b, true);
        b = engine.TryEvaluate("1m=1m2", false);
        assertEquals(b, true);
        b = engine.TryEvaluate("1m=1m3", false);
        assertEquals(b, true);

    }
}
