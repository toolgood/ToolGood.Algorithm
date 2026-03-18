package toolgood.algorithm.Tests;

import org.junit.Test;
import static org.junit.Assert.*;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;

public class ValueTest {
    @Test
    public void constant_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double e = engine.TryEvaluate("e", 0.0);
        assertEquals(Math.E, e, 10);
        e = engine.TryEvaluate("pi", 0.0);
        assertEquals(Math.PI, e, 10);

        boolean b = engine.TryEvaluate("true", false);
        assertEquals(true, b);
        b = engine.TryEvaluate("false", true);
        assertEquals(false, b);
    }

    @Test
    public void boolean_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int b1 = engine.TryEvaluate("if(true,1,2)", 0);
        assertEquals(1, b1);

        b1 = engine.TryEvaluate("if(false,1,2)", 0);
        assertEquals(2, b1);
    }

    @Test
    public void array_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int r = engine.TryEvaluate("count(Array(1,2,3,4))", 0);
        assertEquals(4, r);

        r = engine.TryEvaluate("(1=1)*9+2", 0);
        assertEquals(11, r);
        r = engine.TryEvaluate("(1=2)*9+2", 0);
        assertEquals(2, r);
    }

    @Test
    public void TestVersion() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t25 = engine.TryEvaluate("Engineversion", "");
        assertEquals("ToolGood.Algorithm 6.2", t25);
        String t26 = engine.TryEvaluate("Algorithmversion", "");
        assertEquals("ToolGood.Algorithm 6.2", t26);
    }

    @Test
    public void Test_Json() throws Exception {
        AlgorithmEngine engine = new AlgorithmEngine();
        toolgood.algorithm.internals.functions.FunctionBase t = engine.Parse("{'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'}");
        String c = engine.Evaluate(t).toString();
        assertEquals("{\"灰色\":\"L\",\"canBookCount\":905,\"saleCount\":91,\"specId\":\"43b0e72e98731aed69e1f0cc7d64bf4d\"}", c);
    }

    @Test
    public void PARAM_test() {
        Cylinder engine = new Cylinder(10, 15);
        int num = engine.TryEvaluate("PARAM('半径')", 0);
        assertEquals(10, num);
        num = engine.TryEvaluate("PARAMETER('半径')", 0);
        assertEquals(10, num);
        num = engine.TryEvaluate("GETPARAMETER('半径')", 0);
        assertEquals(10, num);

        num = engine.TryEvaluate("半径", 0);
        assertEquals(10, num);
    }

    @Test
    public void Error_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String num = engine.TryEvaluate("Error('出错了')", "");
        assertEquals("", num);
        assertEquals("出错了", engine.LastError);
    }

    @Test
    public void Json_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String str = engine.TryEvaluate("{name:'toolgood', age:'12',}['name']", "");
        assertEquals("toolgood", str);

        str = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}['other']['work']", "");
        assertEquals("IT", str);

        str = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\")['name']", "");
        assertEquals("toolgood", str);

        str = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\")['other']['work']", "");
        assertEquals("IT", str);

        boolean b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('age')", false);
        assertEquals(true, b);
        b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasKey('age')", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("{e:'toolgood', pi:'12',other:{work:'IT'}}.hasKey('e')", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").has('age')", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('work')", true);
        assertEquals(false, b);

        b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasValue('toolgood')", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").hasValue('toolgood')", false);
        assertEquals(true, b);
    }

    @Test
    public void array_test2() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.setUseExcelIndex(true);
        int num = engine.TryEvaluate("[1,2,3,4,][2]", 0);
        assertEquals(2, num);

        String str = engine.TryEvaluate("[1,2,3,4,'555'][5]", "");
        assertEquals("555", str);

        boolean b = engine.TryEvaluate("[1,2,3,4,].has('1')", false);
        assertEquals(true, b);
        b = engine.TryEvaluate("['abc','age'].hasKey('age')", false);
        assertEquals(true, b);
        b = engine.TryEvaluate("['abc','age'].hasValue('age')", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("json(\"['abc','age']\").has('age')", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("json(\"['abc','age']\").hasValue('age')", false);
        assertEquals(true, b);
    }

    @Test
    public void Distance_M_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean b = engine.TryEvaluate("1=1m", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=10dm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=100cm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000mm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.001km", false);
        assertEquals(true, b);
    }

    @Test
    public void Distance_DM_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.DistanceUnit = toolgood.algorithm.enums.DistanceUnitType.DM;
        boolean b = engine.TryEvaluate("1=0.1m", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1dm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=10cm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=100mm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.0001km", false);
        assertEquals(true, b);
    }

    @Test
    public void Distance_CM_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.DistanceUnit = toolgood.algorithm.enums.DistanceUnitType.CM;
        boolean b = engine.TryEvaluate("1=0.01m", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.1dm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1cm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=10mm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.00001km", false);
        assertEquals(true, b);
    }

    @Test
    public void Distance_MM_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.DistanceUnit = toolgood.algorithm.enums.DistanceUnitType.MM;
        boolean b = engine.TryEvaluate("1=0.001m", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.01dm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.1cm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1mm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.000001km", false);
        assertEquals(true, b);
    }

    @Test
    public void Distance_KM_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.DistanceUnit = toolgood.algorithm.enums.DistanceUnitType.KM;

        boolean b = engine.TryEvaluate("1=1m*1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=10dm*1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=100cm*1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000mm*1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.001km*1000", false);
        assertEquals(true, b);
    }

    @Test
    public void Area_M2_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean b = engine.TryEvaluate("1=1m*1m", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m2=1m*1m", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m2=10dm*10dm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m2=100cm*100cm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m2=1000mm*1000mm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m2=0.001km*0.001km", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1m2", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=100dm2", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=10000cm2", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000mm2", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.000001km2", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1km2=1km*1km", false);
        assertEquals(true, b);
    }

    @Test
    public void Area_DM2_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.AreaUnit = toolgood.algorithm.enums.AreaUnitType.DM2;

        boolean b = engine.TryEvaluate("1=1m2/100", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=100dm2/100", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=10000cm2/100", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000mm2/100", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.000001km2/100", false);
        assertEquals(true, b);
    }

    @Test
    public void Area_CM2_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.AreaUnit = toolgood.algorithm.enums.AreaUnitType.CM2;

        boolean b = engine.TryEvaluate("1=1m2/100/100", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=100dm2/100/100", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=10000cm2/100/100", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000mm2/100/100", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.000001km2/100/100", false);
        assertEquals(true, b);
    }

    @Test
    public void Area_MM2_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.AreaUnit = toolgood.algorithm.enums.AreaUnitType.MM2;

        boolean b = engine.TryEvaluate("1=1m2/100/100/100", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=100dm2/100/100/100", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=10000cm2/100/100/100", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000mm2/100/100/100", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.000001km2/100/100/100", false);
        assertEquals(true, b);
    }

    @Test
    public void Area_KM2_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.AreaUnit = toolgood.algorithm.enums.AreaUnitType.KM2;

        boolean b = engine.TryEvaluate("1=1m2*1000*1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=100dm2*1000*1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=10000cm2*1000*1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000mm2*1000*1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.000001km2*1000*1000", false);
        assertEquals(true, b);
    }

    @Test
    public void Volume_M3_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean b = engine.TryEvaluate("1=1m*1m*1m", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m3=1m*1m*1m", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m3=1m2*1m", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m3=1000L", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1L=1000ml", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m3=1000ml*1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m3=10dm*10dm*10dm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m3=100cm*100cm*100cm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m3=1000mm*1000mm*1000mm", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1m3=0.001km*0.001km*0.001km", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1m3", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000dm3", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000cm3", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000000mm3", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1km3/1000/1000/1000", false);
        assertEquals(true, b);
    }

    @Test
    public void Volume_DM3_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.VolumeUnit = toolgood.algorithm.enums.VolumeUnitType.DM3;
        boolean b = engine.TryEvaluate("1=1m3/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000dm3/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000cm3/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000000mm3/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000", false);
        assertEquals(true, b);
    }

    @Test
    public void Volume_CM3_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.VolumeUnit = toolgood.algorithm.enums.VolumeUnitType.CM3;
        boolean b = engine.TryEvaluate("1=1m3/1000/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000dm3/1000/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000cm3/1000/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000000mm3/1000/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000/1000", false);
        assertEquals(true, b);
    }

    @Test
    public void Volume_MM3_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.VolumeUnit = toolgood.algorithm.enums.VolumeUnitType.MM3;
        boolean b = engine.TryEvaluate("1=1m3/1000/1000/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000dm3/1000/1000/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000cm3/1000/1000/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000000000mm3/1000/1000/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000/1000/1000", false);
        assertEquals(true, b);
    }

    @Test
    public void Volume_KM3_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.VolumeUnit = toolgood.algorithm.enums.VolumeUnitType.KM3;
        boolean b = engine.TryEvaluate("1=1m3*1000*1000*1000", false);
        assertEquals(true, b);
        b = engine.TryEvaluate("1=1000dm3*1000*1000*1000", false);
        assertEquals(true, b);
        b = engine.TryEvaluate("1=1000000cm3*1000*1000*1000", false);
        assertEquals(true, b);
        b = engine.TryEvaluate("1=1000000000mm3*1000*1000*1000", false);
        assertEquals(true, b);
        b = engine.TryEvaluate("1=1km3", false);
        assertEquals(true, b);
    }

    @Test
    public void Mass_KG_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean b = engine.TryEvaluate("1=1kg", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000g", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.001t", false);
        assertEquals(true, b);
    }

    @Test
    public void Mass_G_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.MassUnit = toolgood.algorithm.enums.MassUnitType.G;

        boolean b = engine.TryEvaluate("1=1kg/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000g/1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.001t/1000", false);
        assertEquals(true, b);
    }

    @Test
    public void Mass_T_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.MassUnit = toolgood.algorithm.enums.MassUnitType.T;

        boolean b = engine.TryEvaluate("1=1kg*1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=1000g*1000", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=0.001t*1000", false);
        assertEquals(true, b);
    }

    @Test
    public void Unit_Error_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean b = engine.TryEvaluate("1m=1kg", false);
        assertEquals(true, b);
        b = engine.TryEvaluate("1m=1m2", false);
        assertEquals(true, b);
        b = engine.TryEvaluate("1m=1m3", false);
        assertEquals(true, b);
    }
}