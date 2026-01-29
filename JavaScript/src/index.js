import { AlgorithmEngine } from './AlgorithmEngine.js';
import { Operand } from './Operand.js';
import * as Enums from './Enums/index.js';

export {
  AlgorithmEngine,
  Operand,
  Enums
};

export default {
  AlgorithmEngine,
  Operand,
  Enums
};

// Browser support
if (typeof window !== 'undefined') {
  window.ToolGood = window.ToolGood || {};
  window.ToolGood.Algorithm = {
    AlgorithmEngine,
    Operand,
    Enums
  };
}