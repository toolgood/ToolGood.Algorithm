import { AlgorithmEngine } from './AlgorithmEngine.js';
import { AlgorithmEngineEx } from './AlgorithmEngineEx.js';
import { AlgorithmEngineHelper } from './AlgorithmEngineHelper.js';
import * as Enums from './Enums/index.js';

export {
  AlgorithmEngine,
  AlgorithmEngineEx,
  AlgorithmEngineHelper,
  Enums
};

export default {
  AlgorithmEngine,
  AlgorithmEngineEx,
  AlgorithmEngineHelper,
  Enums
};

// Browser support
if (typeof window !== 'undefined') {
  window.ToolGood = window.ToolGood || {};
  window.ToolGood.Algorithm = {
    AlgorithmEngine,
    AlgorithmEngineEx,
    AlgorithmEngineHelper,
    Enums
  };
}