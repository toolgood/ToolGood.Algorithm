import { AlgorithmEngineHelper } from './AlgorithmEngineHelper.js';
import antlr4 from './antlr4/index.web.js';

export {
  AlgorithmEngineHelper,
};

export default {
  AlgorithmEngineHelper,
};

// Browser support
if (typeof window !== 'undefined') {
  window.ToolGood = window.ToolGood || {};
  window.ToolGood.antlr4 = antlr4;
  window.ToolGood.Algorithm = {
    AlgorithmEngineHelper,
  };
}