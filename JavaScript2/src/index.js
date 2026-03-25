import { AlgorithmEngineHelper } from './AlgorithmEngineHelper.js';
import { AntlrErrorListener } from './AntlrErrorTextWriter.js';


export {
  AlgorithmEngineHelper,
  AntlrErrorListener,
};

export default {
  AlgorithmEngineHelper,
  AntlrErrorListener,
};

// Browser support
if (typeof window !== 'undefined') {
  window.ToolGood = window.ToolGood || {};
  window.ToolGood.Algorithm = {
    AlgorithmEngineHelper,
    AntlrErrorListener,
  };
}