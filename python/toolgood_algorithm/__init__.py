"""ToolGood.Algorithm - Python port of the C# / Java expression engine.

The package layout mirrors ``csharp/ToolGood.Algorithm`` so that a C# file can
be located in the Python tree directly::

    csharp/ToolGood.Algorithm/                 python/toolgood_algorithm/
      AlgorithmEngine.cs                       algorithm_engine.py
      AlgorithmEngineEx.cs                     algorithm_engine_ex.py
      AlgorithmEngineHelper.cs                 algorithm_engine_helper.py
      FunctionCache.cs / IFunctionCache.cs     function_cache.py
      CalculationLogic/                        calculation_logic/
      Enums/                                   enums/
      Internals/                               internals/
      LitJson/                                 lit_json/
      MathNet/                                 math_net/
      Operands/                                operands/
      UnitConversion/                          unit_conversion/
      math/ (ANTLR generated)                  math/

Quick start::

    from toolgood_algorithm import AlgorithmEngine

    engine = AlgorithmEngine()
    print(engine.try_evaluate("1+2*3", 0))      # 7
    print(engine.try_evaluate("ABS(-5)", 0))    # 5
"""
from __future__ import annotations

from .internals import functions  # noqa: F401  (registers all functions)
from .algorithm_engine import AlgorithmEngine, NoneEngine
from .algorithm_engine_ex import AlgorithmEngineEx
from .calculation_logic import CalculationLogicEngine, CalculationLogicInfo, CalculationLogicType
from .algorithm_engine_helper import (
    AlgorithmEngineHelper,
    CalculateTree,
    ConditionTree,
    DiyNameInfo,
    DiyNameKeyInfo,
)
from .enums import (
    AreaUnitType,
    CalculateTreeType,
    CombineCalculateType,
    ConditionTreeType,
    DistanceUnitType,
    MassUnitType,
    NumberUnitType,
    OperandType,
    VolumeUnitType,
)
from .function_cache import FunctionCache, IFunctionCache
from .internals.functions.function_base import FunctionBase
from .internals.parameter_type import ParameterType
from .lit_json import JsonData, JsonMapper
from .operands import KeyValue, MyDate, Operand

__version__ = "6.3.0"

__all__ = [
    # engines
    "AlgorithmEngine",
    "AlgorithmEngineEx",
    "NoneEngine",
    "AlgorithmEngineHelper",
    "FunctionCache",
    "IFunctionCache",
    "CalculationLogicEngine",
    "CalculationLogicInfo",
    "CalculationLogicType",
    # values
    "Operand",
    "OperandType",
    "MyDate",
    "JsonData",
    "JsonMapper",
    "KeyValue",
    # parsing
    "FunctionBase",
    "ParameterType",
    "CalculateTree",
    "ConditionTree",
    "DiyNameInfo",
    "DiyNameKeyInfo",
    # enums
    "AreaUnitType",
    "DistanceUnitType",
    "VolumeUnitType",
    "MassUnitType",
    "NumberUnitType",
    "CalculateTreeType",
    "ConditionTreeType",
    "CombineCalculateType",
]
