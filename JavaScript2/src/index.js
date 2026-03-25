import { AlgorithmEngineHelper } from './AlgorithmEngineHelper.js';

export {
  AlgorithmEngineHelper,
};

export default {
  AlgorithmEngineHelper,
};

// Browser support
if (typeof window !== 'undefined') {
  window.ToolGood = window.ToolGood || {};
  window.ToolGood.Algorithm = {
    AlgorithmEngineHelper,
  };
}