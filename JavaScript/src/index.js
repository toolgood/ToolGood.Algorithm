import { AlgorithmEngine } from './AlgorithmEngine.js';
import { AlgorithmEngineEx } from './AlgorithmEngineEx.js';
import { AlgorithmEngineHelper } from './AlgorithmEngineHelper.js';
import { Operand } from './Operand.js';
import { CalculationLogicEngine } from './CalculationLogic/CalculationLogicEngine.js';
import { CalculationLogicInfo } from './CalculationLogic/CalculationLogicInfo.js';
import { CalculationLogicType } from './CalculationLogic/CalculationLogicType.js';
import * as Enums from './Enums/index.js';

export {
  AlgorithmEngine,
  AlgorithmEngineEx,
  AlgorithmEngineHelper,
  Operand,
  CalculationLogicEngine,
  CalculationLogicInfo,
  CalculationLogicType,
  Enums
};

export default {
  AlgorithmEngine,
  AlgorithmEngineEx,
  AlgorithmEngineHelper,
  Operand,
  CalculationLogicEngine,
  CalculationLogicInfo,
  CalculationLogicType,
  Enums
};

// Browser support
if (typeof window !== 'undefined') {
  window.ToolGood = window.ToolGood || {};
  window.ToolGood.Algorithm = {
    AlgorithmEngine,
    AlgorithmEngineEx,
    AlgorithmEngineHelper,
    Operand,
    CalculationLogicEngine,
    CalculationLogicInfo,
    CalculationLogicType,
    Enums
  };
}