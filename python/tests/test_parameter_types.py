"""Port of ``ParameterTypeTest.java`` (Java JUnit) to pytest.

Each Java ``@Test`` method maps to one test function below, in the same order
and with the same assertions.  The Java pattern::

    FunctionBase fb = AlgorithmEngineHelper.ParseFormula("exp");
    List<ParameterType> list = fb.GetParameterTypes(new AlgorithmEngine());

becomes::

    types = AlgorithmEngineHelper.parse_formula("exp").get_parameter_types(AlgorithmEngine())
"""
from __future__ import annotations

import pytest

from toolgood_algorithm import AlgorithmEngine, AlgorithmEngineHelper, OperandType


# region 基础测试
def test_basic_number_parameter():
    types = AlgorithmEngineHelper.parse_formula("test+1").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].name == "test"
    assert types[0].type == OperandType.NUMBER


def test_basic_boolean_parameter():
    types = AlgorithmEngineHelper.parse_formula("if(test,2,3)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].name == "test"
    assert types[0].type == OperandType.BOOLEAN


def test_basic_text_parameter():
    types = AlgorithmEngineHelper.parse_formula("left(test,ww)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].name == "test"
    assert types[1].name == "ww"
    assert types[0].type == OperandType.TEXT


# endregion

# region 比较操作符测试
def test_compare_equal_number():
    types = AlgorithmEngineHelper.parse_formula("if(test=1,2,3)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].name == "test"
    assert types[0].type == OperandType.NUMBER
    assert types[0].operator == "=="
    assert types[0].value == "1"


def test_compare_equal_text():
    types = AlgorithmEngineHelper.parse_formula("if(test='123tt',2,3)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].name == "test"
    assert types[0].type == OperandType.TEXT
    assert types[0].operator == "=="
    assert types[0].value == "123tt"


def test_compare_greater_than_or_equal():
    types = AlgorithmEngineHelper.parse_formula("if(test>=1,2,3)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].name == "test"
    assert types[0].type == OperandType.NUMBER
    assert types[0].operator == ">="
    assert types[0].value == "1"


def test_compare_greater_than_or_equal_expression():
    types = AlgorithmEngineHelper.parse_formula("if(test>=1+1,2,3)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].name == "test"
    assert types[0].type == OperandType.NUMBER
    assert types[0].operator == ">="
    assert types[0].value == "2"


def test_compare_less_than():
    types = AlgorithmEngineHelper.parse_formula("if(test<100,1,0)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].name == "test"
    assert types[0].type == OperandType.NUMBER
    assert types[0].operator == "<"
    assert types[0].value == "100"


def test_compare_less_than_or_equal():
    types = AlgorithmEngineHelper.parse_formula("if(test<=50,1,0)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].name == "test"
    assert types[0].type == OperandType.NUMBER
    assert types[0].operator == "<="
    assert types[0].value == "50"


def test_compare_greater_than():
    types = AlgorithmEngineHelper.parse_formula("if(test>0,1,0)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].name == "test"
    assert types[0].type == OperandType.NUMBER
    assert types[0].operator == ">"
    assert types[0].value == "0"


def test_compare_not_equal():
    types = AlgorithmEngineHelper.parse_formula("if(test!=0,1,0)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].name == "test"
    assert types[0].type == OperandType.NUMBER
    assert types[0].operator == "!="
    assert types[0].value == "0"


# endregion

# region 逻辑运算符测试
def test_logic_and():
    types = AlgorithmEngineHelper.parse_formula("if(a && b,1,0)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].name == "a"
    assert types[0].type == OperandType.BOOLEAN
    assert types[1].name == "b"
    assert types[1].type == OperandType.BOOLEAN


def test_logic_or():
    types = AlgorithmEngineHelper.parse_formula("if(a || b,1,0)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].name == "a"
    assert types[0].type == OperandType.BOOLEAN
    assert types[1].name == "b"
    assert types[1].type == OperandType.BOOLEAN


def test_logic_not():
    types = AlgorithmEngineHelper.parse_formula("if(!a,1,0)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].name == "a"
    assert types[0].type == OperandType.BOOLEAN


def test_logic_complex():
    types = AlgorithmEngineHelper.parse_formula("if(a && b || c,1,0)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    assert types[0].type == OperandType.BOOLEAN
    assert types[1].type == OperandType.BOOLEAN
    assert types[2].type == OperandType.BOOLEAN


def test_logic_and_function():
    types = AlgorithmEngineHelper.parse_formula("and(a,b,c)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    assert types[0].type == OperandType.BOOLEAN
    assert types[1].type == OperandType.BOOLEAN
    assert types[2].type == OperandType.BOOLEAN


def test_logic_or_function():
    types = AlgorithmEngineHelper.parse_formula("or(a,b)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.BOOLEAN
    assert types[1].type == OperandType.BOOLEAN


# endregion

# region 数学运算测试
def test_math_add():
    types = AlgorithmEngineHelper.parse_formula("a+b").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.NUMBER


def test_math_subtract():
    types = AlgorithmEngineHelper.parse_formula("a-b").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.NUMBER


def test_math_multiply():
    types = AlgorithmEngineHelper.parse_formula("a*b").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.NUMBER


def test_math_divide():
    types = AlgorithmEngineHelper.parse_formula("a/b").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.NUMBER


def test_math_mod():
    types = AlgorithmEngineHelper.parse_formula("a%b").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.NUMBER


def test_math_abs():
    types = AlgorithmEngineHelper.parse_formula("abs(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


def test_math_sqrt():
    types = AlgorithmEngineHelper.parse_formula("sqrt(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


def test_math_round():
    types = AlgorithmEngineHelper.parse_formula("round(a,b)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.NUMBER


def test_math_ceiling():
    types = AlgorithmEngineHelper.parse_formula("ceiling(a,0.5)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


def test_math_floor():
    types = AlgorithmEngineHelper.parse_formula("floor(a,1)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


def test_math_power():
    types = AlgorithmEngineHelper.parse_formula("power(a,b)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.NUMBER


def test_math_sum():
    types = AlgorithmEngineHelper.parse_formula("sum(a,b,c)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.NUMBER
    assert types[2].type == OperandType.NUMBER


def test_math_max():
    types = AlgorithmEngineHelper.parse_formula("max(a,b,c)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    assert types[0].type == OperandType.NUMBER


def test_math_min():
    types = AlgorithmEngineHelper.parse_formula("min(a,b)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.NUMBER


def test_math_average():
    types = AlgorithmEngineHelper.parse_formula("average(a,b,c)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    assert types[0].type == OperandType.NUMBER


# endregion

# region 字符串函数测试
def test_string_left():
    types = AlgorithmEngineHelper.parse_formula("left(a,n)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.TEXT
    assert types[1].type == OperandType.NUMBER


def test_string_right():
    types = AlgorithmEngineHelper.parse_formula("right(a,n)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.TEXT
    assert types[1].type == OperandType.NUMBER


def test_string_mid():
    types = AlgorithmEngineHelper.parse_formula("mid(a,p1,p2)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    assert types[0].type == OperandType.TEXT
    assert types[1].type == OperandType.NUMBER
    assert types[2].type == OperandType.NUMBER


def test_string_len():
    types = AlgorithmEngineHelper.parse_formula("len(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.TEXT


def test_string_upper():
    types = AlgorithmEngineHelper.parse_formula("upper(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.TEXT


def test_string_lower():
    types = AlgorithmEngineHelper.parse_formula("lower(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.TEXT


def test_string_trim():
    types = AlgorithmEngineHelper.parse_formula("trim(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.TEXT


def test_string_replace():
    types = AlgorithmEngineHelper.parse_formula("replace(a,p1,p2,p3)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 4
    assert types[0].type == OperandType.TEXT
    assert types[1].type == OperandType.NUMBER
    assert types[2].type == OperandType.NUMBER


def test_string_substitute():
    types = AlgorithmEngineHelper.parse_formula("substitute(a,'old','new')").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.TEXT


def test_string_find():
    types = AlgorithmEngineHelper.parse_formula("find('test',a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.TEXT


def test_string_concatenate():
    types = AlgorithmEngineHelper.parse_formula("concatenate(a,b,c)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    assert types[0].type == OperandType.TEXT


def test_string_connect():
    types = AlgorithmEngineHelper.parse_formula("a & b").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.TEXT
    assert types[1].type == OperandType.TEXT


# endregion

# region 日期函数测试
def test_date_date():
    types = AlgorithmEngineHelper.parse_formula("date(yr,mon,dy)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.NUMBER
    assert types[2].type == OperandType.NUMBER


def test_date_date_with_time():
    types = AlgorithmEngineHelper.parse_formula("date(yr,mon,dy,hr,m1,s1)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 6
    for i in range(6):
        assert types[i].type == OperandType.NUMBER


def test_date_year():
    types = AlgorithmEngineHelper.parse_formula("year(dt)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.DATE


def test_date_month():
    types = AlgorithmEngineHelper.parse_formula("month(dt)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.DATE


def test_date_day():
    types = AlgorithmEngineHelper.parse_formula("day(dt)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.DATE


def test_date_hour():
    types = AlgorithmEngineHelper.parse_formula("hour(dt)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.DATE


def test_date_minute():
    types = AlgorithmEngineHelper.parse_formula("minute(dt)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.DATE


def test_date_second():
    types = AlgorithmEngineHelper.parse_formula("second(dt)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.DATE


def test_date_adddays():
    types = AlgorithmEngineHelper.parse_formula("adddays(dt,num)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.DATE
    assert types[1].type == OperandType.NUMBER


def test_date_datedif():
    types = AlgorithmEngineHelper.parse_formula("datedif(startDt,endDt,'d')").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.DATE
    assert types[1].type == OperandType.DATE


# endregion

# region 类型检查函数测试
def test_type_check_isnumber():
    types = AlgorithmEngineHelper.parse_formula("isnumber(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NONE


def test_type_check_istext():
    types = AlgorithmEngineHelper.parse_formula("istext(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NONE


def test_type_check_isnull():
    types = AlgorithmEngineHelper.parse_formula("isnull(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NONE


def test_type_check_isnullorempty():
    types = AlgorithmEngineHelper.parse_formula("isnullorempty(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NONE


def test_type_check_iserror():
    types = AlgorithmEngineHelper.parse_formula("iserror(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NONE


def test_type_check_iseven():
    types = AlgorithmEngineHelper.parse_formula("iseven(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


def test_type_check_isodd():
    types = AlgorithmEngineHelper.parse_formula("isodd(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


def test_type_check_islogical():
    types = AlgorithmEngineHelper.parse_formula("islogical(a)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NONE


# endregion

# region 流程控制函数测试
def test_flow_if_with_condition():
    types = AlgorithmEngineHelper.parse_formula("if(score>=60,'Pass','Fail')").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].name == "score"
    assert types[0].type == OperandType.NUMBER
    assert types[0].operator == ">="
    assert types[0].value == "60"


def test_flow_ifs():
    types = AlgorithmEngineHelper.parse_formula("ifs(a=1,'One',a=2,'Two',a=3,'Three')").get_parameter_types(
        AlgorithmEngine()
    )
    assert types is not None
    assert len(types) == 3
    assert types[0].name == "a"
    assert types[0].type == OperandType.NUMBER


def test_flow_iferror():
    types = AlgorithmEngineHelper.parse_formula("iferror(a/b,0)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.NUMBER


def test_flow_switch():
    types = AlgorithmEngineHelper.parse_formula("switch(a,b,c)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    assert types[0].name == "a"
    assert types[0].type == OperandType.NONE


# endregion

# region 条件统计函数测试
def test_conditional_sumif():
    types = AlgorithmEngineHelper.parse_formula("sumif(values,'>10')").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.ARRAY


def test_conditional_sumif_with_sum_range():
    types = AlgorithmEngineHelper.parse_formula("sumif(range,'>10',sumRange)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.ARRAY
    assert types[1].type == OperandType.ARRAY


def test_conditional_countif():
    types = AlgorithmEngineHelper.parse_formula("countif(values,'>5')").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.ARRAY


def test_conditional_averageif():
    types = AlgorithmEngineHelper.parse_formula("averageif(values,'>0')").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.ARRAY


# endregion

# region 三角函数测试
def test_trig_sin():
    types = AlgorithmEngineHelper.parse_formula("sin(angle)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


def test_trig_cos():
    types = AlgorithmEngineHelper.parse_formula("cos(angle)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


def test_trig_tan():
    types = AlgorithmEngineHelper.parse_formula("tan(angle)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


def test_trig_asin():
    types = AlgorithmEngineHelper.parse_formula("asin(val)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


def test_trig_acos():
    types = AlgorithmEngineHelper.parse_formula("acos(val)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


def test_trig_atan2():
    types = AlgorithmEngineHelper.parse_formula("atan2(y,x)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.NUMBER


def test_trig_degrees():
    types = AlgorithmEngineHelper.parse_formula("degrees(rad)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


def test_trig_radians():
    types = AlgorithmEngineHelper.parse_formula("radians(deg)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1
    assert types[0].type == OperandType.NUMBER


# endregion

# region 进制转换函数测试
def test_conversion_bin2dec():
    types = AlgorithmEngineHelper.parse_formula("bin2dec(binary)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1


def test_conversion_dec2bin():
    types = AlgorithmEngineHelper.parse_formula("dec2bin(decimal)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1


def test_conversion_hex2dec():
    types = AlgorithmEngineHelper.parse_formula("hex2dec(hex)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 1


# endregion

# region 复杂表达式测试
def test_complex_nested_if():
    types = AlgorithmEngineHelper.parse_formula("if(a>100,'High',if(a>50,'Medium','Low'))").get_parameter_types(
        AlgorithmEngine()
    )
    assert types is not None
    assert len(types) == 2
    assert types[0].name == "a"
    assert types[0].type == OperandType.NUMBER


def test_complex_multiple_parameters():
    types = AlgorithmEngineHelper.parse_formula("(a+b)*(c-d)/e()").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 4
    for p in types:
        assert p.type == OperandType.NUMBER


def test_complex_mixed_types():
    types = AlgorithmEngineHelper.parse_formula(
        "if(amount>1000, 'High: ' & name, 'Low')"
    ).get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].name == "amount"
    assert types[0].type == OperandType.NUMBER
    assert types[1].name == "name"
    assert types[1].type == OperandType.TEXT


def test_complex_date_calculation():
    types = AlgorithmEngineHelper.parse_formula("adddays(startDate, duration)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.DATE
    assert types[1].type == OperandType.NUMBER


def test_complex_score_grade():
    types = AlgorithmEngineHelper.parse_formula(
        "ifs(score>=90,'A',score>=80,'B',score>=70,'C',score>=60,'D',true,'F')"
    ).get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 4
    assert types[0].name == "score"
    assert types[0].type == OperandType.NUMBER


def test_complex_string_manipulation():
    types = AlgorithmEngineHelper.parse_formula(
        "upper(left(name,1)) & lower(mid(name,2,len(name)-1))"
    ).get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    assert types[0].name == "name"
    assert types[0].type == OperandType.TEXT


def test_complex_conditional_sum():
    types = AlgorithmEngineHelper.parse_formula("sumif(amounts,'>0')/countif(amounts,'>0')").get_parameter_types(
        AlgorithmEngine()
    )
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.ARRAY


def test_complex_boolean_expression():
    types = AlgorithmEngineHelper.parse_formula(
        "if((age>=18 && age<=65) || isVIP, 'Eligible', 'Not Eligible')"
    ).get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    assert types[0].name == "age"
    assert types[0].type == OperandType.NUMBER
    assert types[2].name == "isVIP"
    assert types[2].type == OperandType.BOOLEAN


# endregion

# region 金融函数测试
def test_financial_pmt():
    types = AlgorithmEngineHelper.parse_formula("pmt(arg1,arg2,arg3)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.NUMBER
    assert types[2].type == OperandType.NUMBER


def test_financial_pv():
    types = AlgorithmEngineHelper.parse_formula("pv(arg1,arg2,arg3)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    for p in types:
        assert p.type == OperandType.NUMBER


def test_financial_fv():
    types = AlgorithmEngineHelper.parse_formula("fv(arg1,arg2,arg3)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3
    for p in types:
        assert p.type == OperandType.NUMBER


def test_financial_npv():
    types = AlgorithmEngineHelper.parse_formula("npv(arg1,arg2)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.NUMBER


# endregion

# region 数组函数测试
def test_array_array():
    types = AlgorithmEngineHelper.parse_formula("array(a,b,c)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 3


def test_array_large():
    types = AlgorithmEngineHelper.parse_formula("large(arr,k)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.ARRAY
    assert types[1].type == OperandType.NUMBER


def test_array_small():
    types = AlgorithmEngineHelper.parse_formula("small(arr,k)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.ARRAY
    assert types[1].type == OperandType.NUMBER


def test_array_rank():
    types = AlgorithmEngineHelper.parse_formula("rank(num,ref)").get_parameter_types(AlgorithmEngine())
    assert types is not None
    assert len(types) == 2
    assert types[0].type == OperandType.NUMBER
    assert types[1].type == OperandType.ARRAY


# endregion
